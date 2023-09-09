using Cyclon.Console.Resolvers;
using Cyclon.Core;
using System.Security.Cryptography;
using System.Text;

namespace Cyclon.Console.Handlers;

internal sealed class EncryptFileCommandHandler : IHandler<EncryptCommandContext>
{
    private readonly IKeyIVGenerator _keyIVGenerator;

    public EncryptFileCommandHandler(IKeyIVGenerator keyIVGenerator)
    {
        _keyIVGenerator = keyIVGenerator;
    }

    public async Task Handle(EncryptCommandContext encryptCommandContext, CancellationToken cancellationToken)
    {
        try
        {
            var secretBytes = Encoding.Unicode.GetBytes(encryptCommandContext.Passphrase);
            var (key, iv) = _keyIVGenerator.Generate(secretBytes);
            var options = new EncryptionOptions(key, iv);

            if (FileUtilities.IsAFile(encryptCommandContext.FilePath))
            {
                var rootDirectory = CreateRootDirectory(encryptCommandContext.FilePath);
                await EncryptFile(encryptCommandContext.FilePath, rootDirectory, options, cancellationToken);
            }
            else
            {
                var fileSystem = new FileSystem(encryptCommandContext.FilePath);
                var rootDirectory = CreateRootDirectory(encryptCommandContext.FilePath);
                await Encrypt(rootDirectory, fileSystem.RootFolder, options, cancellationToken);
            }
        }
        catch (CryptographicException)
        {
            throw new InvalidOperationException("An exception occured during file encryption.");
        }
    }


    private async Task Encrypt(DirectoryInfo parentDirectory, FileSystem.Folder folder, EncryptionOptions options, CancellationToken cancellationToken)
    {
        var directory = Directory.CreateDirectory(Path.Combine(parentDirectory.FullName, folder.Name));

        foreach (var path in folder.FilePaths)
        {
            await EncryptFile(path, directory, options, cancellationToken);
        }

        foreach (var childFolder in folder.Folders)
        {
            await Encrypt(directory, childFolder, options, cancellationToken);
        }
    }

    private static async Task EncryptFile(
        string sourceFilePath,
        DirectoryInfo directory,
        EncryptionOptions options,
        CancellationToken cancellationToken)
    {
        var encryptedFileName = FileUtilities.GetEncryptedFileName(sourceFilePath);

        using (var readStream = new FileStream(sourceFilePath, FileMode.Open))
        {
            var path = Path.Combine(directory.FullName, encryptedFileName);
            var fileWriter = new FileWriter(path);
            await new AesEncryptor(fileWriter).Encrypt(readStream, options, cancellationToken);
        }
    }

    private static DirectoryInfo CreateRootDirectory(string path)
    {
        return Directory.CreateDirectory(Path.Combine(path, "cyclonenc")); ;
    }
}

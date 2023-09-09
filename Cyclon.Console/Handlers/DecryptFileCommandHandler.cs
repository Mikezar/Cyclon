using Cyclon.Console.Resolvers;
using Cyclon.Core;
using System.Security.Cryptography;
using System.Text;

namespace Cyclon.Console.Handlers;

internal sealed class DecryptFileCommandHandler : IHandler<DecryptCommandContext>
{
    private readonly IKeyIVGenerator _keyIVGenerator;

    public DecryptFileCommandHandler(IKeyIVGenerator keyIVGenerator)
    {
        _keyIVGenerator = keyIVGenerator;
    }

    public async Task Handle(DecryptCommandContext decryptCommandContext, CancellationToken cancellationToken)
    {
        try
        {
            var secretBytes = Encoding.Unicode.GetBytes(decryptCommandContext.Passphrase);
            var (key, iv) = _keyIVGenerator.Generate(secretBytes);
            var options = new EncryptionOptions(key, iv);

            if (FileUtilities.IsAFile(decryptCommandContext.FilePath))
            {
                var rootDirectory = CreateRootDirectory(decryptCommandContext.FilePath);
                await DecryptFile(decryptCommandContext.FilePath, rootDirectory, options, cancellationToken);
            }
            else
            {
                var fileSystem = new FileSystem(decryptCommandContext.FilePath);
                var rootDirectory = CreateRootDirectory(decryptCommandContext.FilePath);
                await DecryptFile(rootDirectory, fileSystem.RootFolder, options, cancellationToken);
            }
        }
        catch (CryptographicException)
        {
            throw new InvalidOperationException("An exception occured during file decryption.");
        }
    }

    private async Task DecryptFile(DirectoryInfo parentDirectory, FileSystem.Folder folder, EncryptionOptions options, CancellationToken cancellationToken)
    {
        var directory = Directory.CreateDirectory(Path.Combine(parentDirectory.FullName, folder.Name));

        foreach (var path in folder.FilePaths)
        {
            await DecryptFile(path, directory, options, cancellationToken);
        }

        foreach (var childFolder in folder.Folders)
        {
            await DecryptFile(directory, childFolder, options, cancellationToken);
        }
    }

    private static async Task DecryptFile(
        string sourceFilePath,
        DirectoryInfo directory,
        EncryptionOptions options,
        CancellationToken cancellationToken)
    {
        var decryptedFileName = FileUtilities.GetDecryptedFileName(sourceFilePath);

        using (var readStream = new FileStream(sourceFilePath, FileMode.Open))
        {
            var path = Path.Combine(directory.FullName, decryptedFileName);
            var fileWriter = new FileWriter(path);
            await new AesDecryptor(fileWriter).Decrypt(readStream, options, cancellationToken);
        }
    }

    private static DirectoryInfo CreateRootDirectory(string path)
    {
        return Directory.CreateDirectory(Path.Combine(path, "cyclondec")); ;
    }
}
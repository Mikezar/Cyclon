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
            var filePaths = FileUtilities.GetAllFilePaths(encryptCommandContext.FilePath);
            var directory = Directory.CreateDirectory(Path.Combine(encryptCommandContext.FilePath, "cyclonenc"));

            foreach (var path in filePaths)
            {
                await EncryptFile(path, directory, options, cancellationToken);
            }
        }
        catch (CryptographicException)
        {
            throw new InvalidOperationException("An exception occured during file encryption.");
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
}

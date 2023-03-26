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
            var filePaths = FileUtilities.GetAllFilePaths(decryptCommandContext.FilePath);
            var directory = Directory.CreateDirectory(Path.Combine(decryptCommandContext.FilePath, "cyclondec"));

            foreach (var path in filePaths)
            {
                await DecryptFile(path, directory, options, cancellationToken);
            }
        }
        catch (CryptographicException)
        {
            throw new InvalidOperationException("An exception occured during file decryption.");
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
}
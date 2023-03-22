using System.Security.Cryptography;

namespace Cyclon.Core;

public sealed class AesEncryptor : IEncryptor
{
    private readonly IWriter _writer;

    public AesEncryptor(IWriter writer)
    {
        _writer = writer;
    }

    public async Task Encrypt(Stream readStream, EncryptionOptions encryptionOptions, CancellationToken cancellationToken)
    {
        using var aes = Aes.Create();

        aes.Key = encryptionOptions.Key;
        aes.IV = encryptionOptions.IV;

        using var transform = aes.CreateEncryptor();
        using var writeStream = _writer.OpenStream();
        using var outStreamEncrypted = new CryptoStream(writeStream, transform, CryptoStreamMode.Write);

        int count = 0;
        int offset = 0;
        int blockSizeBytes = aes.BlockSize / 8;
        byte[] data = new byte[blockSizeBytes];
        int bytesRead = 0;

        do
        {
            count = await readStream.ReadAsync(data.AsMemory(0, blockSizeBytes), cancellationToken);
            await outStreamEncrypted.WriteAsync(data.AsMemory(0, count), cancellationToken);

            offset += count;
            bytesRead += blockSizeBytes;
        } while (count > 0);

        await outStreamEncrypted.FlushFinalBlockAsync(cancellationToken);
    }
}
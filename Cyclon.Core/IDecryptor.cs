namespace Cyclon.Core;

public interface IDecryptor
{
    Task Decrypt(Stream stream, EncryptionOptions encryptionOptions, CancellationToken cancellationToken);
}


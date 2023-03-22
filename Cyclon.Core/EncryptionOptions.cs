namespace Cyclon.Core;

public class EncryptionOptions
{
    public EncryptionOptions(byte[] key, byte[] iv)
    {
        Key = key;
        IV = iv;
    }

    public byte[] Key { get; }
    public byte[] IV { get; }
}

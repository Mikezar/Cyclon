namespace Cyclon.Core;

public interface IKeyIVGenerator
{
    public (byte[] key, byte[] iv) Generate(byte[] secret);
}

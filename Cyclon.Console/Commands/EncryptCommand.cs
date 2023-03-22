using System.CommandLine;

namespace Cyclon.Console.Commands;

internal sealed class EncryptCommand : Command
{
    public Option<string> Path { get; }

    public Option<string> Passphrase { get; }

    public EncryptCommand() : base("encrypt", "Encrypts file using AES algorithm in CBC mode.")
    {
        Path = new Option<string>("--path", "Defines a path to the file or to the folder.")
        {
            IsRequired = true
        };

        Passphrase = new Option<string>("--passphrase", "Sets the passphrase that will be used for generating key.")
        {
            IsRequired = true
        };

        AddOption(Path);
        AddOption(Passphrase);
    }
}

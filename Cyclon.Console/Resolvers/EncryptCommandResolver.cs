using Cyclon.Console.Commands;
using System.CommandLine.Parsing;

namespace Cyclon.Console.Resolvers;

internal sealed class EncryptCommandResolver : IResolver<EncryptCommand, EncryptCommandContext>
{
    public EncryptCommandContext Resolve(ParseResult parseResult, EncryptCommand command)
    {
        var filePath = parseResult.GetValueForOption(command.Path).OrThrow();
        var passphrase = parseResult.GetValueForOption(command.Passphrase).OrThrow();

        return new EncryptCommandContext(filePath, passphrase);
    }
}

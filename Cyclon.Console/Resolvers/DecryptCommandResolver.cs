using Cyclon.Console.Commands;
using System.CommandLine.Parsing;

namespace Cyclon.Console.Resolvers;

internal sealed class DecryptCommandResolver : IResolver<DecryptCommand, DecryptCommandContext>
{
    public DecryptCommandContext Resolve(ParseResult parseResult, DecryptCommand command)
    {
        var filePath = parseResult.GetValueForOption(command.Path).OrThrow();
        var passphrase = parseResult.GetValueForOption(command.Passphrase).OrThrow();

        return new DecryptCommandContext(filePath, passphrase);
    }
}

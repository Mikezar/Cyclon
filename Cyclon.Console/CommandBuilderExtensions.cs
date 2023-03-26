using Cyclon.Console.Commands;
using Cyclon.Console.Handlers;
using Cyclon.Console.Resolvers;

namespace Cyclon.Console;

internal static class CommandBuilderExtensions
{
    public static void RegisterCommands(this CommandBuilder commandBuilder, CancellationToken cancellationToken)
    {
        commandBuilder.AddCommand<EncryptCommand, EncryptCommandContext, EncryptCommandResolver, EncryptFileCommandHandler>(cancellationToken);
        commandBuilder.AddCommand<DecryptCommand, DecryptCommandContext, DecryptCommandResolver, DecryptFileCommandHandler>(cancellationToken);
    }
}

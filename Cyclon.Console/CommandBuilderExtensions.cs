using Cyclon.Console.Commands;
using Cyclon.Console.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Cyclon.Console;

internal static class CommandBuilderExtensions
{
    public static void RegisterCommands(this CommandBuilder commandBuilder, CancellationToken cancellationToken)
    {
        commandBuilder.AddCommand<EncryptCommand>(async (parseResult, command) =>
        {
            var filePath = parseResult.GetValueForOption(command.Path).OrThrow();
            var passphrase = parseResult.GetValueForOption(command.Passphrase).OrThrow();
            var handler = ServiceProviderAccessor.ServiceProvider.GetRequiredService<EncryptFileCommandHandler>();
            await handler.Handle(filePath, passphrase, cancellationToken);
        });

        commandBuilder.AddCommand<DecryptCommand>(async (parseResult, command) =>
        {
            var filePath = parseResult.GetValueForOption(command.Path).OrThrow();
            var passphrase = parseResult.GetValueForOption(command.Passphrase).OrThrow();
            var handler = ServiceProviderAccessor.ServiceProvider.GetRequiredService<DecryptFileCommandHandler>();
            await handler.Handle(filePath, passphrase, cancellationToken);
        });
    }
}

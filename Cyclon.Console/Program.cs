using Cyclon.Console;
using Cyclon.Console.Handlers;
using Cyclon.Core;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

CancellationTokenSource cancellationTokenSource = new();

Console.CancelKeyPress += (sender, eventArgs) =>
{
    cancellationTokenSource.Cancel();
    eventArgs.Cancel = true;
};

CyclonProcess.Start(RegisterDependencies, Run, cancellationTokenSource.Token);

IServiceProvider RegisterDependencies(CancellationToken cancellationToken)
{
    var serviceCollection = new ServiceCollection();

    serviceCollection.AddSingleton((_) =>
    {
        var commandBuilder = new CommandBuilder();
        commandBuilder.RegisterCommands(cancellationToken);
        return commandBuilder;
    });

    serviceCollection.AddSingleton<IKeyIVGenerator, Pbkdf2KeyIVGenerator>();
    serviceCollection.AddSingleton<EncryptFileCommandHandler>();
    serviceCollection.AddSingleton<DecryptFileCommandHandler>();

    var serviceProvider = serviceCollection.BuildServiceProvider();

    ServiceProviderAccessor.Create(serviceProvider);

    return serviceProvider;
}

async Task Run(IServiceProvider serviceProvider)
{
    var commandBuilder = serviceProvider.GetRequiredService<CommandBuilder>();
    var command = commandBuilder.GetRootCommand();
    await command.InvokeAsync(args);
}
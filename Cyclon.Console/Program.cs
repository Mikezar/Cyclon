using Cyclon.Console;
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
    var commandBuilder = new CommandBuilder(serviceCollection);
    commandBuilder.RegisterCommands(cancellationToken);

    serviceCollection.AddSingleton(commandBuilder);
    serviceCollection.AddSingleton<IKeyIVGenerator, Pbkdf2KeyIVGenerator>();

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
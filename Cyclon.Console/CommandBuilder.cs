using Cyclon.Console.Handlers;
using Cyclon.Console.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace Cyclon.Console;

internal sealed class CommandBuilder
{
    private readonly RootCommand _rootCommand;
    private readonly IServiceCollection _serviceCollection;

    public CommandBuilder(IServiceCollection serviceCollection)
    {
        _rootCommand = new RootCommand("Enter a command to perform an action.");
        _rootCommand.AddAlias("cyclon");
        _serviceCollection = serviceCollection;
    }

    public void AddCommand<TCommand, TContext, TResolver, THandler>(CancellationToken cancellationToken)
        where TCommand : Command
        where TContext : IResolutionContext
        where TResolver : IResolver<TCommand, TContext>
        where THandler : IHandler<TContext>
    {
        var command = (TCommand)Activator.CreateInstance(typeof(TCommand))!;

        command.SetHandler(async (context) =>
        {
            var resolver = ServiceProviderAccessor.ServiceProvider.GetRequiredService<IResolver<TCommand, TContext>>();
            var handler = ServiceProviderAccessor.ServiceProvider.GetRequiredService<IHandler<TContext>>();
            var resolvedCommand = resolver.Resolve(context.BindingContext.ParseResult, command);
            await handler.Handle(resolvedCommand, cancellationToken);
        });

        _rootCommand.AddCommand(command);
        _serviceCollection.AddSingleton(typeof(IHandler<TContext>), typeof(THandler));
        _serviceCollection.AddSingleton(typeof(IResolver<TCommand,TContext>), typeof(TResolver));
    }

    public RootCommand GetRootCommand()
    {
        return _rootCommand;
    }
}

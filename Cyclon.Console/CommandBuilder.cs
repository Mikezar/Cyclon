using System.CommandLine;
using System.CommandLine.Parsing;

namespace Cyclon.Console;

internal sealed class CommandBuilder
{
    private readonly RootCommand _rootCommand;

    public CommandBuilder()
    {
        _rootCommand = new RootCommand("Enter a command to perform an action.");
        _rootCommand.AddAlias("cyclon");
    }

    public void AddCommand<TCommand>(Func<ParseResult, TCommand, Task> action)  where TCommand : Command
    {
        var command = (TCommand)Activator.CreateInstance(typeof(TCommand))!;
        command.SetHandler(async (context) =>
        {
            await action(context.BindingContext.ParseResult, command);
        });

        _rootCommand.AddCommand(command);
    }

    public RootCommand GetRootCommand()
    {
        return _rootCommand;
    }
}

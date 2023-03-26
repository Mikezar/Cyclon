using System.CommandLine;
using System.CommandLine.Parsing;

namespace Cyclon.Console.Resolvers;

public interface IResolver<in TCommand, out TResolutionContext> 
    where TCommand : Command
    where TResolutionContext : IResolutionContext
{
    TResolutionContext Resolve(ParseResult parseResult, TCommand command);
}

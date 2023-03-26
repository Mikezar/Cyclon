using Cyclon.Console.Resolvers;

namespace Cyclon.Console.Handlers;

public interface IHandler<TContext> where TContext : IResolutionContext
{
    Task Handle(TContext context, CancellationToken cancellationToken);
}

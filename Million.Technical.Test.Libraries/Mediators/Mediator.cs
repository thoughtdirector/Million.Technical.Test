using Microsoft.Extensions.DependencyInjection;
using Million.Technical.Test.Libraries.Cqs.Request;
using System.Collections.Concurrent;

namespace Million.Technical.Test.Libraries.Mediators;

public class Mediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<TRequest, TResult>(TRequest request)
        where TRequest : IRequest
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        }

        return await handler.HandleAsync(request);
    }
}

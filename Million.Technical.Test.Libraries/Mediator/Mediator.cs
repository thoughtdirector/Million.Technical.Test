using Million.Technical.Test.Libraries.Cqs.Request;
using System.Collections.Concurrent;

namespace Million.Technical.Test.Libraries.Mediator;

public class Mediator
{
    private readonly ConcurrentDictionary<Type, object> _handlers = new();

    public void Register<TRequest, TResult, THandler>(THandler handler)
        where TRequest : IRequest
        where THandler : IRequestHandler<TRequest, TResult>
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler));

        var key = typeof(TRequest);
        if (!_handlers.TryAdd(key, handler))
            throw new InvalidOperationException($"A handler for {key.Name} is already registered.");
    }

    public async Task<TResult> SendAsync<TRequest, TResult>(TRequest request)
        where TRequest : IRequest
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (_handlers.TryGetValue(typeof(TRequest), out var handler))
        {
            if (handler is IRequestHandler<TRequest, TResult> typedHandler)
            {
                return await typedHandler.HandleAsync(request);
            }

            throw new InvalidOperationException(
                $"Registered handler for {typeof(TRequest).Name} does not match expected type.");
        }

        throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
    }
}
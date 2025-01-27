namespace Million.Technical.Test.Libraries.Cqs.Request
{
    public interface IRequestHandler<TRequest, TResult>
        where TRequest : IRequest
    {
        Task<TResult> HandleAsync(TRequest request);
    }
}
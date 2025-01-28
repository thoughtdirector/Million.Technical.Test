namespace Million.Technical.Test.Libraries.Cqs.Request.Commands
{
    public interface ICommand<TResult> : IRequest
    {
        Guid? Id { get; }
    }
}
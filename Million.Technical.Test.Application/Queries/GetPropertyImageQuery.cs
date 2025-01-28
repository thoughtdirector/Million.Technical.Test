using Million.Technical.Test.Libraries.Cqs.Request.Queries;

namespace Million.Technical.Test.Application.Queries
{
    public record GetPropertyImageQuery() : IQuery<byte[]>
    {
        public Guid PropertyId { get; init; }
        public Guid ImageId { get; init; }
    }
}
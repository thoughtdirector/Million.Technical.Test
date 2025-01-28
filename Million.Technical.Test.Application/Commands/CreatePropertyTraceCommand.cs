using Million.Technical.Test.Libraries.Cqs.Request.Commands;

namespace Million.Technical.Test.Application.Commands
{
    public record CreatePropertyTraceCommand : ICommand<Guid>
    {
        public Guid? Id { get; init; }
        public required Guid PropertyId { get; init; }
        public DateTime? DateSale { get; init; }
        public string? Name { get; init; }
        public decimal? Value { get; init; }
        public decimal? Tax { get; init; }
    }
}
using Million.Technical.Test.Libraries.Cqs.Request.Commands;

namespace Million.Technical.Test.Application.Commands
{
    public record CreatePropertyTraceCommand : ICommand<Guid>
    {
        public Guid? Id { get; init; }
        public required Guid PropertyId { get; init; }
        public required DateTime DateSale { get; init; }
        public required string Name { get; init; }
        public required decimal Value { get; init; }
        public required decimal Tax { get; init; }
    }
}
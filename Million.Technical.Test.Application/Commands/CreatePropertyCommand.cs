using Million.Technical.Test.Libraries.Cqs.Request.Commands;

namespace Million.Technical.Test.Application.Commands
{
    public record CreatePropertyCommand : ICommand<Guid>
    {
        public Guid? Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required decimal Price { get; init; }
        public required string CodeInternal { get; init; }
        public required int Year { get; init; }
        public required Guid IdOwner { get; init; }
    }
}
using Million.Technical.Test.Application.Commands.Models;
using Million.Technical.Test.Libraries.Cqs.Request.Commands;

namespace Million.Technical.Test.Application.Commands
{
    public record UpdatePropertyCommand : ICommand<Guid>
    {
        public Guid? Id { get; init; }
        public required Guid PropertyId { get; init; }
        public string? Name { get; init; }
        public string? Address { get; init; }
        public decimal? Price { get; init; }
        public string? CodeInternal { get; init; }
        public int? Year { get; init; }
        public Guid? IdOwner { get; init; }
        public PropertyTraceInfo? Trace { get; init; }
    }
}
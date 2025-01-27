using Million.Technical.Test.Libraries.Cqs.Request.Commands;

namespace Million.Technical.Test.Application.Commands
{
    public record ChangePropertyPriceCommand : ICommand<string>
    {
        public Guid? Id { get; init; }
        public Decimal? Price { get; init; }
    }
}
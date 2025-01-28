namespace Million.Technical.Test.Application.Commands.Models
{
    public class PropertyTraceInfo
    {
        public DateTime DateSale { get; init; }
        public string Name { get; init; } = null!;
        public decimal Value { get; init; }
        public decimal Tax { get; init; }
    }
}
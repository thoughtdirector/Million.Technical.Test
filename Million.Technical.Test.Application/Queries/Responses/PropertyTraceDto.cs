namespace Million.Technical.Test.Application.Queries.Responses
{
    public class PropertyTraceDto
    {
        public Guid? IdPropertyTrace { get; set; }
        public DateTime? DateSale { get; init; }
        public string? Name { get; init; }
        public decimal? Value { get; init; }
        public decimal? Tax { get; init; }
    }
}
namespace Million.Technical.Test.Application.Queries.Responses
{
    public class PropertyDetailDto
    {
        public Guid IdProperty { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal? Price { get; set; }
        public string CodeInternal { get; set; } = null!;
        public int Year { get; set; }
        public OwnerDto Owner { get; set; } = null!;
        public ICollection<PropertyImageDto> Images { get; set; } = new List<PropertyImageDto>();
        public ICollection<PropertyTraceDto> Traces { get; set; } = new List<PropertyTraceDto>();
    }
}
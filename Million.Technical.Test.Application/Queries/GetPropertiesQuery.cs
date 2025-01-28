using Million.Technical.Test.Application.Queries.Responses;
using Million.Technical.Test.Libraries.Cqs.Request.Queries;

namespace Million.Technical.Test.Application.Queries
{
    public record GetPropertiesQuery : IQuery<IEnumerable<PropertyDetailDto>>
    {
        public string? Name { get; init; }
        public string? Address { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public string? CodeInternal { get; init; }
        public int? MinYear { get; init; }
        public int? MaxYear { get; init; }
        public Guid? OwnerId { get; init; }
        public string? OwnerName { get; init; }
        public DateTime? MinDateSale { get; init; }
        public DateTime? MaxDateSale { get; init; }
        public bool? HasImages { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
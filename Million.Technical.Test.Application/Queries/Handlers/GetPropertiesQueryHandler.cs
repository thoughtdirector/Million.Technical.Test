using Microsoft.EntityFrameworkCore;
using Million.Technical.Test.Application._Resourses.Constants;
using Million.Technical.Test.Application.Queries.Responses;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Repositories;

namespace Million.Technical.Test.Application.Queries.Handlers
{
    public class GetPropertiesQueryHandler(IRepository<Property> _propertyRepository) : IRequestHandler<GetPropertiesQuery, IEnumerable<PropertyDetailDto>>
    {
        public async Task<IEnumerable<PropertyDetailDto>> HandleAsync(GetPropertiesQuery query)
        {
            var queryable = GetInitialQuery();

            queryable = ApplyBasicFilters(queryable, query);
            queryable = ApplyPriceFilters(queryable, query);
            queryable = ApplyOwnerFilters(queryable, query);
            queryable = ApplyTraceFilters(queryable, query);
            queryable = ApplyImageFilters(queryable, query);

            return await GetPaginatedResults(queryable, query);
        }

        private IQueryable<Property> GetInitialQuery()
        {
            return _propertyRepository.GetQueryable()
                .Include(p => p.Owner)
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyTraces)
                .AsQueryable();
        }

        private static IQueryable<Property> ApplyBasicFilters(IQueryable<Property> query, GetPropertiesQuery filters)
        {
            if (!string.IsNullOrWhiteSpace(filters.Name))
                query = query.Where(p => p.Name!.Contains(filters.Name));

            if (!string.IsNullOrWhiteSpace(filters.Address))
                query = query.Where(p => p.Address!.Contains(filters.Address));

            if (!string.IsNullOrWhiteSpace(filters.CodeInternal))
                query = query.Where(p => p.CodeInternal!.Contains(filters.CodeInternal));

            if (filters.MinYear.HasValue)
                query = query.Where(p => p.Year >= filters.MinYear.Value);

            if (filters.MaxYear.HasValue)
                query = query.Where(p => p.Year <= filters.MaxYear.Value);

            return query;
        }

        private static IQueryable<Property> ApplyPriceFilters(IQueryable<Property> query, GetPropertiesQuery filters)
        {
            if (filters.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filters.MinPrice.Value);

            if (filters.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filters.MaxPrice.Value);

            return query;
        }

        private static IQueryable<Property> ApplyOwnerFilters(IQueryable<Property> query, GetPropertiesQuery filters)
        {
            if (filters.OwnerId.HasValue)
                query = query.Where(p => p.IdOwner == filters.OwnerId.Value);

            if (!string.IsNullOrWhiteSpace(filters.OwnerName))
                query = query.Where(p => p.Owner!.Name!.Contains(filters.OwnerName));

            return query;
        }

        private static IQueryable<Property> ApplyTraceFilters(IQueryable<Property> query, GetPropertiesQuery filters)
        {
            if (!filters.MinDateSale.HasValue && !filters.MaxDateSale.HasValue)
                return query;

            if (filters.MinDateSale.HasValue)
                query = query.Where(p => p.PropertyTraces != null &&
                                        p.PropertyTraces.Any(t => t.DateSale >= filters.MinDateSale.Value));

            if (filters.MaxDateSale.HasValue)
                query = query.Where(p => p.PropertyTraces != null &&
                                        p.PropertyTraces.Any(t => t.DateSale <= filters.MaxDateSale.Value));

            return query;
        }

        private static IQueryable<Property> ApplyImageFilters(IQueryable<Property> query, GetPropertiesQuery filters)
        {
            if (!filters.HasImages.HasValue)
                return query;

            return filters.HasImages.Value
                ? query.Where(p => p.PropertyImages != null &&
                                  p.PropertyImages.Any(i => i.Enabled))
                : query.Where(p => p.PropertyImages == null ||
                                  !p.PropertyImages.Any(i => i.Enabled));
        }

        private static async Task<IEnumerable<PropertyDetailDto>> GetPaginatedResults(
            IQueryable<Property> query,
            GetPropertiesQuery filters)
        {
            var pageSize = Math.Min(filters.PageSize, PaginationConstants.MAX_PAGE_SIZE);
            var skip = (filters.PageNumber - 1) * pageSize;

            return await query
                .Skip(skip)
                .Take(pageSize)
                .Select(p => MapToPropertyDetailDto(p))
                .ToListAsync();
        }

        private static PropertyDetailDto MapToPropertyDetailDto(Property p)
        {
            return new PropertyDetailDto
            {
                IdProperty = p.IdProperty,
                Name = p.Name!,
                Address = p.Address!,
                Price = p.Price,
                CodeInternal = p.CodeInternal!,
                Year = p.Year,
                Owner = new OwnerDto
                {
                    IdOwner = p.Owner!.IdOwner,
                    Name = p.Owner.Name
                },
                Images = p.PropertyImages?.Select(i => new PropertyImageDto
                {
                    IdPropertyImage = i.IdPropertyImage,
                    ImageUrl = $"/api/property/{p.IdProperty}/image/{i.IdPropertyImage}"
                })?.ToList() ?? new List<PropertyImageDto>(),
                Traces = p.PropertyTraces?.Select(t => new PropertyTraceDto
                {
                    IdPropertyTrace = t.IdPropertyTrace,
                    DateSale = t.DateSale,
                    Name = t.Name,
                    Value = t.Value,
                    Tax = t.Tax
                })?.ToList() ?? new List<PropertyTraceDto>()
            };
        }
    }
}
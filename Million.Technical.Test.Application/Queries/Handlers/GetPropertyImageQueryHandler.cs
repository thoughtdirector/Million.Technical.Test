using Microsoft.EntityFrameworkCore;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repositories;

namespace Million.Technical.Test.Application.Queries.Handlers
{
    public class GetPropertyImageQueryHandler
        (IRepository<PropertyImage> _propertyRepository) : IRequestHandler<GetPropertyImageQuery, byte[]>
    {
        public async Task<byte[]> HandleAsync(GetPropertyImageQuery query)
        {
            var image = await _propertyRepository.GetQueryable()
                .Where(i => i.IdProperty == query.PropertyId
                       && i.IdPropertyImage == query.ImageId
                       && i.Enabled)
                .Select(i => i.ImageData).FirstOrDefaultAsync();

            if (image == null)
                throw new NotFoundException("Image not found");

            return image;
        }
    }
}
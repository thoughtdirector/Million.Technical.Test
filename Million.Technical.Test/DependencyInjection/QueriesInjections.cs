using Million.Technical.Test.Application.Queries;
using Million.Technical.Test.Application.Queries.Handlers;
using Million.Technical.Test.Application.Queries.Responses;
using Million.Technical.Test.Libraries.Cqs.Request;

namespace Million.Technical.Test.Api.DependencyInjection
{
    public static class QueriesInjections
    {
        public static void InjectQueries(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRequestHandler<GetPropertiesQuery, IEnumerable<PropertyDetailDto>>, GetPropertiesQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<GetPropertyImageQuery, byte[]>, GetPropertyImageQueryHandler>();
        }
    }
}
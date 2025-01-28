using Microsoft.EntityFrameworkCore;
using Million.Technical.Test.Application.Shared.Methods;
using Million.Technical.Test.Infrastructure.Data;
using Million.Technical.Test.Infrastructure.Shared.Methods;
using Million.Technical.Test.Libraries.Repositories;

namespace Million.Technical.Test.Api.DependencyInjection
{
    public static class BaseInjections
    {
        public static void InjectBase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DbContext, RealEstateDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Million.Technical.Test.Infrastructure")), ServiceLifetime.Transient);

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IImageService>(provider => new ImageService());
        }
    }
}
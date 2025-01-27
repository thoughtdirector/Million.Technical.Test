using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Handlers;
using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Infrastructure.Validators.Commands;
using Million.Technical.Test.Libraries.Cqs.Request;

namespace Million.Technical.Test.Api.DependencyInjection
{
    public static class CommandsInjections
    {
        public static void InjectCommands(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRequestHandler<CreatePropertyCommand, Guid>, CreatePropertyCommandHandler>();
            builder.Services.AddScoped<ICreatePropertyValidator, CreatePropertyValidator>();
            builder.Services.AddScoped<IRequestHandler<CreateOwnerCommand, Guid>, CreateOwnerCommandHandler>();
            builder.Services.AddScoped<ICreateOwnerValidator, CreateOwnerValidator>();
            builder.Services.AddScoped<IRequestHandler<CreatePropertyTraceCommand, Guid>, CreatePropertyTraceCommandHandler>();
            builder.Services.AddScoped<ICreatePropertyTraceValidator, CreatePropertyTraceValidator>();
            builder.Services.AddScoped<IRequestHandler<AddPropertyImageCommand, Guid>, AddPropertyImageCommandHandler>();
            builder.Services.AddScoped<IAddPropertyImageValidator, AddPropertyImageValidator>();
            builder.Services.AddScoped<IRequestHandler<UpdatePropertyCommand, Guid>, UpdatePropertyCommandHandler>();
            builder.Services.AddScoped<IUpdatePropertyValidator, UpdatePropertyValidator>();
            builder.Services.AddScoped<IRequestHandler<ChangePropertyPriceCommand, string>, ChangePropertyPriceCommandHandler>();
            builder.Services.AddScoped<IChangePropertyPriceValidator, ChangePropertyPriceValidator>();
        }
    }
}
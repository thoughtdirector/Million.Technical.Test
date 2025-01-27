using Million.Technical.Test.Api.DependencyInjection;
using Million.Technical.Test.Application.Commands.Handlers;
using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Infrastructure.Validators.Commands;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Mediators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Inject();

builder.Services.AddScoped<Mediator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

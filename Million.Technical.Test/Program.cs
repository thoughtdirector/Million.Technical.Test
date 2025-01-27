using Million.Technical.Test.Api.DependencyInjection;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Mediators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Mediator>();
builder.Services.AddSingleton(provider =>
{
    var mediator = new Mediator();

    mediator.Register<CreatePropertyCommand, Guid, IRequestHandler<CreatePropertyCommand, Guid>>(
        provider.GetRequiredService<IRequestHandler<CreatePropertyCommand, Guid>>());

    mediator.Register<CreateOwnerCommand, Guid, IRequestHandler<CreateOwnerCommand, Guid>>(
        provider.GetRequiredService<IRequestHandler<CreateOwnerCommand, Guid>>());
    return mediator;
});

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

builder.Inject();

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
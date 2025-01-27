using Microsoft.AspNetCore.Http;
using Million.Technical.Test.Libraries.Cqs.Request.Commands;

namespace Million.Technical.Test.Application.Commands
{
    public record AddPropertyImageCommand : ICommand<Guid>
    {
        public Guid? Id { get; init; }
        public required Guid PropertyId { get; init; }
        public required IFormFile Image { get; init; }
        public bool Enabled { get; init; } = true;
    }
}
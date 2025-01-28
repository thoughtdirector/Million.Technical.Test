using Microsoft.AspNetCore.Http;
using Million.Technical.Test.Libraries.Cqs.Request.Commands;

namespace Million.Technical.Test.Application.Commands
{
    public record CreateOwnerCommand : ICommand<Guid>
    {
        public Guid? Id { get; init; }
        public string? Name { get; init; }
        public string? Address { get; init; }
        public DateTime? Birthday { get; init; }
        public IFormFile? Photo { get; init; }
    }
}
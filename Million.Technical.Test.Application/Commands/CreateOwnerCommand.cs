using Microsoft.AspNetCore.Http;
using Million.Technical.Test.Libraries.Cqs.Request.Commands;

namespace Million.Technical.Test.Application.Commands
{
    public record CreateOwnerCommand : ICommand<Guid>
    {
        public Guid? Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required DateTime Birthday { get; init; }
        public IFormFile? Photo { get; init; }
    }

}

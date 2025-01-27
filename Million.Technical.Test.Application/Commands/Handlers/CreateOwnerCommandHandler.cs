using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Application.Shared.Methods;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Repositories;

namespace Million.Technical.Test.Application.Commands.Handlers
{
    public class CreateOwnerCommandHandler
           (IRepository<Owner> _ownerRepository,
           ICreateOwnerValidator _validator,
           IImageService _imageService) : IRequestHandler<CreateOwnerCommand, Guid>
    {
        public async Task<Guid> HandleAsync(CreateOwnerCommand command)
        {
            _validator.ValidateAndThrow(command);

            byte[]? photoData = null;

            if (command.Photo != null && command.Photo.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await command.Photo.CopyToAsync(memoryStream);
                photoData = await _imageService.CompressImageAsync(memoryStream.ToArray(), command.Photo.FileName);
            }

            Owner owner = new()
            {
                IdOwner = Guid.NewGuid(),
                Name = command.Name,
                Address = command.Address,
                Birthday = command.Birthday,
                Photo = photoData
            };

            Owner result = await _ownerRepository.CreateAsync(owner);
            return result.IdOwner;
        }
    }
}
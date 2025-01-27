using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Application.Shared.Methods;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repository;

namespace Million.Technical.Test.Application.Commands.Handlers
{
    public class AddPropertyImageCommandHandler
            (IRepository<Property> _propertyRepository,
            IRepository<PropertyImage> _imageRepository,
            IImageService _imageService,
            IAddPropertyImageValidator _validator) : IRequestHandler<AddPropertyImageCommand, Guid>
    {
        public async Task<Guid> HandleAsync(AddPropertyImageCommand command)
        {
            _validator.ValidateAndThrow(command);

            Property property = await _propertyRepository.GetByIdAsync(command.PropertyId)
                ?? throw new NotFoundException($"Property with ID {command.PropertyId} not found");

            byte[]? imageData = null;

            if (command.Image != null && command.Image.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await command.Image.CopyToAsync(memoryStream);
                imageData = await _imageService.CompressImageAsync(memoryStream.ToArray(), command.Image.FileName!);
            }

            PropertyImage propertyImage = new() 
            {
                IdPropertyImage = Guid.NewGuid(),
                IdProperty = command.PropertyId,
                ImageData = imageData,
                Enabled = command.Enabled
            };

            PropertyImage result = await _imageRepository.CreateAsync(propertyImage);
            return result.IdPropertyImage;
        }
    }
}

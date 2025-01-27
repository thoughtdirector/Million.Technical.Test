using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repository;

namespace Million.Technical.Test.Application.Commands.Handlers
{
    public class CreatePropertyCommandHandler
        (IRepository<Property> _propertyRepository,
        IRepository<Owner> _ownerRepository,
        ICreatePropertyValidator _validator) : IRequestHandler<CreatePropertyCommand, Guid>
    {
        public async Task<Guid> HandleAsync(CreatePropertyCommand command)
        {
            _validator.ValidateAndThrow(command);

            Owner owner = await _ownerRepository.GetByIdAsync(command.IdOwner)
                ?? throw new NotFoundException($"Owner with ID {command.IdOwner} not found");

            Property property = new()
            {
                IdProperty = Guid.NewGuid(),
                Name = command.Name,
                Address = command.Address,
                Price = command.Price,
                CodeInternal = command.CodeInternal,
                Year = command.Year,
                IdOwner = command.IdOwner
            };

            Property result = await _propertyRepository.CreateAsync(property);
            return result.IdProperty;
        }
    }
}
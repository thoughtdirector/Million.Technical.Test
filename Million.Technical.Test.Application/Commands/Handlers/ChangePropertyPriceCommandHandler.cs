using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repository;

namespace Million.Technical.Test.Application.Commands.Handlers
{
    public class ChangePropertyPriceCommandHandler
        (IRepository<Property> _propertyRepository,
        IChangePropertyPriceValidator _validator) : IRequestHandler<ChangePropertyPriceCommand, string>
    {
        public async Task<string> HandleAsync(ChangePropertyPriceCommand command)
        {
            _validator.ValidateAndThrow(command);

            Property property = await _propertyRepository.GetByIdAsync(command.Id!.Value)
               ?? throw new NotFoundException($"Property with ID {command.Id} not found");

            property.Price = command.Price;
            await _propertyRepository.UpdateAsync(property);

            return $"Property price changed successfully.";
        }
    }
}
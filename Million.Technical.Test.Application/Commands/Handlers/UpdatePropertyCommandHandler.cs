using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repositories;

namespace Million.Technical.Test.Application.Commands.Handlers
{
    public class UpdatePropertyCommandHandler
            (IRepository<Property> _propertyRepository,
            IRepository<Owner> _ownerRepository,
            IRepository<PropertyTrace> _traceRepository,
            IUpdatePropertyValidator _validator) : IRequestHandler<UpdatePropertyCommand, Guid>
    {
        public async Task<Guid> HandleAsync(UpdatePropertyCommand command)
        {
            _validator.ValidateAndThrow(command);

            var property = await _propertyRepository.GetByIdAsync(command.PropertyId)
                ?? throw new NotFoundException($"Property with ID {command.PropertyId} not found");

            if (command.Name != null) property.Name = command.Name;
            if (command.Address != null) property.Address = command.Address;
            if (command.Price.HasValue) property.Price = command.Price.Value;
            if (command.CodeInternal != null) property.CodeInternal = command.CodeInternal;
            if (command.Year.HasValue) property.Year = command.Year.Value;

            if (command.IdOwner.HasValue)
            {
                var owner = await _ownerRepository.GetByIdAsync(command.IdOwner.Value)
                    ?? throw new NotFoundException($"Owner with ID {command.IdOwner} not found");
                property.IdOwner = command.IdOwner.Value;
            }

            if (command.Trace != null)
            {
                var propertyTrace = new PropertyTrace
                {
                    IdProperty = command.PropertyId,
                    DateSale = command.Trace.DateSale,
                    Name = command.Trace.Name,
                    Value = command.Trace.Value,
                    Tax = command.Trace.Tax
                };

                await _traceRepository.CreateAsync(propertyTrace);
            }

            await _propertyRepository.UpdateAsync(property);
            return property.IdProperty;
        }
    }
}
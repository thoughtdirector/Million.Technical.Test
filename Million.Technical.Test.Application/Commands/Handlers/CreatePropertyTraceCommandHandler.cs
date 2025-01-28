using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Libraries.Cqs.Request;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repositories;

namespace Million.Technical.Test.Application.Commands.Handlers
{
    public class CreatePropertyTraceCommandHandler
      (IRepository<Property> _propertyRepository,
      IRepository<PropertyTrace> _traceRepository,
      ICreatePropertyTraceValidator _validator) : IRequestHandler<CreatePropertyTraceCommand, Guid>
    {
        public async Task<Guid> HandleAsync(CreatePropertyTraceCommand command)
        {
            _validator.ValidateAndThrow(command);

            Property property = await _propertyRepository.GetByIdAsync(command.PropertyId)
                ?? throw new NotFoundException($"Property with ID {command.PropertyId} not found");

            PropertyTrace propertyTrace = new()
            {
                IdPropertyTrace = Guid.NewGuid(),
                IdProperty = command.PropertyId,
                DateSale = command.DateSale!.Value,
                Name = command.Name,
                Value = command.Value!.Value,
                Tax = command.Tax!.Value
            };

            PropertyTrace result = await _traceRepository.CreateAsync(propertyTrace);
            return result.IdPropertyTrace;
        }
    }
}
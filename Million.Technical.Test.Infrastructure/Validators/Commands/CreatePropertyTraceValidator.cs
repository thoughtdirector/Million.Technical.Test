using FluentValidation;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Infrastructure.Validators.Constants;

namespace Million.Technical.Test.Infrastructure.Validators.Commands
{
    public class CreatePropertyTraceValidator : AbstractValidator<CreatePropertyTraceCommand>, ICreatePropertyTraceValidator
    {
        public CreatePropertyTraceValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotNull()
                .NotEmpty().WithMessage("Property Id is required")
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Property Id must be a valid GUID format");

            RuleFor(x => x.DateSale)
                .NotEmpty().WithMessage("Sale date is required")
                .LessThan(DateTime.UtcNow)
                .WithMessage("Sale date cannot be in the future");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(ValidationConstants.MAX_PROPERTY_NAME_LENGTH)
                .WithMessage($"Name cannot exceed {ValidationConstants.MAX_PROPERTY_NAME_LENGTH} characters")
                .MinimumLength(ValidationConstants.MIN_PROPERTY_NAME_LENGTH)
                .WithMessage($"Name must be at least {ValidationConstants.MIN_PROPERTY_NAME_LENGTH} characters");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required")
                .GreaterThan(ValidationConstants.MIN_PRICE)
                .WithMessage($"Value must be greater than {ValidationConstants.MIN_PRICE}");

            RuleFor(x => x.Tax)
                .NotEmpty().WithMessage("Tax is required")
                .GreaterThanOrEqualTo(ValidationConstants.MIN_PRICE)
                .WithMessage("Tax must be greater than or equal to 0");
        }

        public void ValidateAndThrow(CreatePropertyTraceCommand command)
        {
            this.ValidateAndThrow<CreatePropertyTraceCommand>(command);
        }
    }
}
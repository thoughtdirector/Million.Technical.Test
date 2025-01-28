using FluentValidation;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Infrastructure.Validators.Constants;

namespace Million.Technical.Test.Infrastructure.Validators.Commands
{
    public class CreatePropertyValidator : AbstractValidator<CreatePropertyCommand>, ICreatePropertyValidator
    {
        public CreatePropertyValidator()
        {
            RuleFor(x => x.Name).NotNull()
                .NotEmpty().WithMessage("Property name is required")
                .MaximumLength(ValidationConstants.MAX_PROPERTY_NAME_LENGTH).WithMessage("Property name cannot exceed 100 characters")
                .MinimumLength(ValidationConstants.MIN_PROPERTY_NAME_LENGTH).WithMessage("Property name must be at least 3 characters");

            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty().WithMessage("Property address is required")
                .MaximumLength(ValidationConstants.MAX_ADDRESS_LENGTH).WithMessage("Property address cannot exceed 250 characters")
                .MinimumLength(ValidationConstants.MIN_ADDRESS_LENGTH).WithMessage("Property address must be at least 5 characters");

            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(ValidationConstants.MIN_PRICE).WithMessage("Price must be greater than 0");

            RuleFor(x => x.CodeInternal)
                .NotNull()
                .NotEmpty().WithMessage("Internal code is required")
                .MaximumLength(ValidationConstants.MAX_INTERNAL_CODE_LENGTH).WithMessage("Internal code cannot exceed 50 characters")
                .Matches(ValidationConstants.INTERNAL_CODE_REGEX_PATTERN).WithMessage("Internal code can only contain letters, numbers, hyphens and underscores");

            RuleFor(x => x.Year)
                .NotNull()
                .GreaterThan(ValidationConstants.MIN_YEAR).WithMessage("Year must be after 1800")
                .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage($"Year cannot be in the future");

            RuleFor(x => x.IdOwner)
                .NotNull()
                .NotEmpty().WithMessage("Owner Id is required")
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Owner Id must be a valid GUID format");
        }

        public void ValidateAndThrow(CreatePropertyCommand command)
        {
            this.ValidateAndThrow<CreatePropertyCommand>(command);
        }
    }
}
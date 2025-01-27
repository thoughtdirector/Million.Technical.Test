using FluentValidation;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Infrastructure.Validators.Constants;

namespace Million.Technical.Test.Infrastructure.Validators.Commands
{
    public class CreateOwnerValidator : AbstractValidator<CreateOwnerCommand>, ICreateOwnerValidator
    {
        public CreateOwnerValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("Owner name is required")
                .MaximumLength(ValidationConstants.MAX_PROPERTY_NAME_LENGTH).WithMessage("Owner name cannot exceed 100 characters")
                .MinimumLength(ValidationConstants.MIN_PROPERTY_NAME_LENGTH).WithMessage("Owner name must be at least 3 characters");

            RuleFor(x => x.PhotoName)
               .MaximumLength(ValidationConstants.MAX_PROPERTY_NAME_LENGTH).WithMessage("Photo name cannot exceed 100 characters")
               .MinimumLength(ValidationConstants.MIN_PROPERTY_NAME_LENGTH).WithMessage("Photo name must be at least 3 characters")
               .Matches(ValidationConstants.PHOTO_NAME_REGEX_PATTERN).WithMessage("Photo name must have a valid extension");

            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty().WithMessage("Owner address is required")
                .MaximumLength(ValidationConstants.MAX_ADDRESS_LENGTH).WithMessage("Owner address cannot exceed 250 characters")
                .MinimumLength(ValidationConstants.MIN_ADDRESS_LENGTH).WithMessage("Owner address must be at least 5 characters");

            RuleFor(x => x.Birthday)
                .NotNull()
                .NotEmpty().WithMessage("Birthday is required")
                .LessThan(DateTime.UtcNow).WithMessage("Birthday cannot be in the future")
                .GreaterThan(DateTime.UtcNow.AddYears(-120)).WithMessage("Birthday cannot be more than 120 years ago");

            When(x => x.Photo != null, () =>
            {
                RuleFor(x => x.Photo!.Length)
                    .LessThanOrEqualTo(ValidationConstants.MAX_IMAGE_SIZE_BYTES)
                    .WithMessage($"Photo size must not exceed {ValidationConstants.MAX_IMAGE_SIZE_BYTES / (1024 * 1024)}MB");
            });
        }

        public void ValidateAndThrow(CreateOwnerCommand command)
        {
            this.ValidateAndThrow<CreateOwnerCommand>(command);
        }
    }
}
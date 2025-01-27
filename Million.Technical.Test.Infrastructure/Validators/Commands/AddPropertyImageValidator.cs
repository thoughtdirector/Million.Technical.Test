using FluentValidation;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Infrastructure.Validators.Constants;

namespace Million.Technical.Test.Infrastructure.Validators.Commands
{
    public class AddPropertyImageValidator : AbstractValidator<AddPropertyImageCommand>, IAddPropertyImageValidator
    {
        public AddPropertyImageValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotNull()
                .NotEmpty().WithMessage("Property Id is required")
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Property Id must be a valid GUID format");

            RuleFor(x => x.Image)
                .NotNull()
                .NotEmpty().WithMessage("Image is required");

            RuleFor(x => x.Image!.Length)
                               .LessThanOrEqualTo(ValidationConstants.MAX_IMAGE_SIZE_BYTES)
                               .WithMessage($"Photo size must not exceed {ValidationConstants.MAX_IMAGE_SIZE_BYTES / (1024 * 1024)}MB");
        }

        public void ValidateAndThrow(AddPropertyImageCommand command)
        {
            this.ValidateAndThrow<AddPropertyImageCommand>(command);
        }
    }
}
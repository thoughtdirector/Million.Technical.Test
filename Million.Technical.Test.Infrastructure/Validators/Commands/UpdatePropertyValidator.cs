using FluentValidation;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Infrastructure.Validators.Constants;

namespace Million.Technical.Test.Infrastructure.Validators.Commands
{
    public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyCommand>, IUpdatePropertyValidator
    {
        public UpdatePropertyValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotNull()
                .NotEmpty().WithMessage("Property Id is required")
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Property Id must be a valid GUID format");

            When(x => x.Name != null, () =>
            {
                RuleFor(x => x.Name)
                    .MinimumLength(ValidationConstants.MIN_PROPERTY_NAME_LENGTH)
                    .MaximumLength(ValidationConstants.MAX_PROPERTY_NAME_LENGTH)
                    .WithMessage($"Name must be between {ValidationConstants.MIN_PROPERTY_NAME_LENGTH} and {ValidationConstants.MAX_PROPERTY_NAME_LENGTH} characters");
            });

            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address)
                    .MinimumLength(ValidationConstants.MIN_ADDRESS_LENGTH)
                    .MaximumLength(ValidationConstants.MAX_ADDRESS_LENGTH)
                    .WithMessage($"Address must be between {ValidationConstants.MIN_ADDRESS_LENGTH} and {ValidationConstants.MAX_ADDRESS_LENGTH} characters");
            });

            When(x => x.Price.HasValue, () =>
            {
                RuleFor(x => x.Price!.Value)
                    .GreaterThan(ValidationConstants.MIN_PRICE)
                    .WithMessage($"Price must be greater than {ValidationConstants.MIN_PRICE}");
            });

            When(x => x.CodeInternal != null, () =>
            {
                RuleFor(x => x.CodeInternal)
                    .MaximumLength(ValidationConstants.MAX_INTERNAL_CODE_LENGTH)
                    .Matches(ValidationConstants.INTERNAL_CODE_REGEX_PATTERN)
                    .WithMessage("Internal code can only contain letters, numbers, hyphens and underscores");
            });

            When(x => x.Year.HasValue, () =>
            {
                RuleFor(x => x.Year!.Value)
                    .GreaterThan(ValidationConstants.MIN_YEAR)
                    .LessThanOrEqualTo(DateTime.UtcNow.Year)
                    .WithMessage($"Year must be between {ValidationConstants.MIN_YEAR} and current year");
            });

            When(x => x.IdOwner.HasValue, () =>
            {
                RuleFor(x => x.IdOwner)
                    .Must(id => Guid.TryParse(id.ToString(), out _))
                    .WithMessage("Owner Id must be a valid GUID format");
            });

            When(x => x.NewImage != null, () =>
            {
                RuleFor(x => x.NewImage!.Length)
                    .LessThanOrEqualTo(ValidationConstants.MAX_IMAGE_SIZE_BYTES)
                    .WithMessage($"Image size must not exceed {ValidationConstants.MAX_IMAGE_SIZE_BYTES / (1024 * 1024)}MB");
            });

            When(x => x.Trace != null, () =>
            {
                RuleFor(x => x.Trace!.DateSale)
                    .LessThanOrEqualTo(DateTime.UtcNow)
                    .WithMessage("Sale date cannot be in the future");

                RuleFor(x => x.Trace!.Name)
                    .NotEmpty()
                    .MaximumLength(ValidationConstants.MAX_PROPERTY_NAME_LENGTH);

                RuleFor(x => x.Trace!.Value)
                    .GreaterThan(ValidationConstants.MIN_PRICE);

                RuleFor(x => x.Trace!.Tax)
                    .GreaterThanOrEqualTo(ValidationConstants.MIN_PRICE);
            });
        }

        public void ValidateAndThrow(UpdatePropertyCommand command)
        {
            this.ValidateAndThrow<UpdatePropertyCommand>(command);
        }
    }
}
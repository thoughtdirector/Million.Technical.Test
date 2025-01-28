using FluentValidation;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Validations;
using Million.Technical.Test.Infrastructure.Validators.Constants;

namespace Million.Technical.Test.Infrastructure.Validators.Commands
{
    public class ChangePropertyPriceValidator : AbstractValidator<ChangePropertyPriceCommand>, IChangePropertyPriceValidator
    {
        public ChangePropertyPriceValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty().WithMessage("Property Id is required")
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Property Id must be a valid GUID format");

            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(ValidationConstants.MIN_PRICE).WithMessage("Price must be greater than 0");
        }

        public void ValidateAndThrow(ChangePropertyPriceCommand command)
        {
            this.ValidateAndThrow<ChangePropertyPriceCommand>(command);
        }
    }
}
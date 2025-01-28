using FluentValidation.TestHelper;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Infrastructure.Validators.Commands;

namespace Million.Technical.Test.UnitTests.Commands
{
    [TestFixture]
    public class CreatePropertyTraceCommandTests
    {
        private CreatePropertyTraceValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreatePropertyTraceValidator();
        }

        [Test]
        public void Validator_WhenCommandIsValid_ShouldNotHaveValidationErrors()
        {
            var command = new CreatePropertyTraceCommand
            {
                PropertyId = Guid.NewGuid(),
                DateSale = DateTime.UtcNow.AddDays(-1),
                Name = "Sale Record",
                Value = 150000,
                Tax = 5000
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validator_WhenPropertyIdIsEmpty_ShouldHaveValidationError()
        {
            var command = new CreatePropertyTraceCommand
            {
                PropertyId = Guid.Empty,
                DateSale = DateTime.UtcNow,
                Name = "Sale Record",
                Value = 150000,
                Tax = 5000
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.PropertyId)
                .WithErrorMessage("Property Id is required");
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("A")]
        public void Validator_WhenNameIsInvalid_ShouldHaveValidationError(string? name)
        {
            var command = new CreatePropertyTraceCommand
            {
                PropertyId = Guid.NewGuid(),
                DateSale = DateTime.UtcNow,
                Name = name,
                Value = 150000,
                Tax = 5000
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Validator_WhenNameIsTooLong_ShouldHaveValidationError()
        {
            var command = new CreatePropertyTraceCommand
            {
                PropertyId = Guid.NewGuid(),
                DateSale = DateTime.UtcNow,
                Name = new string('A', 101),
                Value = 150000,
                Tax = 5000
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name cannot exceed 100 characters");
        }

        [Test]
        public void Validator_WhenDateSaleIsInFuture_ShouldHaveValidationError()
        {
            var command = new CreatePropertyTraceCommand
            {
                PropertyId = Guid.NewGuid(),
                DateSale = DateTime.UtcNow.AddDays(1),
                Name = "Sale Record",
                Value = 150000,
                Tax = 5000
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.DateSale)
                .WithErrorMessage("Sale date cannot be in the future");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validator_WhenValueIsInvalid_ShouldHaveValidationError(decimal value)
        {
            var command = new CreatePropertyTraceCommand
            {
                PropertyId = Guid.NewGuid(),
                DateSale = DateTime.UtcNow,
                Name = "Sale Record",
                Value = value,
                Tax = 5000
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Value)
                .WithErrorMessage("Value must be greater than 0,01");
        }

        [Test]
        public void Validator_WhenTaxIsNegative_ShouldHaveValidationError()
        {
            var command = new CreatePropertyTraceCommand
            {
                PropertyId = Guid.NewGuid(),
                DateSale = DateTime.UtcNow,
                Name = "Sale Record",
                Value = 150000,
                Tax = -1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Tax)
                .WithErrorMessage("Tax must be greater than or equal to 0");
        }
    }
}
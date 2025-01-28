using FluentValidation.TestHelper;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Handlers;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Infrastructure.Validators.Commands;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repositories;
using Moq;

namespace Million.Technical.Test.UnitTests.Commands
{
    [TestFixture]
    public class ChangePropertyPriceCommandTests
    {
        private ChangePropertyPriceValidator _validator;
        private Mock<IRepository<Property>> _propertyRepositoryMock;
        private ChangePropertyPriceCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _validator = new ChangePropertyPriceValidator();
            _propertyRepositoryMock = new Mock<IRepository<Property>>();
            _handler = new ChangePropertyPriceCommandHandler(
                _propertyRepositoryMock.Object,
                _validator);
        }

        [Test]
        public void Validator_WhenCommandIsValid_ShouldNotHaveValidationErrors()
        {
            var command = new ChangePropertyPriceCommand
            {
                Id = Guid.NewGuid(),
                Price = 150000
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validator_WhenIdIsNull_ShouldHaveValidationError()
        {
            var command = new ChangePropertyPriceCommand
            {
                Id = null,
                Price = 150000
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Property Id is required");
        }

        [Test]
        public void Validator_WhenPriceIsNull_ShouldHaveValidationError()
        {
            var command = new ChangePropertyPriceCommand
            {
                Id = Guid.NewGuid(),
                Price = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-100)]
        public void Validator_WhenPriceIsInvalid_ShouldHaveValidationError(decimal price)
        {
            var command = new ChangePropertyPriceCommand
            {
                Id = Guid.NewGuid(),
                Price = price
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price)
                .WithErrorMessage("Price must be greater than 0");
        }

        [Test]
        public void Handler_WhenPropertyNotFound_ShouldThrowNotFoundException()
        {
            var command = new ChangePropertyPriceCommand
            {
                Id = Guid.NewGuid(),
                Price = 150000
            };

            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(command.Id.Value))
                .ReturnsAsync((Property?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(
                () => _handler.HandleAsync(command));
            Assert.That(ex.Message, Is.EqualTo($"Property with ID {command.Id} not found"));
        }

        [Test]
        public async Task Handler_WhenCommandIsValid_ShouldUpdatePropertyPrice()
        {
            var propertyId = Guid.NewGuid();
            var originalPrice = 100000m;
            var newPrice = 150000m;

            var command = new ChangePropertyPriceCommand
            {
                Id = propertyId,
                Price = newPrice
            };

            var property = new Property
            {
                IdProperty = propertyId,
                Price = originalPrice
            };

            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync(property);

            var result = await _handler.HandleAsync(command);

            Assert.That(result, Is.EqualTo("Property price changed successfully."));
            Assert.That(property.Price, Is.EqualTo(newPrice));
            _propertyRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Property>(
                p => p.IdProperty == propertyId &&
                     p.Price == newPrice)), Times.Once);
        }

        [Test]
        public void Handler_WhenValidationFails_ShouldThrowValidationException()
        {
            var command = new ChangePropertyPriceCommand
            {
                Id = Guid.NewGuid(),
                Price = -1
            };

            Assert.ThrowsAsync<FluentValidation.ValidationException>(
                () => _handler.HandleAsync(command));
        }
    }
}
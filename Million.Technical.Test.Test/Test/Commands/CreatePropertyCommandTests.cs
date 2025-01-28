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
    public class CreatePropertyCommandTests
    {
        private CreatePropertyValidator _validator;
        private Mock<IRepository<Property>> _propertyRepositoryMock;
        private Mock<IRepository<Owner>> _ownerRepositoryMock;
        private CreatePropertyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _validator = new CreatePropertyValidator();
            _propertyRepositoryMock = new Mock<IRepository<Property>>();
            _ownerRepositoryMock = new Mock<IRepository<Owner>>();
            _handler = new CreatePropertyCommandHandler(
                _propertyRepositoryMock.Object,
                _ownerRepositoryMock.Object,
                _validator);
        }

        [Test]
        public void Validator_WhenCommandIsValid_ShouldNotHaveValidationErrors()
        {
            var command = new CreatePropertyCommand
            {
                Name = "Test Property",
                Address = "123 Test St",
                Price = 100000,
                CodeInternal = "TEST-001",
                Year = 2020,
                IdOwner = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase("", "Property address is required")]
        [TestCase("12", "Property address must be at least 5 characters")]
        public void Validator_WhenAddressIsInvalid_ShouldHaveValidationError(string address, string expectedError)
        {
            var command = new CreatePropertyCommand
            {
                Name = "Test Property",
                Address = address,
                Price = 100000,
                CodeInternal = "TEST-001",
                Year = 2020,
                IdOwner = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address)
                .WithErrorMessage(expectedError);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validator_WhenPriceIsInvalid_ShouldHaveValidationError(decimal price)
        {
            var command = new CreatePropertyCommand
            {
                Name = "Test Property",
                Address = "123 Test St",
                Price = price,
                CodeInternal = "TEST-001",
                Year = 2020,
                IdOwner = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Test]
        public void Handler_WhenOwnerDoesNotExist_ShouldThrowNotFoundException()
        {
            var command = new CreatePropertyCommand
            {
                Name = "Test Property",
                Address = "123 Test St",
                Price = 100000,
                CodeInternal = "TEST-001",
                Year = 2020,
                IdOwner = Guid.NewGuid()
            };

            _ownerRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Owner?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command));
            Assert.That(ex.Message, Is.EqualTo($"Owner with ID {command.IdOwner} not found"));
        }

        [Test]
        public async Task Handler_WhenCommandIsValid_ShouldCreateProperty()
        {
            var ownerId = Guid.NewGuid();
            var command = new CreatePropertyCommand
            {
                Name = "Test Property",
                Address = "123 Test St",
                Price = 100000,
                CodeInternal = "TEST-001",
                Year = 2020,
                IdOwner = ownerId
            };

            var owner = new Owner { IdOwner = ownerId, Name = "Test Owner" };
            _ownerRepositoryMock.Setup(x => x.GetByIdAsync(ownerId))
                .ReturnsAsync(owner);

            var createdProperty = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = command.Name,
                Address = command.Address,
                Price = command.Price,
                CodeInternal = command.CodeInternal,
                Year = command.Year,
                IdOwner = command.IdOwner
            };

            _propertyRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Property>()))
                .ReturnsAsync(createdProperty);

            var result = await _handler.HandleAsync(command);

            Assert.That(result, Is.EqualTo(createdProperty.IdProperty));
            _propertyRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Property>()), Times.Once);
        }

        [Test]
        public void Handler_WhenValidationFails_ShouldThrowValidationException()
        {
            var command = new CreatePropertyCommand
            {
                Name = "",
                Address = "123 Test St",
                Price = 100000,
                CodeInternal = "TEST-001",
                Year = 2020,
                IdOwner = Guid.NewGuid()
            };

            Assert.ThrowsAsync<FluentValidation.ValidationException>(
                () => _handler.HandleAsync(command));
        }
    }
}
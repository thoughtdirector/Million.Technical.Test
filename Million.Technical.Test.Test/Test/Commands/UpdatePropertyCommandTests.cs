using FluentValidation.TestHelper;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Handlers;
using Million.Technical.Test.Application.Commands.Models;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Infrastructure.Validators.Commands;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repositories;
using Moq;

namespace Million.Technical.Test.UnitTests.Commands
{
    [TestFixture]
    public class UpdatePropertyCommandTests
    {
        private UpdatePropertyValidator _validator;
        private Mock<IRepository<Property>> _propertyRepositoryMock;
        private Mock<IRepository<Owner>> _ownerRepositoryMock;
        private Mock<IRepository<PropertyTrace>> _traceRepositoryMock;
        private UpdatePropertyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdatePropertyValidator();
            _propertyRepositoryMock = new Mock<IRepository<Property>>();
            _ownerRepositoryMock = new Mock<IRepository<Owner>>();
            _traceRepositoryMock = new Mock<IRepository<PropertyTrace>>();
            _handler = new UpdatePropertyCommandHandler(
                _propertyRepositoryMock.Object,
                _ownerRepositoryMock.Object,
                _traceRepositoryMock.Object,
                _validator);
        }

        [Test]
        public void Validator_WhenCommandIsValid_ShouldNotHaveValidationErrors()
        {
            var command = new UpdatePropertyCommand
            {
                PropertyId = Guid.NewGuid(),
                Name = "Updated Property",
                Address = "456 New St",
                Price = 200000,
                CodeInternal = "PROP-002",
                Year = 2023,
                IdOwner = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validator_WhenPropertyIdIsEmpty_ShouldHaveValidationError()
        {
            var command = new UpdatePropertyCommand
            {
                PropertyId = Guid.Empty
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.PropertyId);
        }

        [Test]
        public void Validator_WhenNameLengthIsInvalid_ShouldHaveValidationError()
        {
            var command = new UpdatePropertyCommand
            {
                PropertyId = Guid.NewGuid(),
                Name = "A"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Validator_WhenAddressLengthIsInvalid_ShouldHaveValidationError()
        {
            var command = new UpdatePropertyCommand
            {
                PropertyId = Guid.NewGuid(),
                Address = new string('A', 251) // Too long
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Test]
        public void Validator_WhenPriceIsNegative_ShouldHaveValidationError()
        {
            var command = new UpdatePropertyCommand
            {
                PropertyId = Guid.NewGuid(),
                Price = -1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price!.Value);
        }

        [Test]
        public void Validator_WhenTraceHasFutureDate_ShouldHaveValidationError()
        {
            var command = new UpdatePropertyCommand
            {
                PropertyId = Guid.NewGuid(),
                Trace = new PropertyTraceInfo
                {
                    DateSale = DateTime.UtcNow.AddDays(1),
                    Name = "Future Trace",
                    Value = 100000,
                    Tax = 1000
                }
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Trace!.DateSale);
        }

        [Test]
        public void Handler_WhenPropertyNotFound_ShouldThrowNotFoundException()
        {
            var command = new UpdatePropertyCommand
            {
                PropertyId = Guid.NewGuid()
            };

            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(command.PropertyId))
                .ReturnsAsync((Property?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(
                () => _handler.HandleAsync(command));
            Assert.That(ex.Message, Is.EqualTo($"Property with ID {command.PropertyId} not found"));
        }

        [Test]
        public void Handler_WhenOwnerNotFound_ShouldThrowNotFoundException()
        {
            var propertyId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var command = new UpdatePropertyCommand
            {
                PropertyId = propertyId,
                IdOwner = ownerId
            };

            var property = new Property { IdProperty = propertyId };
            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync(property);

            _ownerRepositoryMock.Setup(x => x.GetByIdAsync(ownerId))
                .ReturnsAsync((Owner?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(
                () => _handler.HandleAsync(command));
            Assert.That(ex.Message, Is.EqualTo($"Owner with ID {ownerId} not found"));
        }

        [Test]
        public async Task Handler_WhenUpdatingAllFields_ShouldUpdateSuccessfully()
        {
            var propertyId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var command = new UpdatePropertyCommand
            {
                PropertyId = propertyId,
                Name = "Updated Name",
                Address = "Updated Address",
                Price = 300000,
                CodeInternal = "NEW-001",
                Year = 2023,
                IdOwner = ownerId,
                Trace = new PropertyTraceInfo
                {
                    DateSale = DateTime.UtcNow.AddDays(-3),
                    Name = "New Trace",
                    Value = 300000,
                    Tax = 3000
                }
            };

            var property = new Property { IdProperty = propertyId };
            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync(property);

            var owner = new Owner { IdOwner = ownerId };
            _ownerRepositoryMock.Setup(x => x.GetByIdAsync(ownerId))
                .ReturnsAsync(owner);

            var result = await _handler.HandleAsync(command);

            Assert.That(result, Is.EqualTo(propertyId));
            Assert.Multiple(() =>
            {
                Assert.That(property.Name, Is.EqualTo(command.Name));
                Assert.That(property.Address, Is.EqualTo(command.Address));
                Assert.That(property.Price, Is.EqualTo(command.Price));
                Assert.That(property.CodeInternal, Is.EqualTo(command.CodeInternal));
                Assert.That(property.Year, Is.EqualTo(command.Year));
                Assert.That(property.IdOwner, Is.EqualTo(command.IdOwner));
            });

            _propertyRepositoryMock.Verify(x => x.UpdateAsync(property), Times.Once);
            _traceRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<PropertyTrace>()), Times.Once);
        }

        [Test]
        public async Task Handler_WhenUpdatingOnlyPrice_ShouldUpdateOnlyPrice()
        {
            var propertyId = Guid.NewGuid();
            var originalName = "Original Name";
            var newPrice = 250000m;

            var command = new UpdatePropertyCommand
            {
                PropertyId = propertyId,
                Price = newPrice
            };

            var property = new Property
            {
                IdProperty = propertyId,
                Name = originalName,
                Price = 200000
            };

            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync(property);

            await _handler.HandleAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(property.Name, Is.EqualTo(originalName));
                Assert.That(property.Price, Is.EqualTo(newPrice));
            });

            _propertyRepositoryMock.Verify(x => x.UpdateAsync(property), Times.Once);
            _traceRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<PropertyTrace>()), Times.Never);
        }
    }
}
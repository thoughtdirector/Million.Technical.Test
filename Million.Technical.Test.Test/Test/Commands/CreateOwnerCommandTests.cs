using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Handlers;
using Million.Technical.Test.Application.Shared.Methods;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Infrastructure.Validators.Commands;
using Million.Technical.Test.Libraries.Repositories;
using Moq;

namespace Million.Technical.Test.UnitTests.Commands
{
    [TestFixture]
    public class CreateOwnerCommandTests
    {
        private CreateOwnerValidator _validator;
        private Mock<IRepository<Owner>> _ownerRepositoryMock;
        private Mock<IImageService> _imageServiceMock;
        private CreateOwnerCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateOwnerValidator();
            _ownerRepositoryMock = new Mock<IRepository<Owner>>();
            _imageServiceMock = new Mock<IImageService>();
            _handler = new CreateOwnerCommandHandler(
                _ownerRepositoryMock.Object,
                _validator,
                _imageServiceMock.Object);
        }

        private static IFormFile CreateMockFormFile(string fileName, string contentType, long length)
        {
            var mock = new Mock<IFormFile>();
            mock.Setup(f => f.FileName).Returns(fileName);
            mock.Setup(f => f.ContentType).Returns(contentType);
            mock.Setup(f => f.Length).Returns(length);
            return mock.Object;
        }

        [Test]
        public void Validator_WhenCommandIsValid_ShouldNotHaveValidationErrors()
        {
            var command = new CreateOwnerCommand
            {
                Name = "Angel",
                Address = "123 Street",
                Birthday = DateTime.Now.AddYears(-30),
                Photo = CreateMockFormFile("test.jpg", "image/jpeg", 1024 * 1024)
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("Jo")]
        public void Validator_WhenNameIsInvalid_ShouldHaveValidationError(string? name)
        {
            var command = new CreateOwnerCommand
            {
                Name = name,
                Address = "123 Main Street",
                Birthday = DateTime.Now.AddYears(-30)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Validator_WhenAddressIsTooLong_ShouldHaveValidationError()
        {
            var longAddress = GenerateLongString(251);
            var command = new CreateOwnerCommand
            {
                Name = "Angel",
                Address = longAddress,
                Birthday = DateTime.Now.AddYears(-30)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address)
                .WithErrorMessage("Owner address cannot exceed 250 characters");
        }

        [Test]
        public void Validator_WhenBirthdayIsInFuture_ShouldHaveValidationError()
        {
            var command = new CreateOwnerCommand
            {
                Name = "Angel",
                Address = "123 Street",
                Birthday = DateTime.Now.AddDays(1)
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Birthday);
        }

        [Test]
        public async Task Handler_WhenCommandIsValid_ShouldCreateOwner()
        {
            var command = new CreateOwnerCommand
            {
                Name = "Angel",
                Address = "123 Street",
                Birthday = DateTime.Now.AddYears(-30)
            };

            var createdOwner = new Owner
            {
                IdOwner = Guid.NewGuid(),
                Name = command.Name,
                Address = command.Address,
                Birthday = command.Birthday!.Value
            };

            _ownerRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Owner>()))
                .ReturnsAsync(createdOwner);

            var result = await _handler.HandleAsync(command);

            Assert.That(result, Is.EqualTo(createdOwner.IdOwner));
            _ownerRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Owner>()), Times.Once);
        }

        private static string GenerateLongString(int length)
        {
            return new string('A', length);
        }
    }
}
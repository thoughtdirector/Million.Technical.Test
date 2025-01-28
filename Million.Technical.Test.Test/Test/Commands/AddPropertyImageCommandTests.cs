using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Commands.Handlers;
using Million.Technical.Test.Application.Shared.Methods;
using Million.Technical.Test.Domain.Entities;
using Million.Technical.Test.Infrastructure.Validators.Commands;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Repositories;
using Moq;

namespace Million.Technical.Test.UnitTests.Commands
{
    [TestFixture]
    public class AddPropertyImageCommandTests
    {
        private AddPropertyImageValidator _validator;
        private Mock<IRepository<Property>> _propertyRepositoryMock;
        private Mock<IRepository<PropertyImage>> _imageRepositoryMock;
        private Mock<IImageService> _imageServiceMock;
        private AddPropertyImageCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _validator = new AddPropertyImageValidator();
            _propertyRepositoryMock = new Mock<IRepository<Property>>();
            _imageRepositoryMock = new Mock<IRepository<PropertyImage>>();
            _imageServiceMock = new Mock<IImageService>();
            _handler = new AddPropertyImageCommandHandler(
                _propertyRepositoryMock.Object,
                _imageRepositoryMock.Object,
                _imageServiceMock.Object,
                _validator);
        }

        private static Mock<IFormFile> CreateMockFormFile(string fileName, string contentType, long length)
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.ContentType).Returns(contentType);
            fileMock.Setup(f => f.Length).Returns(length);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            return fileMock;
        }

        [Test]
        public void Validator_WhenCommandIsValid_ShouldNotHaveValidationErrors()
        {
            var mockFile = CreateMockFormFile("test.jpg", "image/jpeg", 1024).Object;
            var command = new AddPropertyImageCommand
            {
                PropertyId = Guid.NewGuid(),
                Image = mockFile,
                Enabled = true
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validator_WhenPropertyIdIsEmpty_ShouldHaveValidationError()
        {
            var mockFile = CreateMockFormFile("test.jpg", "image/jpeg", 1024).Object;
            var command = new AddPropertyImageCommand
            {
                PropertyId = Guid.Empty,
                Image = mockFile
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.PropertyId)
                .WithErrorMessage("Property Id is required");
        }

        [Test]
        public void Validator_WhenImageSizeExceedsLimit_ShouldHaveValidationError()
        {
            var largeFile = CreateMockFormFile("test.jpg", "image/jpeg", 12 * 1024 * 1024).Object; // 6MB
            var command = new AddPropertyImageCommand
            {
                PropertyId = Guid.NewGuid(),
                Image = largeFile
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Image.Length);
        }

        [Test]
        public void Handler_WhenPropertyNotFound_ShouldThrowNotFoundException()
        {
            var mockFile = CreateMockFormFile("test.jpg", "image/jpeg", 1024).Object;
            var command = new AddPropertyImageCommand
            {
                PropertyId = Guid.NewGuid(),
                Image = mockFile
            };

            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(command.PropertyId))
                .ReturnsAsync((Property?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(
                () => _handler.HandleAsync(command));
            Assert.That(ex.Message, Is.EqualTo($"Property with ID {command.PropertyId} not found"));
        }

        [Test]
        public async Task Handler_WhenImageIsValid_ShouldCreatePropertyImage()
        {
            var propertyId = Guid.NewGuid();
            var mockFile = CreateMockFormFile("test.jpg", "image/jpeg", 1024);
            var command = new AddPropertyImageCommand
            {
                PropertyId = propertyId,
                Image = mockFile.Object,
                Enabled = true
            };

            var property = new Property { IdProperty = propertyId };
            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync(property);

            var compressedImageData = new byte[] { 1, 2, 3 };
            _imageServiceMock.Setup(x => x.CompressImageAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(compressedImageData);

            var createdImage = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                IdProperty = propertyId,
                ImageData = compressedImageData,
                Enabled = true
            };

            _imageRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PropertyImage>()))
                .ReturnsAsync(createdImage);

            var result = await _handler.HandleAsync(command);

            Assert.That(result, Is.EqualTo(createdImage.IdPropertyImage));
            _imageServiceMock.Verify(x => x.CompressImageAsync(It.IsAny<byte[]>(), It.IsAny<string>()), Times.Once);
            _imageRepositoryMock.Verify(x => x.CreateAsync(It.Is<PropertyImage>(
                img => img.IdProperty == propertyId &&
                       img.ImageData == compressedImageData &&
                       img.Enabled)), Times.Once);
        }

        [Test]
        public void Handler_WhenImageProcessingFails_ShouldThrowException()
        {
            var propertyId = Guid.NewGuid();
            var mockFile = CreateMockFormFile("test.jpg", "image/jpeg", 1024);
            var command = new AddPropertyImageCommand
            {
                PropertyId = propertyId,
                Image = mockFile.Object
            };

            var property = new Property { IdProperty = propertyId };
            _propertyRepositoryMock.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync(property);

            _imageServiceMock.Setup(x => x.CompressImageAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Image processing failed"));

            Assert.ThrowsAsync<Exception>(() => _handler.HandleAsync(command));
            _imageRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<PropertyImage>()), Times.Never);
        }
    }
}
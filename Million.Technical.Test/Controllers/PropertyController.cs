using Microsoft.AspNetCore.Mvc;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Application.Queries;
using Million.Technical.Test.Application.Queries.Responses;
using Million.Technical.Test.Libraries.Exceptions;
using Million.Technical.Test.Libraries.Mediators;

namespace Million.Technical.Test.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    [Produces("application/json")]
    public class PropertyController : ControllerBase
    {
        private readonly Mediator _mediator;

        public PropertyController(Mediator mediator)
        {
            _mediator = mediator;
        }

        /// Creates a new property.
        /// The command containing the property details to be created.
        /// Returns the unique identifier (GUID) of the newly created property if successful.
        /// If validation errors occur, returns a 400 Bad Request with the errors.
        /// If the property is not found, returns a 404 Not Found.
        /// If an unexpected error occurs, returns a 500 Internal Server Error.
        [HttpPost("create_property")]
        public async Task<ActionResult<Guid>> CreateProperty(
            [FromBody] CreatePropertyCommand command)
        {
            try
            {
                var propertyId = await _mediator.SendAsync<CreatePropertyCommand, Guid>(command);
                return propertyId;
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the property");
            }
        }

        /// Adds an image to a property.
        /// The command containing the property ID and image details.
        /// Returns the unique identifier (GUID) of the newly added image if successful.
        /// If validation errors occur, returns a 400 Bad Request with the errors.
        /// If the property is not found, returns a 404 Not Found.
        /// If an unexpected error occurs, returns a 500 Internal Server Error.
        [HttpPost("add_property_image")]
        public async Task<ActionResult<Guid>> AddPropertyImage(
            [FromForm] AddPropertyImageCommand command)
        {
            try
            {
                var imageId = await _mediator.SendAsync<AddPropertyImageCommand, Guid>(command);
                return imageId;
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while adding the property image");
            }
        }

        /// Creates a trace for a property.
        /// The command containing the property trace details.
        /// Returns the unique identifier (GUID) of the newly created property trace if successful.
        /// If validation errors occur, returns a 400 Bad Request with the errors.
        /// If the property is not found, returns a 404 Not Found.
        /// If an unexpected error occurs, returns a 500 Internal Server Error.
        [HttpPost("create_property_trace")]
        public async Task<ActionResult<Guid>> CreatePropertyTrace(
           [FromBody] CreatePropertyTraceCommand command)
        {
            try
            {
                var traceId = await _mediator.SendAsync<CreatePropertyTraceCommand, Guid>(command);
                return traceId;
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the property trace");
            }
        }

        /// Changes the price of a property.
        /// The command containing the property ID and the new price.
        /// Returns a confirmation message if successful.
        /// If validation errors occur, returns a 400 Bad Request with the errors.
        /// If the property is not found, returns a 404 Not Found.
        /// If an unexpected error occurs, returns a 500 Internal Server Error.
        [HttpPut("change_property_price")]
        public async Task<ActionResult<string>> ChangePropertyPrice(
            [FromBody] ChangePropertyPriceCommand command)
        {
            try
            {
                var response = await _mediator.SendAsync<ChangePropertyPriceCommand, string>(command);
                return response;
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while changing the property price");
            }
        }

        /// Updates the details of a property.
        /// The command containing the updated property details.
        /// Returns the unique identifier (GUID) of the updated property if successful.
        /// If validation errors occur, returns a 400 Bad Request with the errors.
        /// If the property is not found, returns a 404 Not Found.
        /// If an unexpected error occurs, returns a 500 Internal Server Error.
        [HttpPut("update_property")]
        public async Task<ActionResult<Guid>> UpdateProperty(
            [FromBody] UpdatePropertyCommand command)
        {
            try
            {
                var propertyId = await _mediator.SendAsync<UpdatePropertyCommand, Guid>(command);
                return propertyId;
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the property");
            }
        }

        /// Retrieves a specific image of a property by its ID.
        /// propertyID The unique identifier of the property.
        /// imageId The unique identifier of the image.
        /// Returns the image as a JPEG file if found.
        /// If the image is not found, returns a 404 Not Found.
        /// If an unexpected error occurs, returns a 500 Internal Server Error.
        [HttpGet("property/{propertyId}/image/{imageId}")]
        [Produces("image/jpeg")]
        public async Task<IActionResult> GetPropertyImage(Guid propertyId, Guid imageId)
        {
            try
            {
                GetPropertyImageQuery query = new() { PropertyId = propertyId, ImageId = imageId };

                var imageData = await _mediator.SendAsync<GetPropertyImageQuery, byte[]>(query);

                if (imageData == null || imageData.Length == 0)
                    return NotFound("Image not found");

                return File(imageData, "image/jpeg");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the image");
            }
        }

        /// Retrieves a list of properties filtered by specific criteria.
        ///The query containing the filter criteria.
        /// Returns a list of properties matching the criteria.
        /// If an unexpected error occurs, returns a 500 Internal Server Error with error details.
        [HttpGet("get_properies_by_filters")]
        public async Task<ActionResult<IEnumerable<PropertyDetailDto>>> GetProperties(
           [FromQuery] GetPropertiesQuery query)
        {
            try
            {
                var properties = await _mediator.SendAsync<GetPropertiesQuery, IEnumerable<PropertyDetailDto>>(query);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving properties", error = ex.Message });
            }
        }
    }
}
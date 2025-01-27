using Microsoft.AspNetCore.Mvc;
using Million.Technical.Test.Application.Commands;
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the property");
            }
        }

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
    }
}
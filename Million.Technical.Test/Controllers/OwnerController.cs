using Microsoft.AspNetCore.Mvc;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Libraries.Mediators;

namespace Million.Technical.Test.Api.Controllers
{
    /// API Controller for managing Owner-related operations.
    [ApiController]
    [Route("api/owner")]
    [Produces("application/json")]
    public class OwnerController : ControllerBase
    {
        private readonly Mediator _mediator;

        /// Initializes a new instance of the class.
        ///An instance of the Mediator to handle commands and queries
        public OwnerController(Mediator mediator)
        {
            _mediator = mediator;
        }

        /// Creates a new owner.
        /// The command containing the owner details to be created.
        /// Returns the unique identifier (GUID) of the newly created owner if successful.
        /// If validation errors occur, returns a 400 Bad Request with the errors.
        /// If an unexpected error occurs, returns a 500 Internal Server Error.
        [HttpPost("create_owner")]
        public async Task<ActionResult<Guid>> CreateOwner(
            [FromForm] CreateOwnerCommand command)
        {
            try
            {
                // Sends the CreateOwnerCommand to the Mediator and awaits the resulting GUID.
                var ownerId = await _mediator.SendAsync<CreateOwnerCommand, Guid>(command);

                // Returns the GUID of the newly created owner with a 200 OK response.
                return ownerId;
            }
            catch (FluentValidation.ValidationException ex)
            {
                // Handles validation exceptions from FluentValidation.
                // Returns a 400 Bad Request response with detailed error information.
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
            catch (Exception)
            {
                // Handles any other unexpected exceptions.
                // Returns a 500 Internal Server Error response with a generic error message.
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the owner");
            }
        }
    }
}
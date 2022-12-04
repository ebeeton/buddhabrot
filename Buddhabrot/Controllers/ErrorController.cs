using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Exception handler.
	/// </summary>
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorController : ControllerBase
	{
		/// <summary>
		/// Development exception handler.
		/// </summary>
		/// <param name="hostEnvironment"><see cref="IHostEnvironment"/>.</param>
		/// <returns><see cref="ControllerBase.Problem"/></returns>
		[Route("/error-development")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
		{
			if (!hostEnvironment.IsDevelopment())
			{
				return NotFound();
			}

			var exceptionHandlerFeature =
				HttpContext.Features.Get<IExceptionHandlerFeature>()!;

			return Problem(
				detail: exceptionHandlerFeature.Error.StackTrace,
				title: exceptionHandlerFeature.Error.Message);
		}

		/// <summary>
		/// Non-development exception handler.
		/// </summary>
		/// <returns></returns>
		[Route("/error")]
		public IActionResult HandleError() => Problem();
	}
}

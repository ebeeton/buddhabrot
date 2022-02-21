using Buddhabrot.API.DTO;
using Buddhabrot.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Buddhabrot.Controllers
{
	/// <summary>
	/// Buddhabrot API controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class BuddhabrotController : ControllerBase
	{
		private const string PngContentType = "image/png";

		/// <summary>
		/// Gets a buddhabrot image.
		/// </summary>
		/// <returns>A buddhabrot image.</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get([FromQuery]RenderParameters parameters)
		{
			try
			{
				var image = await Renderer.RenderPng(parameters.Width, parameters.Height);
				return File(image, PngContentType);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Rendering failed.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}

using Buddhabrot.API.DTO;
using Buddhabrot.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Buddhabrot.API.Controllers
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
		/// Gets a Buddhabrot image.
		/// </summary>
		/// <param name="parameters">Image generation parameters.</param>
		/// <returns>A Buddhabrot image.</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get([FromQuery] RenderParameters parameters)
		{
			try
			{
				var stopwatch = System.Diagnostics.Stopwatch.StartNew();
				var renderer = new BuddhabrotRenderer(parameters.Width, parameters.Height, parameters.MaxIterations);
				var image = await renderer.RenderPng();
				Log.Information($"Rendered image in {stopwatch.ElapsedMilliseconds} ms.");
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

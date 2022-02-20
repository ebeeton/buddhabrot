using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Buddhabrot.Controllers
{
	/// <summary>
	/// Buddhabrot API controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class Buddhabrot : ControllerBase
	{
		/// <summary>
		/// Gets a buddhabrot image.
		/// </summary>
		/// <returns>A buddhabrot image.</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult Get()
		{
			try
			{
				return Ok("Hello, world!");
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Rendering failed.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}

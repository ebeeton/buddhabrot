using Buddhabrot.API.Services;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Image controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : Controller
	{
		private readonly IPlotRepository _repository;

		/// <summary>
		/// Instantiates an <see cref="ImageController"/>.
		/// </summary>
		/// <param name="repository"><see cref="IPlotRepository"/>.</param>
		public ImageController(IPlotRepository repository) => _repository = repository;

		/// <summary>
		/// Gets an image.
		/// </summary>
		/// <returns>A task representing the work to get the image.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			var plot = _repository.Find(id);
			if (plot == null)
			{
				return new NotFoundResult();
			}

			return File(await ImageService.ToPng(plot), ImageService.PngContentType);
		}
	}
}

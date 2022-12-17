using Buddhabrot.API.Services;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Images controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IImageRepository _repository;

		/// <summary>
		/// Instantiates an <see cref="ImagesController"/>.
		/// </summary>
		/// <param name="repository"><see cref="IImageRepository"/>.</param>
		public ImagesController(IImageRepository repository) => _repository = repository;

		/// <summary>
		/// Gets an image.
		/// </summary>
		/// <param name="id"><see cref="Core.Models.Plot"/> ID.</param>
		/// <returns>A task representing the work to get the image.</returns>
		[HttpGet("{id}", Name = "GetImage")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetAsync(int id)
		{
			// FindAsync appears to have performance problems when dealing with varbinary(max).
			// https://stackoverflow.com/a/28619983
			var image = _repository.Find(id);
			if (image == null)
			{
				return new NotFoundResult();
			}

			return File(await ImageService.ToPng(image), ImageService.PngContentType);
		}
	}
}

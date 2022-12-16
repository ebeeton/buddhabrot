using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.API.Services;
using Buddhabrot.Core.Models;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Mandelbrot API controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class PlotsController : ControllerBase
	{
		/// <summary>
		/// AutoMapper.
		/// </summary>
		protected IMapper _mapper;

		/// <summary>
		/// Plot repository.
		/// </summary>
		protected IPlotRepository _repository;

		/// <summary>
		/// Instantiates a <see cref="PlotsController"/>.
		/// </summary>
		/// <param name="repository">Plot repository.</param>
		/// <param name="mapper">AutoMapper.</param>
		public PlotsController(IPlotRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Plots a Mandelbrot image.
		/// </summary>
		/// <param name="request">Mandelbrot plot request.</param>
		/// <returns>The ID of the queued plot.</returns>
		[HttpPost("Mandelbrot")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async Task<IActionResult> PlotAsync(MandelbrotRequest request)
		{
			var plot = _mapper.Map<Plot>(request);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlot(plot.Id);

			var response = _mapper.Map<MandelbrotResponse>(plot);
			return AcceptedAtRoute("GetImage", new { id = response.Id }, response);
		}

		/// <summary>
		/// Plots a Buddhabrot image.
		/// </summary>
		/// <param name="request">Buddhabrot plot request.</param>
		/// <returns>The ID of the queued plot.</returns>
		[HttpPost("Buddhabrot")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async Task<IActionResult> PlotAsync(BuddhabrotRequest request)
		{
			var plot = _mapper.Map<Plot>(request);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlot(plot.Id);

			var response = _mapper.Map<BuddhabrotResponse>(plot);
			return AcceptedAtRoute("GetImage", new { id = plot.Id }, response);
		}

		/// <summary>
		/// Gets an image.
		/// </summary>
		/// <returns>A task representing the work to get the image.</returns>
		[HttpGet("{id}", Name = "GetImage")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetAsync(int id, [FromServices]IImageRepository imageRepository)
		{
			// FindAsync seems to have performance problems when dealing with varbinary(max) - the image data.
			// https://stackoverflow.com/a/28619983
			var image = imageRepository.Find(id);
			if (image == null)
			{
				return new NotFoundResult();
			}

			return File(await ImageService.ToPng(image), ImageService.PngContentType);
		}
	}
}

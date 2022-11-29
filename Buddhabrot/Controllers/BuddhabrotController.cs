using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.Core.Plotting;
using Buddhabrot.Persistence;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Buddhabrot API controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class BuddhabrotController : ControllerBase
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
		/// Instantiates a <see cref="BuddhabrotController"/>.
		/// </summary>
		/// <param name="mapper">AutoMapper.</param>
		/// <param name="repository">Plot repository.</param>
		public BuddhabrotController(IPlotRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Plots a Buddhabrot image.
		/// </summary>
		/// <param name="parameters">Image generation parameters.</param>
		/// <returns>A Buddhabrot image.</returns>
		[HttpPost("Plot")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Plot(BuddhabrotParameters parameters)
		{
			try
			{
				var plotParameters = _mapper.Map<Core.Models.BuddhabrotParameters>(parameters);
				await _repository.EnqueuePlotRequest(plotParameters);

				var plotter = new BuddhabrotPlotter(plotParameters);
				var plot = await plotter.Plot();
				_repository.Add(plot);
				await _repository.SaveChangesAsync();

				// TODO:: Move this into an image conversion service.
				using var image = Image.LoadPixelData<Rgb24>(plot.ImageData, plot.Width, plot.Height);
				var output = new MemoryStream();
				await image.SaveAsPngAsync(output);
				output.Seek(0, SeekOrigin.Begin);

				return File(output, Constants.PngContentType);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Buddhabrot plot failed.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}

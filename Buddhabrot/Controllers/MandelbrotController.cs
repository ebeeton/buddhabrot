using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.Core.Plotting;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Mandelbrot API controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class MandelbrotController : ControllerBase
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
		/// Instantiates a <see cref="MandelbrotController"/>.
		/// </summary>
		/// <param name="repository">Plot repository.</param>
		/// <param name="mapper">AutoMapper.</param>
		public MandelbrotController(IPlotRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Plots a Mandelbrot image.
		/// </summary>
		/// <param name="parameters">Image generation parameters.</param>
		/// <returns>A Mandelbrot image.</returns>
		[HttpPost("Plot")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Plot(MandelbrotParameters parameters)
		{
			try
			{
				var plotParameters = _mapper.Map<Core.Models.MandelbrotParameters>(parameters);
				await _repository.EnqueuePlotRequest(plotParameters);

				var plotter = new MandelbrotPlotter(plotParameters);
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
				Log.Error(ex, "Mandelbrot plot failed.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}

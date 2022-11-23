using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.Core.Plotting;
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
		/// Instantiates a <see cref="MandelbrotController"/>.
		/// </summary>
		/// <param name="mapper">AutoMapper.</param>
		public MandelbrotController(IMapper mapper) => _mapper = mapper;

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
				var stopwatch = System.Diagnostics.Stopwatch.StartNew();
				var plotter = new MandelbrotPlotter(_mapper.Map<Core.Models.MandelbrotParameters>(parameters));
				var plot = await plotter.Plot();
				using var image = Image.LoadPixelData<Rgb24>(plot.ImageData, plot.Width, plot.Height);
				var output = new MemoryStream();
				await image.SaveAsPngAsync(output);
				output.Seek(0, SeekOrigin.Begin);

				Log.Information($"Plotted image in {stopwatch.ElapsedMilliseconds} ms.");
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

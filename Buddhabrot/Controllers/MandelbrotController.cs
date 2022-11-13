using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.Core.Plotting;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Mandelbrot API controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class MandelbrotController : ControllerBase
	{
		protected IMapper _mapper;

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
				var plotter = new MandelbrotPlotter(_mapper.Map<Core.Parameters.MandelbrotParameters>(parameters));
				var image = await plotter.PlotPng();
				Log.Information($"Plotted image in {stopwatch.ElapsedMilliseconds} ms.");
				return File(image, Constants.PngContentType);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Mandelbrot plot failed.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}

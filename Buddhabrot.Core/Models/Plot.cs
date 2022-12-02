using Buddhabrot.Core.Interfaces;
using Newtonsoft.Json;

namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// A Buddhabrot or Mandelbrot plot.
	/// </summary>
	public class Plot
	{
		/// <summary>
		/// Instantiates a <see cref="Plot"/>.
		/// </summary>
		public Plot() { }

		/// <summary>
		/// Instantiates a <see cref="Plot"/>.
		/// </summary>
		/// <param name="parameters"><see cref="IPlotParameters"/> used to generate the plot.</param>
		/// <param name="plotType"><see cref="PlotType"/>.</param>
		public Plot(IPlotParameters parameters, PlotType plotType)
		{
			Width = parameters.Width;
			Height = parameters.Height;
			PlotParams = JsonConvert.SerializeObject(parameters);
			PlotType = plotType;
		}

		/// <summary>
		/// Primary key.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Type of plot.
		/// </summary>
		public PlotType PlotType { get; set; }

		/// <summary>
		/// The parameters used to generate the plot, in JSON.
		/// </summary>
		public string? PlotParams { get; set; }

		/// <summary>
		/// Image width.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Image height.
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// When the record was created.
		/// </summary>
		public DateTime CreatedUTC { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// When the plot began.
		/// </summary>
		public DateTime? PlotBeginUTC { get; set; }

		/// <summary>
		/// When the plot ended.
		/// </summary>
		public DateTime? PlotEndUTC { get; set; }

		/// <summary>
		/// The raw 24-bit per pixel image data.
		/// </summary>
		public byte[]? ImageData { get; set; }
	}
}
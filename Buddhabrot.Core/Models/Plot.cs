using Buddhabrot.Core.Interfaces;
using Destructurama.Attributed;
using Newtonsoft.Json;

namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// A Buddhabrot or Mandelbrot plot.
	/// </summary>
	public class Plot
	{
		/// <summary>
		/// Possible states of a <see cref="Plot"/>.
		/// </summary>
		public enum PlotState
		{
			Pending,
			Started,
			Complete
		};

		/// <summary>
		/// Instantiates a <see cref="Plot"/>.
		/// </summary>
		public Plot() { }

		/// <summary>
		/// Instantiates a <see cref="Plot"/>.
		/// </summary>
		/// <param name="width">Width of the image in pixels.</param>
		/// <param name="height">Height of the image in pixels.</param>
		/// <param name="parameters"><see cref="IPlotParameters"/> used to generate the plot.</param>
		/// <param name="plotType"><see cref="PlotType"/>.</param>
		public Plot(int width, int height, IPlotParameters parameters, PlotType plotType)
		{
			Width = width;
			Height = height;
			PlotParams = JsonConvert.SerializeObject(parameters);
			PlotType = plotType;
		}

		/// <summary>
		/// Gets <see cref="IPlotParameters"/> from the parameters JSON.
		/// </summary>
		/// <returns><see cref="IPlotParameters"/>.</returns>
		public IPlotParameters GetPlotParameters()
		{
			if (string.IsNullOrWhiteSpace(PlotParams))
			{
				throw new InvalidOperationException($"{nameof(PlotParams)} cannot be null or white space.");
			}

			switch (PlotType)
			{
				case PlotType.Mandelbrot:
					{
						var parameters = JsonConvert.DeserializeObject<MandelbrotParameters>(PlotParams);
						if (parameters == null)
						{
							throw new InvalidOperationException($"Could not deserialize type {nameof(MandelbrotParameters)}.");
						}
						return parameters;
					}
				case PlotType.Buddhabrot:
					{

						var parameters = JsonConvert.DeserializeObject<BuddhabrotParameters>(PlotParams);
						if (parameters == null)
						{
							throw new InvalidOperationException($"Could not deserialize type {nameof(BuddhabrotParameters)}.");
						}
						return parameters;
					}
				default:
					throw new InvalidOperationException("Unsupported plot type.");
			}
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
		public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// When the plot started.
		/// </summary>
		public DateTime? StartedUtc { get; set; }

		/// <summary>
		/// When the plot was completed.
		/// </summary>
		public DateTime? CompletedUtc { get; set; }

		/// <summary>
		/// The raw 24-bit per pixel image data.
		/// </summary>
		[NotLogged]
		public byte[]? ImageData { get; set; }

		/// <summary>
		/// Gets the <see cref="PlotState"/>.
		/// </summary>
		public PlotState State
		{
			get
			{
				if (StartedUtc == null)
				{
					return PlotState.Pending;
				}
				else if (CompletedUtc == null)
				{
					return PlotState.Started;
				}
				else
				{
					return PlotState.Complete;
				}
			}
			set
			{

			}
		}
	}
}
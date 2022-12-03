using Buddhabrot.Core.Models;
using Newtonsoft.Json;

namespace Buddhabrot.Core.Plotting
{
	/// <summary>
	/// Creates instances of classes derived from <see cref="Plotter"/>.
	/// </summary>
	public static class PlotterFactory
	{
		/// <summary>
		/// Get an instance of a class derived from <see cref="Plotter"/> by <see cref="PlotType"/>.
		/// </summary>
		/// <param name="plotType"><see cref="PlotType"/>.</param>
		/// <param name="paramsJson">Plotter-specific parameters in JSON.</param>
		/// <returns>Instance of a class derived from <see cref="Plotter"/>.</returns>
		public static Plotter GetPlotter(PlotType plotType, string paramsJson)
		{
			if (string.IsNullOrWhiteSpace(paramsJson))
			{
				throw new ArgumentNullException(nameof(paramsJson));
			}

			switch (plotType)
			{
				case PlotType.Mandelbrot:
					{
						var parameters = JsonConvert.DeserializeObject<MandelbrotParameters>(paramsJson);
						if (parameters == null)
						{
							throw new ArgumentException($"Could not deserialize type {nameof(MandelbrotParameters)}.", nameof(paramsJson));
						}
						return new MandelbrotPlotter(parameters);
					}
				case PlotType.Buddhabrot:
					{
						var parameters = JsonConvert.DeserializeObject<BuddhabrotParameters>(paramsJson);
						if (parameters == null)
						{
							throw new ArgumentException($"Could not deserialize type {nameof(BuddhabrotParameters)}.", nameof(paramsJson));
						}
						return new BuddhabrotPlotter(parameters);
					}
				default:
					throw new ArgumentException("Unsupported plot type.");
			}
		}
	}
}

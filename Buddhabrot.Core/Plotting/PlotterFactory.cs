using Buddhabrot.Core.Models;

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
		public static Plotter GetPlotter(Plot plot)
		{
			switch (plot.PlotType)
			{
				case PlotType.Mandelbrot:
					{
						return new MandelbrotPlotter(plot);
					}
				case PlotType.Buddhabrot:
					{
						return new BuddhabrotPlotter(plot);
					}
				default:
					throw new ArgumentException("Unsupported plot type.");
			}
		}
	}
}

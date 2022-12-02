﻿using Buddhabrot.Core.Interfaces;

namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// Parameters with which to plot a Buddhabrot image.
	/// </summary>
	public class BuddhabrotParameters : IPlotParameters
	{
		/// <summary>
		/// Image width.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Image height.
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// Maximum number of iterations for each pixel.
		/// </summary>
		public int MaxIterations { get; set; }

		/// <summary>
		/// Maximum number of iterations for the intial sample set.
		/// </summary>
		public int MaxSampleIterations { get; set; }

		/// <summary>
		/// Size of the random sample set as a percentage of the total
		/// number of pixels in the image to be generated.
		/// </summary>
		public double SampleSize { get; set; }

		/// <summary>
		/// Number of passes to build the image.
		/// </summary>
		public int Passes { get; set; }

		/// <summary>
		/// Whether to render the same pixel value to each channel.
		/// </summary>
		public bool Grayscale { get; set; }
	}
}

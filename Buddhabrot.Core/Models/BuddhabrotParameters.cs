﻿using Buddhabrot.Core.Interfaces;

namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// Parameters with which to plot a Buddhabrot image.
	/// </summary>
	public class BuddhabrotParameters : IPlotParameters
	{
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
		/// number of pixels in the image.
		/// </summary>
		public double SampleSize { get; set; }

		/// <summary>
		/// Whether to render the same pixel value to each channel.
		/// </summary>
		public bool Grayscale { get; set; }
	}
}

using Buddhabrot.Core.Math;
using Buddhabrot.Core.Parameters;
using Serilog;
using System.Collections.Concurrent;
using System.Numerics;

namespace Buddhabrot.Core.Plotting
{
	/// <summary>
	/// A Buddhabrot image plotter.
	/// </summary>
	public class BuddhabrotPlotter : Plotter
	{
		/// <summary>
		/// Symbolic constants for RGB channel colors.
		/// </summary>
		protected enum Channels
		{
			Red = 0,
			Green,
			Blue,
		};

		/// <summary>
		/// RGB channels of width * height.
		/// </summary>
		protected int[,] _channels;

		/// <summary>
		/// The maximum number of iterations to perform on each pixel.
		/// </summary>
		protected readonly int _maxIterations;

		/// <summary>
		/// The maximum number of iterations for the intial sample set.
		/// </summary>
		protected readonly int _maxSampleIterations;

		/// <summary>
		/// How many random points on the complex plane are chosen to build the image.
		/// </summary>
		protected readonly int _sampleSize;

		/// <summary>
		/// The total number of pixels per channel.
		/// </summary>
		protected readonly int _pixelsPerChannel;

		/// <summary>
		/// Instantiates a Buddhabrot image plotter.
		/// </summary>
		/// <param name="parameters">Parameters used to plot the image.</param>
		public BuddhabrotPlotter(BuddhabrotParameters parameters) : base(parameters.Width, parameters.Height)
		{
			_maxIterations = parameters.MaxIterations;
			_maxSampleIterations = parameters.MaxSampleIterations;
			_pixelsPerChannel = parameters.Width * parameters.Height;
			_sampleSize = (int)(parameters.SampleSize * _pixelsPerChannel);
			_channels = new int[RGBBytesPerPixel, _pixelsPerChannel];
			Log.Information("Buddhabrot plotter instantiated: {@Parameters}", parameters);
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		protected override void Plot()
		{
			// Plot  each channel.
			for (int i = 0; i < RGBBytesPerPixel; ++i)
			{
				PlotChannel(i);
			}
#if false
			// Separate RGB channel plotting method isn't working yet.
			MergeFinalImage();
#endif
		}

		/// <summary>
		/// Plot a Buddhabrot image to one of the RGB image channels.
		/// </summary>
		/// <param name="channel">Index of channel to plot to.</param>
		protected void PlotChannel(int channel)
		{
			// Generate a set of random points not in the Mandelbrot set.
			// We don't care about orbits yet.
			var samplePoints = new ConcurrentBag<Complex>();
			Parallel.For(0, _sampleSize, _ =>
			{
				var point = RandomPointOnComplexPlane();
				if (IsInMandelbrotSet(point, _maxSampleIterations, ref _))
				{
					return;
				}

				samplePoints.Add(point);
			});

			Log.Information($"Using sample size {_sampleSize}. Sample points outside the Mandelbrot set: {samplePoints.Count}.");

			// Iterate the sample set and plot their orbits.
			Parallel.ForEach(samplePoints, p =>
			{
				int iterations = 0;
				var orbits = new Complex[_maxIterations];

				PlotOrbits(p, ref iterations, ref orbits);

				for (int i = 0; i < iterations; ++i)
				{
					var pixelX = (int)Linear.Scale(orbits[i].Real, InitialMinX, InitialMaxX, 0, _width);
					var pixelY = (int)Linear.Scale(orbits[i].Imaginary, InitialMinY, InitialMaxY, 0, _height);

					// TODO:: Reject these before conversion from complex plane to pixels, save some cycles?
					if (!PixelInBounds(pixelX, pixelY))
					{
						continue;
					}

#if true
					// Separate RGB channel plotting method isn't working yet.
					var index = pixelY * _bytesPerLine + pixelX * RGBBytesPerPixel;
					lock (_imageBuffer)
					{
						++_imageBuffer[index];
						++_imageBuffer[index + 1];
						++_imageBuffer[index + 2];
					}
#else
					// Two or more threads could be incrementing the same pixel,
					// so a synchronization method is necessary here.
					var index = pixelY * pixelX;
					Interlocked.Increment(ref _channels[channel, index]);
#endif
				}
			});
		}

		/// <summary>
		/// Generate a random point on the complex plane near the Mandelbrot set.
		/// </summary>
		/// <returns>A random point on the complex plane near the Mandelbrot set.</returns>
		protected static Complex RandomPointOnComplexPlane()
		{
			var random = new Random();
			var real = random.NextDouble() * (InitialMaxX - InitialMinX) + InitialMinX;
			var imaginary = random.NextDouble() * (InitialMaxY - InitialMinY) + InitialMinY;
			return new Complex(real, imaginary);
		}

		/// <summary>
		/// Plots the orbits of a point on the complex plane not in the Mandelbrot set.
		/// </summary>
		/// <param name="c">A point on the complex plane not in the Mandelbrot set.</param>
		/// <param name="iterations">The number of iterations to escape to infinity.</param>
		/// <param name="orbits">An array of size MaxIterations.</param>
		protected void PlotOrbits(Complex c, ref int iterations, ref Complex[] orbits)
		{
			var z = new Complex(0, 0);
			for (int i = 0; i < _maxIterations; ++i)
			{
				if (z.Magnitude > Bailout)
				{
					// Not in the set.
					iterations = i;
					return;
				}

				orbits[i] = z;
				z = z * z + c;
			}
		}

		/// <summary>
		/// Combine the separate RGB channels into the final image output data.
		/// </summary>
		protected void MergeFinalImage()
		{			
			for (int i = 0; i < _pixelsPerChannel; ++i)
			{
				var index = i * RGBBytesPerPixel;
				_imageBuffer[index] = (byte)_channels[(int)Channels.Red, i];
				_imageBuffer[index + 1] = (byte)_channels[(int)Channels.Green, i];
				_imageBuffer[index + 2] = (byte)_channels[(int)Channels.Blue, i];
			}
		}
	}
}

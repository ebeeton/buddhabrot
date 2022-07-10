﻿using System.Numerics;

namespace Buddhabrot.Core
{
	/// <summary>
	/// A Buddhabrot image renderer.
	/// </summary>
	public class BuddhabrotRenderer : Renderer
	{
		/// <summary>
		/// Instantiates a Mandelbrot image renderer.
		/// </summary>
		/// <param name="width">Width of the image in pixels.</param>
		/// <param name="height">Height of the image in pixels.</param>
		/// <param name="maxIterations"></param>
		public BuddhabrotRenderer(int width, int height, int maxIterations) : base(width, height, maxIterations)
		{

		}

		/// <summary>
		/// Determines if a point is in the Mandelbrot set.
		/// </summary>
		/// <param name="c">A point represented as a complex number.</param>
		/// <param name="iterations">The number of iterations for points not in the set to escape to infinity.</param>
		/// <param name="orbits">The orbits of the point as it is iterated.</param>
		/// <returns>True if a point is in the Mandelbrot set.</returns>
		public bool IsInMandelbrotSet(Complex c, ref int iterations, ref Complex[] orbits)
		{
			var z = new Complex(0, 0);
			for (int i = 0; i < MaxIterations; ++i)
			{
				if (z.Magnitude > Bailout)
				{
					// Not in the set.
					iterations = i;
					return false;
				}

				orbits[i] = z;
				z = z * z + c;
			}

			// "Probably" in the set.
			return true;
		}

		/// <summary>
		/// Render a single pixel in the image.
		/// </summary>
		/// <param name="x">Horizontal pixel coordinate, left to right.</param>
		/// <param name="y">Vertical pixel coordinate, top to bottom.</param>
		/// <param name="c">Pixel scaled to the range in the Mandelbrot set.</param>
		protected override void RenderPixel(int x, int y, Complex c)
		{
			int iterations = 0;
			var orbits = new Complex[MaxIterations];
			if (IsInMandelbrotSet(c, ref iterations, ref orbits))
			{
				// Leave points in the set black.
				return;
			}

			// Grayscale plot based on how quickly the point escapes.
			var color = (byte)((double)iterations / MaxIterations * 255);
			var line = y * BytesPerLine;
			_imageData[line + x] =
			_imageData[line + x + 1] =
			_imageData[line + x + 2] = color;
		}
	}
}
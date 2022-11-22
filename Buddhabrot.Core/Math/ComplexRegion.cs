using System.Numerics;

namespace Buddhabrot.Core.Math
{
	/// <summary>
	/// A region on the complex plane defined by minimum and maximum real and imaginary values.
	/// </summary>
	public class ComplexRegion
	{
		/// <summary>
		/// Instantiates a <see cref="ComplexRegion"/>.
		/// </summary>
		public ComplexRegion() { }

		/// <summary>
		/// Instantiates a <see cref="ComplexRegion"/>.
		/// </summary>
		/// <param name="minReal">Minimum real value.</param>
		/// <param name="maxReal">Maximum real value.</param>
		/// <param name="minImaginary">Minimum imaginary value.</param>
		/// <param name="maxImaginary">Maximum imaginary value.</param>
		public ComplexRegion(double minReal, double maxReal, double minImaginary, double maxImaginary)
		{
			MinReal = minReal;
			MaxReal = maxReal;
			MinImaginary = minImaginary;
			MaxImaginary = maxImaginary;
		}

		/// <summary>
		/// Minimum real value.
		/// </summary>
		public double MinReal { get; set; }

		/// <summary>
		/// Maximum real value.
		/// </summary>
		public double MaxReal { get; set; }

		/// <summary>
		/// Minimum imaginary value.
		/// </summary>
		public double MinImaginary { get; set; }

		/// <summary>
		/// Maximum imaginary value.
		/// </summary>
		public double MaxImaginary { get; set; }

		/// <summary>
		/// Is a given point on the complex plane inside the region?
		/// </summary>
		/// <param name="complex"><see cref="Complex"/>.</param>
		/// <returns></returns>
		public bool PointInRegion(Complex complex) => complex.Real >= MinReal && complex.Real <= MaxReal && complex.Imaginary >= MinImaginary && complex.Imaginary <= MaxImaginary;

		/// <summary>
		/// Adjust the minimum and maximum imaginary values to match an aspect ratio.
		/// </summary>
		/// <param name="height">Height.</param>
		/// <param name="width">Width.</param>
		public void MatchAspectRatio(int width, int height)
		{
			var complexWidth = MaxReal - MinReal;
			var aspectRatio = (double)height / width;
			var newComplexHeight = complexWidth * aspectRatio;
			var halfComplexHeightDelta = (newComplexHeight - (MaxImaginary - MinImaginary)) / 2.0;
			MinImaginary -= halfComplexHeightDelta;
			MaxImaginary += halfComplexHeightDelta;
		}
	}
}

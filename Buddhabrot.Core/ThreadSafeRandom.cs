using Serilog;

namespace Buddhabrot.Core
{
	/// <summary>
	/// Thread-safe pseudo-random number generator.
	/// </summary>
	/// <remarks>
	/// See https://stackoverflow.com/q/3049467
	/// </remarks>
	public static class ThreadSafeRandom
	{
		private static readonly Random _random = new();

		/// <summary>
		/// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
		/// </summary>
		/// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
		public static double NextDouble()
		{
			double next;
			lock (_random)
			{
				next = _random.NextDouble();
			}
			return next;
		}
	}
}

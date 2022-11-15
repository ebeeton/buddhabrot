using Serilog;

namespace Buddhabrot.Core
{
	/// <summary>
	/// Thread-safe pseudo-random number generator.
	/// </summary>
	/// <remarks>
	/// See https://stackoverflow.com/q/3049467
	/// </remarks>
	public class ThreadSafeRandom
	{
		private static readonly Random _seeder = new();

		[ThreadStatic]
		private static Random? _threadLocal;

		/// <summary>
		/// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
		/// </summary>
		/// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
		public double NextDouble()
		{
			if (_threadLocal == null)
			{
				int seed;
				lock (_seeder)
				{
					seed = _seeder.Next();
				}
				_threadLocal = new Random(seed);
				Log.Debug("Thread local Random instantiated.");
			}

			return _threadLocal.NextDouble();
		}
	}
}

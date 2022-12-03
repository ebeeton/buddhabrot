using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Buddhabrot.Test
{
	/// <summary>
	/// Extension methods for <see cref="Assert"/>.
	/// </summary>
	public static class AssertExtensions
	{
		/// <summary>
		/// Tests whether two byte arrays have the same length and contents.
		/// </summary>
		/// <param name="assert"><see cref="Assert"/>.</param>
		/// <param name="expected">Expected byte array.</param>
		/// <param name="actual">Actual byte array.</param>
		public static void AreEqual(this Assert assert, byte[] expected, byte[] actual)
		{
			Assert.AreEqual(expected.Length, actual.Length);
			for (int i = 0; i < expected.Length; ++i)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}
		}
	}
}

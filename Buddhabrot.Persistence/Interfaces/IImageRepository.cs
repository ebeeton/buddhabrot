using Buddhabrot.Core.Models;

namespace Buddhabrot.Persistence.Interfaces
{
	/// <summary>
	/// Repository for <see cref="ImageRgb"/> data.
	/// </summary>
	public interface IImageRepository
	{
		/// <summary>
		/// Finds an <see cref="ImageRgb"/> by ID.
		/// </summary>
		/// <param name="id"><see cref="ImageRgb"/> ID.</param>
		/// <returns>An <see cref="ImageRgb"/>.</returns>
		public ImageRgb? Find(int id);
	}
}

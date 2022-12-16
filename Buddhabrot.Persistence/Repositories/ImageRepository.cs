using Buddhabrot.Core.Models;
using Buddhabrot.Persistence.Contexts;
using Buddhabrot.Persistence.Interfaces;

namespace Buddhabrot.Persistence.Repositories
{

	/// <summary>
	/// Repository for <see cref="ImageRgb"/> data.
	/// </summary>
	public class ImageRepository : IImageRepository
	{
		/// <summary>
		/// Database context.
		/// </summary>
		private readonly BuddhabrotContext _context;

		/// <summary>
		/// Instantiates an <see cref="ImageRepository"/>.
		/// </summary>
		/// <param name="context"><see cref="BuddhabrotContext"/>.</param>
		public ImageRepository(BuddhabrotContext context) => _context = context;

		/// <summary>
		/// Finds an <see cref="ImageRgb"/> by ID.
		/// </summary>
		/// <param name="id"><see cref="ImageRgb"/> ID.</param>
		/// <returns>An <see cref="ImageRgb"/>.</returns>
		public ImageRgb? Find(int id)
		{
			return _context.Images.Find(id);
		}
	}
}

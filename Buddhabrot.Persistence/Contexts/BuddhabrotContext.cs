using Buddhabrot.Models;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Buddhabrot.Persistence.Contexts
{
    /// <summary>
    /// Buddhabrot database context.
    /// </summary>
    public class BuddhabrotContext : DbContext, IBuddhabrotContext
    {
        /// <summary>
        /// Instantiates a <see cref="BuddhabrotContext"/>.
        /// </summary>
        /// <param name="options"><see cref="DbContextOptions{TContext}"/>.</param>
        public BuddhabrotContext(DbContextOptions<BuddhabrotContext> options) : base(options)
        {

        }

        /// <summary>
        /// Buddhabrot plots.
        /// </summary>
        public DbSet<BuddhabrotPlot> BuddhabrotPlots { get; set; }

		/// <summary>
		/// Saves all changes made in this context to the database.
		/// </summary>
		/// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
		/// <returns>A task that represents the asynchronous save operation.
        /// The task result contains the number of state entries written to the database.
        /// </returns>
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BuddhabrotPlot>().OwnsOne(
				plot => plot.Parameters, builder => builder.ToJson());

			base.OnModelCreating(modelBuilder);
		}
	}
}

using GameOfLife.Data.Data.Abstractions;
using GameOfLife.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Data.Data
{
    public class GameOfLifeDBContext : DbContext, IGameOfLifeDBContext
    {
        public GameOfLifeDBContext(DbContextOptions dbContextOptions)
           : base(dbContextOptions)
        {
            Database.Migrate();
        }

        public DbSet<Board> Boards { get; set; }

        public new async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Board>(options =>
            {
                options.ToTable(nameof(Board));

                options.HasKey(x => x.Id);

                options.Property(x => x.AliveCellsJson)
                    .HasColumnType("jsonb");
            });
        }
    }
}
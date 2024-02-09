using GameOfLife.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Data.Data.Abstractions
{
    public interface IGameOfLifeDBContext
    {
        DbSet<Board> Boards { get; set; }

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}

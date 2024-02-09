using GameOfLife.Data.Models;

namespace GameOfLife.Data.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Board> BoardRepository { get; } 

        public Task SaveChangesAsync();
    }
}

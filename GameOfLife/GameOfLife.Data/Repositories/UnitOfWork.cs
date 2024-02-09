using GameOfLife.Data.Data;
using GameOfLife.Data.Models;
using GameOfLife.Data.Repositories.Abstractions;

namespace GameOfLife.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GameOfLifeDBContext _dbContext;
        private bool _disposed = false;

        private GenericRepository<Board> _boardRepository;

        public UnitOfWork(GameOfLifeDBContext context)
        {
            _dbContext = context;
        }

        public IGenericRepository<Board> BoardRepository
        {
            get
            {
                _boardRepository ??= new GenericRepository<Board>(_dbContext);
                return _boardRepository;
            }
        }

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
    }
}

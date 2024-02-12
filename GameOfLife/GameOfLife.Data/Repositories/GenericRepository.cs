using GameOfLife.Data.Data;
using GameOfLife.Data.Models;
using GameOfLife.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : ModelBase
    {
        protected readonly GameOfLifeDBContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GameOfLifeDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            return entity!;
        }

        public async Task InsertAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);

        public void Update(TEntity entityToUpdate)
        {
            _dbSet.Update(entityToUpdate);
        }
    }
}

namespace GameOfLife.Data.Repositories.Abstractions
{
    public interface IGenericRepository<TEntity>
    {
        public Task<TEntity> GetByIdAsync(Guid id);

        public Task InsertAsync(TEntity entity);

        void Update(TEntity entityToUpdate);
    }
}

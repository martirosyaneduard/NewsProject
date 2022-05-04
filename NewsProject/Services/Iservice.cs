namespace NewsProject.Services
{
        public interface IService<TEntity, TEntityDto>
        {
            IAsyncEnumerable<TEntity> GetAll();
            Task<TEntity> GetById(int id);
            Task Update(TEntityDto entity, int id);
            Task<TEntity> Add(TEntityDto entity);
            Task Delete(int id);
        }
}

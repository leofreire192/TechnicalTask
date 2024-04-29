using FluentResults;
using Microsoft.AspNetCore.JsonPatch;

namespace CustomersAPI.Repositories
{
    public interface ICustomerRepository<TEntity> where TEntity : class
    {
        Task<Result<TEntity>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<TEntity>>> GetAllAsync();
        Task<Result<TEntity>> AddAsync(TEntity entity);
        Task<Result<TEntity>> UpdateAsync(Guid id, TEntity updatedEntity);

        Task<Result<TEntity>> PatchAsync(Guid id, JsonPatchDocument<TEntity> customerModel);

        Task<Result<bool>> DeleteAsync(Guid id);
    }
}

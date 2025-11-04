using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.DAO.Interfaces
{
    public interface IGenericDAO<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        Task SaveAsync();
    }
}


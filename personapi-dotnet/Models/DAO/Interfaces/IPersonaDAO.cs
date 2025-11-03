using System.Collections.Generic;
using System.Threading.Tasks;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.DAO.Interfaces
{
    public interface IPersonaDAO : IGenericDAO<Persona>
    {
        Task<IEnumerable<Persona>> GetByGeneroAsync(string genero);
        Task<IEnumerable<Persona>> GetAllAsync();
        Task<Persona?> GetByIdAsync(object id);
        Task AddAsync(Persona entity);
        Task UpdateAsync(Persona entity);
        Task DeleteAsync(object id);
        Task SaveAsync();
        
    }
}




using System.Collections.Generic;
using System.Threading.Tasks;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.DAO.Interfaces
{
    public interface IEstudioDAO : IGenericDAO<Estudio>
    {
        Task<IEnumerable<Estudio>> GetByPersonaAsync(int ccPer);
        Task<IEnumerable<Estudio>> GetByProfesionAsync(int idProf);
        Task<IEnumerable<Estudio>> GetAllAsync();
        Task<Estudio?> GetByIdAsync(object id);
        Task AddAsync(Estudio entity);
        Task UpdateAsync(Estudio entity);
        Task DeleteAsync(object id);
        Task SaveAsync();
    }
}



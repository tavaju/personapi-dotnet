using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.DAO.Interfaces
{
    public interface IProfesionDAO : IGenericDAO<Profesion>
    {
        Task<IEnumerable<Profesion>> GetAllAsync();
        Task<Profesion?> GetByIdAsync(object id);
        Task AddAsync(Profesion entity);
        Task UpdateAsync(Profesion entity);
        Task DeleteAsync(object id);
        Task SaveAsync();
    }
}



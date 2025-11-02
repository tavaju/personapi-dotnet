using System.Collections.Generic;
using System.Threading.Tasks;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.DAO.Interfaces
{
    public interface IPersonaDAO : IGenericDAO<Persona>
    {
        // Si quieres, puedes agregar métodos específicos
        Task<IEnumerable<Persona>> GetByGeneroAsync(string genero);
    }
}

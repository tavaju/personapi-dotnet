using System.Collections.Generic;
using System.Threading.Tasks;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.DAO.Interfaces
{
    public interface ITelefonoDAO : IGenericDAO<Telefono>
    {
        Task<IEnumerable<Telefono>> GetByOperadorAsync(string oper);
        Task<IEnumerable<Telefono>> GetByDuenioAsync(int duenio);
    }
}

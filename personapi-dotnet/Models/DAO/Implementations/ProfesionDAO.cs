using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.DAO.Interfaces;

namespace personapi_dotnet.Models.DAO.Implementations
{
    public class ProfesionDAO : IProfesionDAO
    {
        private readonly PersonaDbContext _context;

        public ProfesionDAO(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesion>> GetAllAsync()
        {
            return await _context.Profesions.ToListAsync();
        }

        public async Task<Profesion?> GetByIdAsync(object id)
        {
            return await _context.Profesions.FindAsync(id);
        }

        public async Task AddAsync(Profesion entity)
        {
            await _context.Profesions.AddAsync(entity);
        }

        public async Task UpdateAsync(Profesion entity)
        {
            _context.Profesions.Update(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var profesion = await GetByIdAsync(id);
            if (profesion != null)
                _context.Profesions.Remove(profesion);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

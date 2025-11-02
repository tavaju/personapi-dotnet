using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.DAO.Interfaces;

namespace personapi_dotnet.Models.DAO.Implementations
{
    public class PersonaDAO : IPersonaDAO
    {
        private readonly PersonaDbContext _context;

        public PersonaDAO(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas.ToListAsync();
        }

        public async Task<Persona?> GetByIdAsync(object id)
        {
            return await _context.Personas.FindAsync(id);
        }

        public async Task AddAsync(Persona entity)
        {
            await _context.Personas.AddAsync(entity);
        }

        public async Task UpdateAsync(Persona entity)
        {
            _context.Personas.Update(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var persona = await GetByIdAsync(id);
            if (persona != null)
                _context.Personas.Remove(persona);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Persona>> GetByGeneroAsync(string genero)
        {
            return await _context.Personas
                .Where(p => p.Genero == genero)
                .ToListAsync();
        }
    }
}

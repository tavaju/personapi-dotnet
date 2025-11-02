using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.DAO.Interfaces;

namespace personapi_dotnet.Models.DAO.Implementations
{
    public class TelefonoDAO : ITelefonoDAO
    {
        private readonly PersonaDbContext _context;

        public TelefonoDAO(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Telefono>> GetAllAsync()
        {
            return await _context.Telefonos.ToListAsync();
        }

        public async Task<Telefono?> GetByIdAsync(object id)
        {
            // Telefono tiene clave primaria Num (string)
            if (id is string num)
            {
                return await _context.Telefonos.FindAsync(num);
            }
            return await _context.Telefonos.FindAsync(id);
        }

        public async Task AddAsync(Telefono entity)
        {
            await _context.Telefonos.AddAsync(entity);
        }

        public async Task UpdateAsync(Telefono entity)
        {
            _context.Telefonos.Update(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var telefono = await GetByIdAsync(id);
            if (telefono != null)
                _context.Telefonos.Remove(telefono);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Telefono>> GetByOperadorAsync(string oper)
        {
            return await _context.Telefonos
                .Where(t => t.Oper == oper)
                .ToListAsync();
        }

        public async Task<IEnumerable<Telefono>> GetByDuenioAsync(int duenio)
        {
            return await _context.Telefonos
                .Where(t => t.Duenio == duenio)
                .ToListAsync();
        }
    }
}

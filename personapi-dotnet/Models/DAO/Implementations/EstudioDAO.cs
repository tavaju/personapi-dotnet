using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.DAO.Interfaces;

namespace personapi_dotnet.Models.DAO.Implementations
{
    public class EstudioDAO : IEstudioDAO
    {
        private readonly PersonaDbContext _context;

        public EstudioDAO(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estudio>> GetAllAsync()
        {
            return await _context.Estudios.ToListAsync();
        }

        public async Task<Estudio?> GetByIdAsync(object id)
        {
            // Estudio tiene clave compuesta (IdProf, CcPer)
            // El id debe ser un objeto con ambos valores, como (int idProf, int ccPer) o un ValueTuple
            if (id is ValueTuple<int, int> tuple)
            {
                return await _context.Estudios
                    .FirstOrDefaultAsync(e => e.IdProf == tuple.Item1 && e.CcPer == tuple.Item2);
            }
            return null;
        }

        public async Task AddAsync(Estudio entity)
        {
            await _context.Estudios.AddAsync(entity);
        }

        public async Task UpdateAsync(Estudio entity)
        {
            _context.Estudios.Update(entity);
        }

        public async Task DeleteAsync(object id)
        {
            // Estudio tiene clave compuesta (IdProf, CcPer)
            if (id is ValueTuple<int, int> tuple)
            {
                var estudio = await _context.Estudios
                    .FirstOrDefaultAsync(e => e.IdProf == tuple.Item1 && e.CcPer == tuple.Item2);
                if (estudio != null)
                    _context.Estudios.Remove(estudio);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Estudio>> GetByPersonaAsync(int ccPer)
        {
            return await _context.Estudios
                .Where(e => e.CcPer == ccPer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Estudio>> GetByProfesionAsync(int idProf)
        {
            return await _context.Estudios
                .Where(e => e.IdProf == idProf)
                .ToListAsync();
        }
    }
}

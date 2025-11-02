using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.DAO.Interfaces;

namespace personapi_dotnet.Controllers
{
    public class EstudiosController : Controller
    {
        private readonly IEstudioDAO _estudioDAO;
        private readonly IPersonaDAO _personaDAO;
        private readonly IProfesionDAO _profesionDAO;

        public EstudiosController(IEstudioDAO estudioDAO, IPersonaDAO personaDAO, IProfesionDAO profesionDAO)
        {
            _estudioDAO = estudioDAO;
            _personaDAO = personaDAO;
            _profesionDAO = profesionDAO;
        }

        // GET: Estudios
        public async Task<IActionResult> Index()
        {
            var estudios = await _estudioDAO.GetAllAsync();
            var personas = await _personaDAO.GetAllAsync();
            var profesiones = await _profesionDAO.GetAllAsync();
            
            // Cargar las navegaciones manualmente
            foreach (var estudio in estudios)
            {
                estudio.CcPerNavigation = personas.FirstOrDefault(p => p.Cc == estudio.CcPer);
                estudio.IdProfNavigation = profesiones.FirstOrDefault(pr => pr.Id == estudio.IdProf);
            }
            
            return View(estudios);
        }

        // GET: Estudios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscar por IdProf usando el método del DAO
            var estudios = await _estudioDAO.GetByProfesionAsync(id.Value);
            var estudio = estudios.FirstOrDefault();
            
            if (estudio == null)
            {
                return NotFound();
            }

            // Cargar las navegaciones manualmente
            var personas = await _personaDAO.GetAllAsync();
            var profesiones = await _profesionDAO.GetAllAsync();
            estudio.CcPerNavigation = personas.FirstOrDefault(p => p.Cc == estudio.CcPer);
            estudio.IdProfNavigation = profesiones.FirstOrDefault(pr => pr.Id == estudio.IdProf);

            return View(estudio);
        }

        // GET: Estudios/Create
        public async Task<IActionResult> Create()
        {
            var personas = await _personaDAO.GetAllAsync();
            var profesiones = await _profesionDAO.GetAllAsync();
            ViewData["CcPer"] = new SelectList(personas, "Cc", "Cc");
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Id");
            return View();
        }

        // POST: Estudios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            if (ModelState.IsValid)
            {
                await _estudioDAO.AddAsync(estudio);
                await _estudioDAO.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            var personas = await _personaDAO.GetAllAsync();
            var profesiones = await _profesionDAO.GetAllAsync();
            ViewData["CcPer"] = new SelectList(personas, "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Id", estudio.IdProf);
            return View(estudio);
        }

        // GET: Estudios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscar por IdProf usando el método del DAO
            var estudios = await _estudioDAO.GetByProfesionAsync(id.Value);
            var estudio = estudios.FirstOrDefault();
            
            if (estudio == null)
            {
                return NotFound();
            }
            
            var personas = await _personaDAO.GetAllAsync();
            var profesiones = await _profesionDAO.GetAllAsync();
            ViewData["CcPer"] = new SelectList(personas, "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Id", estudio.IdProf);
            return View(estudio);
        }

        // POST: Estudios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            if (id != estudio.IdProf)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _estudioDAO.UpdateAsync(estudio);
                    await _estudioDAO.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var estudios = await _estudioDAO.GetByProfesionAsync(estudio.IdProf);
                    if (!estudios.Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var personas = await _personaDAO.GetAllAsync();
            var profesiones = await _profesionDAO.GetAllAsync();
            ViewData["CcPer"] = new SelectList(personas, "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Id", estudio.IdProf);
            return View(estudio);
        }

        // GET: Estudios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscar por IdProf usando el método del DAO
            var estudios = await _estudioDAO.GetByProfesionAsync(id.Value);
            var estudio = estudios.FirstOrDefault();
            
            if (estudio == null)
            {
                return NotFound();
            }

            // Cargar las navegaciones manualmente
            var personas = await _personaDAO.GetAllAsync();
            var profesiones = await _profesionDAO.GetAllAsync();
            estudio.CcPerNavigation = personas.FirstOrDefault(p => p.Cc == estudio.CcPer);
            estudio.IdProfNavigation = profesiones.FirstOrDefault(pr => pr.Id == estudio.IdProf);

            return View(estudio);
        }

        // POST: Estudios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Buscar por IdProf y eliminar el primero encontrado
            var estudios = await _estudioDAO.GetByProfesionAsync(id);
            var estudio = estudios.FirstOrDefault();
            
            if (estudio != null)
            {
                var key = (estudio.IdProf, estudio.CcPer);
                await _estudioDAO.DeleteAsync(key);
                await _estudioDAO.SaveAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool EstudioExists(int id)
        {
            var estudios = _estudioDAO.GetByProfesionAsync(id).Result;
            return estudios.Any();
        }
    }
}

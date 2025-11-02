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
    public class ProfesionsController : Controller
    {
        private readonly IProfesionDAO _profesionDAO;

        public ProfesionsController(IProfesionDAO profesionDAO)
        {
            _profesionDAO = profesionDAO;
        }

        // GET: Profesions
        public async Task<IActionResult> Index()
        {
            return View(await _profesionDAO.GetAllAsync());
        }

        // GET: Profesions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesion = await _profesionDAO.GetByIdAsync(id.Value);
            if (profesion == null)
            {
                return NotFound();
            }

            return View(profesion);
        }

        // GET: Profesions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profesions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Des")] Profesion profesion)
        {
            // Remover errores de validación para propiedades de navegación
            ModelState.Remove("Estudios");
            ModelState.Remove("Id"); // El Id se genera automáticamente

            if (ModelState.IsValid)
            {
                try
                {
                    await _profesionDAO.AddAsync(profesion);
                    await _profesionDAO.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Capturar errores de base de datos y mostrar detalles en desarrollo
                    var errorMessage = "Ocurrió un error al guardar la profesión. Por favor, intente nuevamente.";
                    
                    // En desarrollo, mostrar más detalles del error
                    if (ex.InnerException != null)
                    {
                        errorMessage += $" Error: {ex.InnerException.Message}";
                    }
                    
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }
            return View(profesion);
        }

        // GET: Profesions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesion = await _profesionDAO.GetByIdAsync(id.Value);
            if (profesion == null)
            {
                return NotFound();
            }
            return View(profesion);
        }

        // POST: Profesions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Des")] Profesion profesion)
        {
            if (id != profesion.Id)
            {
                return NotFound();
            }

            // Remover errores de validación para propiedades de navegación
            ModelState.Remove("Estudios");

            if (ModelState.IsValid)
            {
                try
                {
                    // Cargar la profesión existente desde la base de datos
                    var profesionExistente = await _profesionDAO.GetByIdAsync(profesion.Id);
                    
                    if (profesionExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar solo los campos editables
                    profesionExistente.Nom = profesion.Nom;
                    profesionExistente.Des = profesion.Des;

                    await _profesionDAO.UpdateAsync(profesionExistente);
                    await _profesionDAO.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _profesionDAO.GetByIdAsync(profesion.Id);
                    if (exists == null)
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
            return View(profesion);
        }

        // GET: Profesions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesion = await _profesionDAO.GetByIdAsync(id.Value);
            if (profesion == null)
            {
                return NotFound();
            }

            return View(profesion);
        }

        // POST: Profesions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _profesionDAO.DeleteAsync(id);
            await _profesionDAO.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfesionExists(int id)
        {
            var profesion = _profesionDAO.GetByIdAsync(id).Result;
            return profesion != null;
        }
    }
}

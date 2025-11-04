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
    public class PersonasController : Controller
    {
        private readonly IPersonaDAO _personaDAO;

        public PersonasController(IPersonaDAO personaDAO)
        {
            _personaDAO = personaDAO;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            return View(await _personaDAO.GetAllAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personaDAO.GetByIdAsync(id.Value);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            // Remover errores de validación para propiedades de navegación
            ModelState.Remove("Estudios");
            ModelState.Remove("Telefonos");

            // Verificar si ya existe una persona con la misma cédula
            var personaExistente = await _personaDAO.GetByIdAsync(persona.Cc);
            
            if (personaExistente != null)
            {
                ModelState.AddModelError("Cc", "Ya existe una persona con esta cédula.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _personaDAO.AddAsync(persona);
                    await _personaDAO.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Capturar errores de clave duplicada u otros errores de base de datos
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("PRIMARY KEY"))
                    {
                        ModelState.AddModelError("Cc", "Ya existe una persona con esta cédula.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar la persona. Por favor, intente nuevamente.");
                    }
                }
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personaDAO.GetByIdAsync(id.Value);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            if (id != persona.Cc)
            {
                return NotFound();
            }

            // Remover errores de validación para propiedades de navegación
            ModelState.Remove("Estudios");
            ModelState.Remove("Telefonos");

            if (ModelState.IsValid)
            {
                try
                {
                    // Cargar la persona existente desde la base de datos
                    var personaExistente = await _personaDAO.GetByIdAsync(persona.Cc);
                    
                    if (personaExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar solo los campos editables
                    personaExistente.Nombre = persona.Nombre;
                    personaExistente.Apellido = persona.Apellido;
                    personaExistente.Genero = persona.Genero;
                    personaExistente.Edad = persona.Edad;

                    await _personaDAO.UpdateAsync(personaExistente);
                    await _personaDAO.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _personaDAO.GetByIdAsync(persona.Cc);
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
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personaDAO.GetByIdAsync(id.Value);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _personaDAO.DeleteAsync(id);
            await _personaDAO.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
            var persona = _personaDAO.GetByIdAsync(id).Result;
            return persona != null;
        }
    }
}

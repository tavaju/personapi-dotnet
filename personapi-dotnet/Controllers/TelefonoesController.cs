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
    public class TelefonoesController : Controller
    {
        private readonly ITelefonoDAO _telefonoDAO;
        private readonly IPersonaDAO _personaDAO;

        public TelefonoesController(ITelefonoDAO telefonoDAO, IPersonaDAO personaDAO)
        {
            _telefonoDAO = telefonoDAO;
            _personaDAO = personaDAO;
        }

        // GET: Telefonoes
        public async Task<IActionResult> Index()
        {
            var telefonos = await _telefonoDAO.GetAllAsync();
            var personas = await _personaDAO.GetAllAsync();
            
            // Cargar las navegaciones manualmente
            foreach (var telefono in telefonos)
            {
                telefono.DuenioNavigation = personas.FirstOrDefault(p => p.Cc == telefono.Duenio);
            }
            
            return View(telefonos);
        }

        // GET: Telefonoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _telefonoDAO.GetByIdAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }

            // Cargar la navegación manualmente
            var personas = await _personaDAO.GetAllAsync();
            telefono.DuenioNavigation = personas.FirstOrDefault(p => p.Cc == telefono.Duenio);

            return View(telefono);
        }

        // GET: Telefonoes/Create
        public async Task<IActionResult> Create()
        {
            var personas = await _personaDAO.GetAllAsync();
            ViewData["Duenio"] = new SelectList(personas, "Cc", "Cc");
            return View();
        }

        // POST: Telefonoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (ModelState.IsValid)
            {
                await _telefonoDAO.AddAsync(telefono);
                await _telefonoDAO.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            var personas = await _personaDAO.GetAllAsync();
            ViewData["Duenio"] = new SelectList(personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        // GET: Telefonoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _telefonoDAO.GetByIdAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }
            var personas = await _personaDAO.GetAllAsync();
            ViewData["Duenio"] = new SelectList(personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        // POST: Telefonoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (id != telefono.Num)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _telefonoDAO.UpdateAsync(telefono);
                    await _telefonoDAO.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _telefonoDAO.GetByIdAsync(telefono.Num);
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
            var personas = await _personaDAO.GetAllAsync();
            ViewData["Duenio"] = new SelectList(personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        // GET: Telefonoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _telefonoDAO.GetByIdAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }

            // Cargar la navegación manualmente
            var personas = await _personaDAO.GetAllAsync();
            telefono.DuenioNavigation = personas.FirstOrDefault(p => p.Cc == telefono.Duenio);

            return View(telefono);
        }

        // POST: Telefonoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _telefonoDAO.DeleteAsync(id);
            await _telefonoDAO.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelefonoExists(string id)
        {
            var telefono = _telefonoDAO.GetByIdAsync(id).Result;
            return telefono != null;
        }
    }
}

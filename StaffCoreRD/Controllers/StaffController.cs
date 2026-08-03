using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffCoreRD.Data;
using StaffCoreRD.Models;

namespace StaffCoreRD.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        private readonly StaffDbContext _context;

        public StaffController(StaffDbContext context)
        {
            _context = context;
        }

        // ---------- INDEX ----------
        // GET: /Staff
        public async Task<IActionResult> Index()
        {
            var personal = await _context.Personal
                .Where(s => s.Activo)
                .OrderBy(s => s.Nombre)
                .ToListAsync();

            return View(personal);
        }

        // ---------- CREATE ----------
        [Authorize(Roles = "Administrador,RRHH")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Staff());
        }

        [Authorize(Roles = "Administrador,RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return View(staff);
            }

            _context.Add(staff);
            await _context.SaveChangesAsync();

            TempData["Exito"] = "Empleado creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // ---------- EDIT ----------
        [Authorize(Roles = "Administrador,RRHH")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Personal.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [Authorize(Roles = "Administrador,RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(staff);
            }

            _context.Update(staff);
            await _context.SaveChangesAsync();

            TempData["Exito"] = "Empleado actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // ---------- DELETE ----------
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Personal.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Personal.FindAsync(id);
            if (staff != null)
            {
                _context.Personal.Remove(staff);
                await _context.SaveChangesAsync();
            }

            TempData["Exito"] = "Empleado eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
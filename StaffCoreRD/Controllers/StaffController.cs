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
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using BibliotecaSemana4.Data;
using BibliotecaSemana4.Models;

namespace BibliotecaSemana4.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Prestamos
        public async Task<IActionResult> Index()
        {
            return View(await ListarPrestamos());
        }

        [HttpGet]
        public async Task<IActionResult> Todos()
        {
            return PartialView("_FilasPrestamos", await ListarPrestamos());
        }

        private Task<List<Prestamo>> ListarPrestamos()
        {
            return _context.Prestamos.Include(prestamo => prestamo.Libro).Include(prestamo => prestamo.Lector)
                .OrderByDescending(prestamo => prestamo.FechaPrestamo).ThenByDescending(prestamo => prestamo.Id)
                .ToListAsync();
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Lector)
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamos/Create
        public async Task<IActionResult> Create()
        {
            await CargarListas();
            return View();
        }

        // POST: Prestamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibroId,LectorId,FechaPrestamo,FechaDevolucion")] Prestamo prestamo)
        {
            if (ModelState.IsValid && await RelacionesValidas(prestamo))
            {
                try
                {
                    _context.Add(prestamo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex) when (ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.NoReferencedRow2 })
                {
                    ModelState.AddModelError(string.Empty, "Una de las opciones seleccionadas ya no existe.");
                }
            }
            await CargarListas();
            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            await CargarListas();
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LibroId,LectorId,FechaPrestamo,FechaDevolucion")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && await RelacionesValidas(prestamo))
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                catch (DbUpdateException ex) when (ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.NoReferencedRow2 })
                {
                    ModelState.AddModelError(string.Empty, "Una de las opciones seleccionadas ya no existe.");
                }
            }
            await CargarListas();
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Lector)
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            _context.Prestamos.Remove(prestamo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            TempData["Mensaje"] = "El registro se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListas()
        {
            var libros = await _context.Libros.OrderBy(libro => libro.Titulo)
                .Select(libro => new { libro.Id, Texto = libro.Titulo }).ToListAsync();
            ViewData["LibroId"] = new SelectList(libros, "Id", "Texto");

            var lectores = await _context.Lectores.OrderBy(lector => lector.Apellido).ThenBy(lector => lector.Nombre)
                .Select(lector => new { lector.Id, Texto = lector.Nombre + " " + lector.Apellido }).ToListAsync();
            ViewData["LectorId"] = new SelectList(lectores, "Id", "Texto");
        }

        private async Task<bool> RelacionesValidas(Prestamo prestamo)
        {
            if (!await _context.Libros.AnyAsync(libro => libro.Id == prestamo.LibroId))
            {
                ModelState.AddModelError(nameof(Prestamo.LibroId), "El libro seleccionado no existe.");
            }

            if (!await _context.Lectores.AnyAsync(lector => lector.Id == prestamo.LectorId))
            {
                ModelState.AddModelError(nameof(Prestamo.LectorId), "El lector seleccionado no existe.");
            }

            return ModelState.IsValid;
        }
    }
}

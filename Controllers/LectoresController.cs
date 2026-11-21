using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using BibliotecaSemana4.Data;
using BibliotecaSemana4.Models;

namespace BibliotecaSemana4.Controllers
{
    public class LectoresController : Controller
    {
        private readonly BibliotecaContext _context;

        public LectoresController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Lectores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lectores.OrderBy(lector => lector.Apellido).ThenBy(lector => lector.Nombre).ToListAsync());
        }

        // GET: Lectores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lector = await _context.Lectores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lector == null)
            {
                return NotFound();
            }

            return View(lector);
        }

        // GET: Lectores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lectores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Correo,Telefono")] Lector lector)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lector);
        }

        // GET: Lectores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lector = await _context.Lectores.FindAsync(id);
            if (lector == null)
            {
                return NotFound();
            }
            return View(lector);
        }

        // POST: Lectores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Correo,Telefono")] Lector lector)
        {
            if (id != lector.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lector);
        }

        // GET: Lectores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lector = await _context.Lectores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lector == null)
            {
                return NotFound();
            }

            return View(lector);
        }

        // POST: Lectores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lector = await _context.Lectores.FindAsync(id);
            if (lector == null)
            {
                return NotFound();
            }

            _context.Lectores.Remove(lector);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.RowIsReferenced2 })
            {
                TempData["Error"] = "No se puede eliminar el lector porque tiene préstamos asociados.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Mensaje"] = "El registro se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}

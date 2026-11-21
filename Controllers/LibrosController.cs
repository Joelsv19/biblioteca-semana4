using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using BibliotecaSemana4.Data;
using BibliotecaSemana4.Models;

namespace BibliotecaSemana4.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros.Include(libro => libro.Autor).Include(libro => libro.Categoria)
                .OrderBy(libro => libro.Titulo).ToListAsync();
            return View(libros);
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public async Task<IActionResult> Create()
        {
            await CargarListas();
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,AutorId,CategoriaId,AnioPublicacion")] Libro libro)
        {
            if (ModelState.IsValid && await RelacionesValidas(libro))
            {
                try
                {
                    _context.Add(libro);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex) when (ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.NoReferencedRow2 })
                {
                    ModelState.AddModelError(string.Empty, "Una de las opciones seleccionadas ya no existe.");
                }
            }
            await CargarListas();
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            await CargarListas();
            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,AutorId,CategoriaId,AnioPublicacion")] Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && await RelacionesValidas(libro))
            {
                try
                {
                    _context.Update(libro);
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
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
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
                TempData["Error"] = "No se puede eliminar el libro porque tiene préstamos asociados.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Mensaje"] = "El registro se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListas()
        {
            var autores = await _context.Autores.OrderBy(autor => autor.Apellido).ThenBy(autor => autor.Nombre)
                .Select(autor => new { autor.Id, Texto = autor.Nombre + " " + autor.Apellido }).ToListAsync();
            ViewData["AutorId"] = new SelectList(autores, "Id", "Texto");

            var categorias = await _context.Categorias.OrderBy(categoria => categoria.Nombre)
                .Select(categoria => new { categoria.Id, Texto = categoria.Nombre }).ToListAsync();
            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Texto");
        }

        private async Task<bool> RelacionesValidas(Libro libro)
        {
            if (!await _context.Autores.AnyAsync(autor => autor.Id == libro.AutorId))
            {
                ModelState.AddModelError(nameof(Libro.AutorId), "El autor seleccionado no existe.");
            }

            if (!await _context.Categorias.AnyAsync(categoria => categoria.Id == libro.CategoriaId))
            {
                ModelState.AddModelError(nameof(Libro.CategoriaId), "La categoría seleccionada no existe.");
            }

            return ModelState.IsValid;
        }
    }
}

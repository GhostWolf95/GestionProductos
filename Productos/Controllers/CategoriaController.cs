using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Productos.Interfaces;
using Productos.Models;
using System.Threading.Tasks;

namespace Productos.Controllers
{
    public class CategoriaController : Controller
    {
        ICategoriaRepository _categoriaRepository;
        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaRepository.GetCategorias();
            return View(categorias);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaRepository.GetCategoria(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaRepository.GetCategoria(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaRepository.Guardar(categoria);
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategoria(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var categoria = await _categoriaRepository.GetCategoria(id);
            if (await TryUpdateModelAsync<Categoria>(
                categoria,
                "",
                s => s.Nombre, s => s.Descripcion, s => s.Estado))
            {
                try
                {
                    await _categoriaRepository.Actualizar(categoria);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Error el guardar datos. " +
                        "Por favor intente nuevamente.");
                }
            }
            return View(categoria);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _categoriaRepository.GetCategoria(id);
            if (categoria == null)
            {
                return NotFound("Registro no existente");
            }

            try
            {
                await _categoriaRepository.Eliminar(categoria);
                return Ok("registro eliminado con exito");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error al eliminar el registro, verifique que no se encuentre relacionado al producto.");
            }
        }
    }
}

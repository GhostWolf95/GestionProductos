using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Productos.DTOs;
using Productos.Interfaces;
using Productos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Productos.Controllers
{
    public class ProductoController : Controller
    {
        IProductoRepository _productoRepository;
        ICategoriaRepository _categoriaRepository;
        public ProductoController(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
        }
        public async Task<IActionResult> Index()
        {
            var productos = await _productoRepository.GetProductos();
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _productoRepository.GetProducto(id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categorias = await _categoriaRepository.GetCategoriasActivas();
            List<SelectListItem> item = categorias.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Nombre.ToString(),
                    Value = a.Idcategoria.ToString(),
                    Selected = false
                };
            });
            ViewBag.Idcategoria = item;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idcategoria,Codigo,Nombre,PrecioVenta,Stock,Descripcion,Imagen")] Producto producto)
        {
            var files = Request.Form.Files;
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                producto.Imagen = fileBytes;
                                //string s = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                            }
                        }
                    }
                }


                await _productoRepository.Guardar(producto);
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _productoRepository.GetProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            var categorias = await _categoriaRepository.GetCategorias();
            List<SelectListItem> item = categorias.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Nombre.ToString(),
                    Value = a.Idcategoria.ToString(),
                    Selected = false
                };
            });
            ViewBag.Idcategoria = item;

            return View(producto);

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProducto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var producto = await _productoRepository.GetProducto(id);
            var files = Request.Form.Files;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            producto.Imagen = fileBytes;
                        }
                    }
                }
            }
            if (await TryUpdateModelAsync<Producto>(
                producto,
                "",
                s => s.Idcategoria, s => s.Codigo, s => s.Nombre,  s => s.PrecioVenta, s => s.Stock
                , s => s.Descripcion, s => s.Imagen, s => s.Estado))
            {
                try
                {
                    await _productoRepository.Actualizar(producto);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Error el guardar datos. " +
                        "Por favor intente nuevamente.");
                }
            }
            return View(producto);
        }

        public FileResult GetFileFromBytes(byte[] bytesIn)
        {
            return File(bytesIn, "image/png");
        }

        [HttpGet]
        public async Task<IActionResult> GetImagenProducto(int? id)
        {
            var user = await _productoRepository.GetProducto(id);
            if (user != null && user.Imagen != null)
            {
                FileResult imageUserFile = GetFileFromBytes(user.Imagen);
                return imageUserFile;
            }
            return null;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _productoRepository.GetProducto(id);
            if (producto == null)
            {
                return NotFound("Registro no existente");
            }

            try
            {
                await _productoRepository.Eliminar(producto);
                return Ok("registro eliminado con exito");
            }
            catch (DbUpdateException  ex )
            {
                return StatusCode(500, "Error al eliminar el registro");
            }
        }
    }
}

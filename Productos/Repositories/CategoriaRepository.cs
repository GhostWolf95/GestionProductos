using Microsoft.EntityFrameworkCore;
using Productos.Data;
using Productos.Interfaces;
using Productos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Productos.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly dbproductosContext _context;
        public CategoriaRepository(dbproductosContext context)
        {
            _context = context;
        }

        public async Task Actualizar(Categoria categoria)
        {
            _context.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task<Categoria> GetCategoria(int? id)
        {
            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(m => m.Idcategoria == id);
            return categoria;
        }

        public async Task<List<Categoria>> GetCategorias()
        {
            var categorias = await _context.Categoria.ToListAsync();
            return categorias;
        }

        public async Task Guardar(Categoria categoria)
        {
            _context.Add(categoria);
            await _context.SaveChangesAsync();
        }
        public async Task Eliminar(Categoria categoria)
        {
            _context.Remove(categoria);
            await _context.SaveChangesAsync();
        }

        public Task<List<Categoria>> GetCategoriasActivas()
        {
            var categorias = from categoria in _context.Categoria
                              where categoria.Estado == true
                           select categoria;

            //await _context.Categoria.ToListAsync();
            return categorias.ToListAsync();
        }
    }
}

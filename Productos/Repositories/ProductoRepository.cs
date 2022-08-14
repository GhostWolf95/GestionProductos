using Microsoft.EntityFrameworkCore;
using Productos.Data;
using Productos.Interfaces;
using Productos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Productos.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly dbproductosContext _context;
        public ProductoRepository(dbproductosContext context)
        {
            _context = context;
        }
        public async Task Actualizar(Producto producto)
        {
            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        public async Task<Producto> GetProducto(int? id)
        {
            var producto = await _context.Producto.Include(d => d.IdcategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Idproducto == id);
            return producto;
        }

        public async Task<List<Producto>> GetProductos()
        {
            var productos = await _context.Producto.Include(d => d.IdcategoriaNavigation).ToListAsync();
            return productos;
        }

        public async Task Guardar(Producto producto)
        {
            try
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        public async Task Eliminar(Producto producto)
        {
            _context.Remove(producto);
            await _context.SaveChangesAsync();
        }
    }
}

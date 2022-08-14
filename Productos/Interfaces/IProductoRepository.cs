using Productos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Productos.Interfaces
{
    public interface IProductoRepository
    {
        Task<Producto> GetProducto(int? id);
        Task<List<Producto>> GetProductos();
        Task Guardar(Producto producto);
        Task Actualizar(Producto producto);
        Task Eliminar(Producto producto);
    }
}

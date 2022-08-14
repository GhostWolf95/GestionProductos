using Productos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Productos.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria> GetCategoria(int? id);
        Task<List<Categoria>> GetCategorias();
        Task<List<Categoria>> GetCategoriasActivas();
        Task Guardar(Categoria categoria);
        Task Actualizar(Categoria categoria);
        Task Eliminar(Categoria categoria);
    }
}

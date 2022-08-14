using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Productos.Models
{
    public partial class Producto
    {
        public int Idproducto { get; set; }

        [Required(ErrorMessage = "El campo Categoria es requerido")]
        public int Idcategoria { get; set; }
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Nombre { get; set; }

        [DisplayName("Precio")]
        [Required(ErrorMessage = "El campo Precio es requerido")]
        [DataType(DataType.Currency, ErrorMessage = "El campo Precio no tiene un valor valido")]
        [DisplayFormat(DataFormatString = "{0:#}", ApplyFormatInEditMode = true)]

        public decimal PrecioVenta { get; set; }

        [Required(ErrorMessage = "El campo Stock es requerido")]
        public int Stock { get; set; }
        public string Descripcion { get; set; }
        public byte[] Imagen { get; set; }
        public bool? Estado { get; set; }

        [DisplayName("Categoria")]
        public virtual Categoria IdcategoriaNavigation { get; set; }
    }
}

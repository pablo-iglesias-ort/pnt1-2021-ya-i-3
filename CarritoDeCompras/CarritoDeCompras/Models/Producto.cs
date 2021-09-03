using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Producto
    {

        public Guid Id { get; set; }

        public string nombre { get;  set; }
        
        public string descripcion { get; set; }
       
        public double precioVigente { get; set; }
        
        public string activo { get; set; }
        
        public Categoria categoria { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models

{
    public class Sucursal
    {
        public Guid Id { get; set; }
        public string nombre { get; set; }
        
        public string descripcion { get; set; }
        
        public string telefono { get; set; }
        
        public string email { get; set; }
       
        public List<StockItem> stockItems { get; set; }
    }
}

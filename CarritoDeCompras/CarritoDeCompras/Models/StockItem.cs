using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarritoDeCompras.Models
{
    public class StockItem
    {
        [Key]
        public Guid Id { get; set; }
       
        public Sucursal Sucursal { get; set; }
        
        public Producto Producto { get; set; }
       
        public int Cantidad { get; set; }
    }
}

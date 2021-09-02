using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Carrito
    {
        private string activo;
        private Cliente cliente;
        private List<CarritoItem> items;
        private double subtotal;
    }
}

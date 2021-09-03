using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public DateTime fechaAlta { get; set; }
        public string email { get; set; }
        public string DNI { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public List<Compra> compras { get; set; }
        public List<Carrito> carritos { get; set; }
    }
}


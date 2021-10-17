using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Empleado
    {
        public Guid Id { get; set; }      
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public DateTime fechaAlta { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
    }
}

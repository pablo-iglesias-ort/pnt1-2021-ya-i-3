using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public DateTime fechaAlta { get; set; }
        public string password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Cliente
    {
        private string nombre;
        private string apellido;
        private string fechaAlta;
        private string email;
        private string DNI;
        private string telefono;
        private string direccion;
        private List<Compra> compras;
        private Carrito carrito;
    }
}


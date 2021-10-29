using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Cliente : Usuario
    {
        public override Rol Rol => Rol.Cliente;

         public IEnumerable<Compra> Compras { get; set; }
        public IEnumerable<Carrito> Carritos { get; set; }

       
    }
}


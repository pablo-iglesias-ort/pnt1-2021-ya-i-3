using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarritoDeCompras.Models
{
    public class Empleado : Usuario
    {
        public override Rol Rol => Rol.Empleado;



    }
}

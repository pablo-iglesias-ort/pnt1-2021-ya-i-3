using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarritoDeCompras.Models
{
    public class Empleado
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [MaxLength(30, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {2} caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido")]
        [MaxLength(30, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {2} caracteres")]
        public string apellido { get; set; }

        [MinLength(8, ErrorMessage = "Debe ingresar al menos {1} digitos")]
        public string telefono { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de alta")]
        public DateTime fechaAlta { get; set; }

        [Required(ErrorMessage = "Debe ingresar una direccion")]
        public string direccion { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electrónico")]
        [EmailAddress]
        public string email { get; set; }
    }
}

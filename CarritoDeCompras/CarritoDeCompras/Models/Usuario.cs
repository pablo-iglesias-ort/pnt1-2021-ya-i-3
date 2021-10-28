using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarritoDeCompras.Models
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [MaxLength(30, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {2} caracteres")]
        public string Nombre { get; set; }
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electrónico")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {1} caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de alta")]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        public byte[] Password { get; set; }

        [Required]
        public Rol Rol { get; set; }
    }
}

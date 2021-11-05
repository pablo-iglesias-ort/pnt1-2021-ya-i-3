using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarritoDeCompras.Models
{
    public  abstract class Usuario
    {

       
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [MaxLength(30, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar un Apellido")]
        [MaxLength(30, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {1} caracteres")]
        public string Apellido { get; set; }


        [Required(ErrorMessage = "Debe ingresar su documento")]
        [Range(1000000, 99999999, ErrorMessage = "Rango no válido para un documento")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; }

        [MinLength(8, ErrorMessage = "Debe ingresar al menos {1} digitos")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe ingresar una direccion")]
        public string Direccion { get; set; }


        [Required(ErrorMessage = "Debe ingresar un correo electrónico")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {1} caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage ="La direccción de correo no es válida. Ejemplo: prueba@email.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de alta")]
        [Display(Name = "Fecha de alta")]
        public DateTime FechaAlta { get; set; }

        public byte[] Password { get; set; }


        [Required]
        public abstract Rol Rol { get; }
    }
}

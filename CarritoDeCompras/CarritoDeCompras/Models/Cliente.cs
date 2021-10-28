using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Cliente
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [MaxLength(30, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {2} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido")]
        [MaxLength(30, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {2} caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de alta")]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessage ="Debe ingresar un correo electrónico")]
        [MinLength(3, ErrorMessage ="Debe superar al menos {1} caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar su documento")]
        [Range(1000000, 99999999, ErrorMessage = "Rango no válido para un documento")]
        public string Dni { get; set; }
        
        [MinLength(8, ErrorMessage ="Debe ingresar al menos {1} digitos")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe ingresar una direccion")]
        public string Direccion { get; set; }

       
        public IEnumerable<Compra> Compras { get; set; }
        public IEnumerable<Carrito> Carritos { get; set; }

        [Required]
        [ForeignKey(nameof(Usuario))]
        public Guid UsuarioId { get; set; }

    }
}


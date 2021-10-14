using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Cliente
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [MaxLength(30, ErrorMessage = "No debe superar los 30 caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos 3 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido")]
        [MaxLength(30, ErrorMessage = "No debe superar los 30 caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos 3 caracteres")]
        public string apellido { get; set; }

        [Required]
        public DateTime fechaAlta { get; set; }

        [Required(ErrorMessage ="Debe ingresar un correo electrónico"]
        [MinLength(3, ErrorMessage ="Debe superar al menos 3 caracteres")]
        [RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        public string email { get; set; }

        [Required(ErrorMessage = "Debe ingresar su documento")]
        [Range(1000000, 99999999, ErrorMessage = "Rango no válido para un documento")]
        public string DNI { get; set; }
        
        [MinLength(6, ErrorMessage ="Debe ingresar al menos 6 digitos")]
        public string telefono { get; set; }

        public string direccion { get; set; }
        public List<Compra> compras { get; set; }
        public List<Carrito> carritos { get; set; }
    }
}


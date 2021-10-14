using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Producto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Debe ingresar un nombre a su producto")]
        [MaxLength(500, ErrorMessage = "No debe superar los 500 caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos 3 caracteres")]
        public string nombre { get;  set; }
        
        [Required(ErrorMessage ="Debe ingresar una descripción a su producto")]
        [MinLength(5, ErrorMessage ="Debe ingresar al menos 5 caracteres")]
        [MaxLength(2000, ErrorMessage = "No debe superar los 2000 caracteres")]
        public string descripcion { get; set; }
       
        [Required(ErrorMessage ="Debe ingresar un valor")]
        [Range(0.0, 999999999.99, ErrorMessage ="El valor debe ser mayor a 0 y menor a 99999999.99")]
        public double precioVigente { get; set; }
        
        [Required]
        public string activo { get; set; }
        
        [Required]
        public Categoria categoria { get; set; }
        
    }
}

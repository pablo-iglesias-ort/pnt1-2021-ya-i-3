using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Producto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Debe ingresar un nombre a su producto")]
        [MaxLength(500, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {1} caracteres")]
        public string Nombre { get;  set; }
        
        [Required(ErrorMessage ="Debe ingresar una descripción a su producto")]
        [MinLength(5, ErrorMessage ="Debe ingresar al menos {1} caracteres")]
        [MaxLength(2000, ErrorMessage = "No debe superar los {1} caracteres")]
        public string Descripcion { get; set; }
       
        [Required(ErrorMessage ="Debe ingresar un valor")]
        [Range(0.0, 999999999.99, ErrorMessage ="El valor debe ser mayor a {1} y menor a {2}")]
        public double PrecioVigente { get; set; }
        
        [Required]
        public Boolean Activo { get; set; }

        [Required]
        [ForeignKey(nameof(Categoria))]
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

    }
}

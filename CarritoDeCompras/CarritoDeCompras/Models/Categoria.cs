﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarritoDeCompras.Models
{
    public class Categoria
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [MaxLength(30, ErrorMessage = "No debe superar los {1} caracteres")]
        [MinLength(3, ErrorMessage = "Debe superar al menos {1} caracteres")]
        public string Nombre { get; set; }

        // no tiene porque ser requerido
        public string Descripcion { get; set; }

        
        public IEnumerable<Producto> Productos { get; set; }
    }
}

using CarritoDeCompras.Controllers;
using CarritoDeCompras.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Data
{
    public static class InicializacionDeDatos
    {
        public static Seguridad seguridad = new Seguridad();

        public static void Inicializar(MVC_Entity_FrameworkContext context)
		{
			context.Database.EnsureCreated();


			if (!context.Usuarios.Any())
			{
				var usuarioEmpleado = new Empleado
				{
					Id = Guid.NewGuid(),
					Nombre = "Carlos",
					Apellido = "Carlin",
					Dni = "32123456",
					NombreUsuario = "Carlos Carlin",
					Telefono = "1562458964",
					Direccion = "Corrientes 2547",
					Email = "Carlos@gmail.com",
					FechaAlta = DateTime.Now,
					Password = seguridad.EncriptarPass("Carloscarlin1"),
					
				};
				var usuarioCliente = new Cliente
				{
					Id = Guid.NewGuid(),
					Nombre = "Juan",
					Apellido = "Juanchez",
					Dni = "33222444",
					NombreUsuario = "Juan Juanchez",
					Telefono = "1564785249",
					Direccion = "Nazca 987",
					Email = "Juan@gmail.com",
					FechaAlta = DateTime.Now,
					Password = seguridad.EncriptarPass("Juanjuanchez1"),
					

				};

				context.Usuarios.AddRange(usuarioEmpleado, usuarioCliente);
				context.SaveChanges();
			}

		}

			
		
			

			
	}

}


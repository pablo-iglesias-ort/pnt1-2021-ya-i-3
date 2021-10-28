using Microsoft.EntityFrameworkCore;

namespace CarritoDeCompras.Models
{
    public class MVC_Entity_FrameworkContext : DbContext
    {
        public MVC_Entity_FrameworkContext(DbContextOptions<MVC_Entity_FrameworkContext> opciones) : base(opciones) { 
        }
        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<CarritoItem> CarritoItems { get; set; }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Empleado > Empleados { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }


    }
}
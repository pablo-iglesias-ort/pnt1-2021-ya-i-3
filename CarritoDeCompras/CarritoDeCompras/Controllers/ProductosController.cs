using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarritoDeCompras.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace CarritoDeCompras.Controllers
{
    public class ProductosController : Controller
    {
        private readonly MVC_Entity_FrameworkContext _context;

        public ProductosController(MVC_Entity_FrameworkContext context)
        {
            _context = context;
        }

        // GET: Productos
        [Authorize(Roles = nameof(Rol.Empleado))]
        public async Task<IActionResult> Index()
        {
            var mVC_Entity_FrameworkContext = _context.Productos.Include(p => p.Categoria);
            return View(await mVC_Entity_FrameworkContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/ListaProductos
        [AllowAnonymous]
        public async Task<IActionResult> ListaProductos()
        {
            var mVC_Entity_FrameworkContext = _context.Productos.Include(p => p.Categoria);
            return View(await mVC_Entity_FrameworkContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> VerDetalle(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,PrecioVigente,Activo,CategoriaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.Id = Guid.NewGuid();
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Descripcion,PrecioVigente,Activo,CategoriaId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var producto = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(Guid id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Agregar(Guid productoId)
        {
            var carritoUsuario = await _context.Carritos.FindAsync(Guid.Parse(User.FindFirst("IdCarrito").Value));
            var producto = await _context.Productos.FindAsync(productoId);

            if (carritoUsuario != null)
            {
              
                var itemsEnCarrito = await _context.CarritoItems.FirstOrDefaultAsync(c => c.ProductoId == productoId && c.CarritoId == carritoUsuario.Id);
                if (itemsEnCarrito != null)
                {
                    itemsEnCarrito.Cantidad += 1;
                    itemsEnCarrito.ValorTotal = itemsEnCarrito.ValorUnitario * itemsEnCarrito.Cantidad;
                    _context.Update(itemsEnCarrito);
                    carritoUsuario.Subtotal += itemsEnCarrito.ValorTotal;
                    _context.Update(carritoUsuario);
                    await _context.SaveChangesAsync();
                }
                else { 
                CarritoItem items = new CarritoItem();
                items.Id = Guid.NewGuid();
                items.CarritoId = carritoUsuario.Id;
                items.ProductoId = producto.Id;
                items.Cantidad = 1;
                items.ValorUnitario = producto.PrecioVigente;
                items.ValorTotal = producto.PrecioVigente * items.Cantidad;


                _context.Add(items);
                carritoUsuario.Subtotal += items.ValorTotal;
                _context.Update(carritoUsuario);
                    await _context.SaveChangesAsync();
                }
            } else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(ListaProductos));
        }

    }

  

}

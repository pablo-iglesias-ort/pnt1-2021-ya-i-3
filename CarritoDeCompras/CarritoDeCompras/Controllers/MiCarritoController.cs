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
    [Authorize]
    public class MiCarritoController : Controller
    {
        private readonly MVC_Entity_FrameworkContext _context;

        public MiCarritoController(MVC_Entity_FrameworkContext context)
        {
            _context = context;
        }

        // GET: MiCarrito
        public async Task<IActionResult> Index()
        {
            var mVC_Entity_FrameworkContext = _context.Carritos.Include(c => c.Cliente);
            return View(await mVC_Entity_FrameworkContext.ToListAsync());
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        // GET: MiCarrito/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .Include(c => c.Items)
                .ThenInclude(it => it.Producto)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);
            if (carrito == null)
            {
                carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .Include(c => c.Items).ThenInclude(it => it.Producto)
                .FirstOrDefaultAsync(m => m.Activo == true && m.ClienteId == Guid.Parse(User.FindFirst("IdUsuario").Value));

            }

            return View(carrito);
        }

        // GET: MiCarrito/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
            return View();
        }

        // POST: MiCarrito/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Activo,ClienteId,Subtotal")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                carrito.Id = Guid.NewGuid();
                _context.Add(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        [Authorize(Roles = nameof(Rol.Cliente))]

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Activo,ClienteId,Subtotal")] Carrito carrito)
        {
            if (id != carrito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carrito.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        // GET: MiCarrito/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // POST: MiCarrito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var carrito = await _context.Carritos.FindAsync(id);
            _context.Carritos.Remove(carrito);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        public async Task<IActionResult> Agregar(Guid productoId)
        {
            var carritoUsuario = await ObtenerCarritoActivo();
            var producto = await _context.Productos.FindAsync(productoId);

            if (carritoUsuario != null)
            {

                var itemsEnCarrito = await _context.CarritoItems.FirstOrDefaultAsync(c => c.ProductoId == productoId && c.CarritoId == carritoUsuario.Id);
                if (itemsEnCarrito != null)
                {
                    itemsEnCarrito.Cantidad += 1;
                    itemsEnCarrito.ValorTotal = itemsEnCarrito.ValorUnitario * itemsEnCarrito.Cantidad;
                    _context.Update(itemsEnCarrito);
                    carritoUsuario.Subtotal += itemsEnCarrito.ValorUnitario;
                    _context.Update(carritoUsuario);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    CarritoItem items = new CarritoItem
                    {
                        Id = Guid.NewGuid(),
                        CarritoId = carritoUsuario.Id,
                        ProductoId = producto.Id,
                        Cantidad = 1,
                        ValorUnitario = producto.PrecioVigente
                    };
                    items.ValorTotal = producto.PrecioVigente * items.Cantidad;


                    _context.Add(items);
                    carritoUsuario.Subtotal += items.ValorTotal;
                    _context.Update(carritoUsuario);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Details), new { Id = carritoUsuario.Id });
        }

        public async Task<IActionResult> EliminarProducto(Guid productoId)
        {
            var carritoUsuario = await ObtenerCarritoActivo();


            if (carritoUsuario != null)
            {

                var itemsEnCarrito = await _context.CarritoItems.FirstOrDefaultAsync(p => p.ProductoId == productoId);
                if (itemsEnCarrito != null)
                {
                    var itemsActuales = itemsEnCarrito.Cantidad;
                   
                    itemsEnCarrito.Cantidad = 0;
                    //itemsEnCarrito.ValorTotal -= itemsEnCarrito.ValorUnitario * itemsEnCarrito.Cantidad;
                    _context.Update(itemsEnCarrito);
                    carritoUsuario.Subtotal -= itemsEnCarrito.ValorUnitario * itemsActuales;
                    _context.CarritoItems.Remove(itemsEnCarrito);
                    _context.Update(carritoUsuario);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Details), new { Id = carritoUsuario.Id });


                }
                
            }
            return NotFound();
        }

        public async Task<IActionResult> EditarProducto(Guid productoId, [Bind("Cantidad")] Carrito carrito)
        {
            var carritoUsuario = await ObtenerCarritoActivo();


            if (carritoUsuario != null)
            {

                var itemsEnCarrito = await _context.CarritoItems.FirstOrDefaultAsync(p => p.ProductoId == productoId);
                if (itemsEnCarrito != null)
                {
                    var itemsActuales = itemsEnCarrito.Cantidad;

                    itemsEnCarrito.Cantidad = 0;
                    //itemsEnCarrito.ValorTotal -= itemsEnCarrito.ValorUnitario * itemsEnCarrito.Cantidad;
                    _context.Update(itemsEnCarrito);
                    carritoUsuario.Subtotal -= itemsEnCarrito.ValorUnitario * itemsActuales;
                    _context.CarritoItems.Remove(itemsEnCarrito);
                    _context.Update(carritoUsuario);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Details), new { Id = carritoUsuario.Id });


                }

            }
            return NotFound();
        }


        public async Task<Carrito> ObtenerCarritoActivo()
        {
            return await _context.Carritos.FirstOrDefaultAsync(c => c.ClienteId == Guid.Parse(User.FindFirst("IdUsuario").Value) && c.Activo == true);
        }

        // POST: MiCarrito/Delete/5
        [HttpPost, ActionName("VaciarCarrito")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VaciarCarrito(Guid Id)
        {
            var carrito = await _context.Carritos.FirstOrDefaultAsync(c => c.Id == Id && c.Activo == true);
            if (carrito != null)
            {
                var carritoItems = await _context.CarritoItems.Where(ci => (ci.CarritoId == carrito.Id)).ToListAsync();
                foreach (var n in carritoItems)
                {
                    _context.CarritoItems.Remove(n);
                }
                carrito.Subtotal = 0;
                _context.Carritos.Update(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = carrito.Id });
            }
            return NotFound();
        }
        private bool CarritoExists(Guid id)
        {
            return _context.Carritos.Any(e => e.Id == id);
        }
        /*
                [HttpPost, ActionName("Comprar")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Comprar(Guid id)
                {
                    var carrito = await _context.Carritos.FindAsync(id);
                    if (carrito != null)
                    {
                        carrito.Activo = false;
                        _context.Carritos.Update(carrito);
                        Carrito carritoNuevo = new Carrito();        // TERMINAR CORREGIR ESTE METODO
                        carritoNuevo.Id = Guid.NewGuid();
                        carritoNuevo.ClienteId = Guid.Parse(User.FindFirst("IdUsuario").Value);
                        carritoNuevo.Activo = true;
                        _context.Add(carritoNuevo);


                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), new { id = carrito.Id });
                    }
                    return NotFound();
                }*/





    } 
}

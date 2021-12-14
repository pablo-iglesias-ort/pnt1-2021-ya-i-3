using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarritoDeCompras.Models;
using Microsoft.AspNetCore.Authorization;

namespace CarritoDeCompras.Controllers
{
    public class ComprasController : Controller
    {
        private readonly MVC_Entity_FrameworkContext _context;

        public ComprasController(MVC_Entity_FrameworkContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == Guid.Parse(User.FindFirst("IdUsuario").Value));
            if (usuario.Rol == Rol.Empleado)
            {
                var mVC_Entity_FrameworkContext = _context.Compras.Include(c => c.Carrito).Include(c => c.Cliente).OrderByDescending(c => c.Total).ToListAsync();
                return View(await mVC_Entity_FrameworkContext);
            }
            else if (usuario.Rol == Rol.Cliente)
            {
                var mVC_Entity_FrameworkContext = _context.Compras.Include(c => c.Carrito).Include(c => c.Cliente).Where(c => (c.ClienteId == usuario.Id));
                return View(mVC_Entity_FrameworkContext);
            }
            else
            {
                return NotFound();
            }

        }


        // GET: Compras/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.Carrito)
                .ThenInclude(c=>c.Items)
                .ThenInclude(c=>c.Producto)
                .Include(c => c.Cliente)
                .Include(c => c.Sucursal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        [Authorize(Roles = nameof(Rol.Empleado))]
        public IActionResult Create()
        {
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id");
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", compra.ClienteId);
            return View(compra);
        }

        [Authorize(Roles = nameof(Rol.Empleado))]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", compra.ClienteId);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ClienteId,CarritoId,Total")] Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.Id))
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
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", compra.ClienteId);
            return View(compra);
        }

        [Authorize(Roles = nameof(Rol.Empleado))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.Carrito)
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var compra = await _context.Compras.FindAsync(id);
            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(Guid id)
        {
            return _context.Compras.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comprar(Guid? Id)
        {
                        
            var carrito = await _context.Carritos.FindAsync(Id);
            var items = await _context.CarritoItems.Where(i => i.CarritoId == carrito.Id).ToListAsync();
            if (items.Count()!=0) { 
            var sucursal = await _context.Sucursales.FirstOrDefaultAsync();
            if (carrito != null)
            {
                Compra compra = new Compra
                {
                    Id = Guid.NewGuid(),
                    CarritoId = carrito.Id,
                    ClienteId = Guid.Parse(User.FindFirst("IdUsuario").Value),
                    SucursalId = sucursal.Id,
                    Total = carrito.Subtotal,
                };
                try
                {
                    await Create(compra);
                    carrito.Activo = false;
                    _context.Carritos.Update(carrito);
                    Carrito carritoNuevo = new Carrito
                    {
                        Id = Guid.NewGuid(),
                        ClienteId = Guid.Parse(User.FindFirst("IdUsuario").Value),
                        Activo = true
                    };        // TERMINAR CORREGIR ESTE METODO
                    _context.Add(carritoNuevo);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(FinalizarCompra), new { id = compra.Id });
            }
            return NotFound();
            }
            else
            {
                TempData["Message"] = "Debe agregar al menos un item para comprar";
                return RedirectToAction("Details", "MiCarrito", new { Id});
            }
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        public async Task<IActionResult> FinalizarCompra(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.Include(c => c.Cliente).Include(c => c.Carrito).ThenInclude(c => c.Items).ThenInclude(c => c.Producto).FirstOrDefaultAsync(c => c.Id == id);
            if (compra == null)
            {
                return NotFound();
            }
            ViewBag.MessageOk = TempData["MessageOK"];
            ViewBag.MessageError = TempData["MessageError"];
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Descripcion", compra.SucursalId);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarCompra(Guid id, Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var compraExistente = _context.Compras.Include(c => c.Carrito).ThenInclude(c => c.Items).ThenInclude(c => c.Producto).FirstOrDefault(c => c.Id == id);
                if (hayStock(compra))

                {
                    foreach (var item in compraExistente.Carrito.Items)
                    {
                        var stockItem = itemEnStock(compra.SucursalId, item.ProductoId);
                        stockItem.Cantidad -= item.Cantidad;
                        _context.StockItem.Update(stockItem);
                    }
                    compraExistente.SucursalId = compra.SucursalId;

                    try
                    {
                        _context.Compras.Update(compraExistente);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CompraExists(compra.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", compra.CarritoId);
                    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", compra.ClienteId);
                    TempData["MessageOK"] = "Gracias por su compra";
                    return RedirectToAction(nameof(FinalizarCompra), new { id = compra.Id });
                } else
                {
                    TempData["MessageError"] = "Sin stock. Intente nuevamente más tarde";
                    return RedirectToAction(nameof(FinalizarCompra), new { id = compra.Id });
                }
            }
            else
            {
                return NotFound();
            }
            
        }
   
        private bool hayStock(Compra compra)
        {
            bool resultado=true;
            var items = _context.CarritoItems.Where(i => (i.CarritoId == compra.CarritoId)).AsEnumerable();
            var i = 0;
            var aux = items.Count();

            while (resultado==true && i < aux)
            {
                var stockItem = _context.StockItem.Include(s => s.Sucursal).Include(s => s.Producto).FirstOrDefault(s => (s.SucursalId == compra.SucursalId && s.ProductoId == items.ElementAt(i).ProductoId));
                if (stockItem == null || stockItem.Cantidad < items.ElementAt(i).Cantidad)
                {
                    resultado = false;
                                 }
                else
                {
                    i++;
                }
            }
            return resultado;
        }

        private StockItem itemEnStock(Guid idSucursal, Guid idProducto)
        {
           var resultado =  _context.StockItem.FirstOrDefault(s => (s.SucursalId == idSucursal && s.ProductoId == idProducto));
            return resultado;
        }
    }
}

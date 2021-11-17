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
    [Authorize(Roles = nameof(Rol.Empleado))]
    public class StockItemsController : Controller
    {
        
        private readonly MVC_Entity_FrameworkContext _context;

        public StockItemsController(MVC_Entity_FrameworkContext context)
        {
            _context = context;
        }

        // GET: StockItems
        public async Task<IActionResult> Index()
        {
            var mVC_Entity_FrameworkContext = _context.StockItem.Include(s => s.Producto).Include(s => s.Sucursal);
            return View(await mVC_Entity_FrameworkContext.ToListAsync());
        }

        // GET: StockItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }


        // GET: StockItems/Details/5
        public async Task<IActionResult> DetallePorSucursal(Guid? idSucursal)
        {
            if (idSucursal == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem.Include(s => s.Producto).Include(s => s.Sucursal).Where(s => (s.SucursalId == idSucursal)).ToListAsync();
            
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        [Authorize(Roles = nameof(Rol.Empleado))]
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Email");
            return View();
        }

        // POST: StockItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cantidad,ProductoId,SucursalId")] StockItem stockItem)
        {
            var urlIngreso = TempData["UrlIngreso"] as string;

            if (ModelState.IsValid)
            {
                var itemSucursal = await _context.StockItem.FirstOrDefaultAsync(c => c.ProductoId == stockItem.ProductoId && c.SucursalId == stockItem.SucursalId);
                if (itemSucursal == null)
                {
                    stockItem.Id = Guid.NewGuid();
                    _context.Add(stockItem);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } else
                {
                    ViewBag.ErrorEnLogin = "Ya existe stock cargado para ese producto y esa sucursal";
                    ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
                    ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Email", stockItem.SucursalId);
                    return View();
                }

               
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Email", stockItem.SucursalId);
            return View(stockItem);
        }

        [Authorize(Roles = nameof(Rol.Empleado))]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem.FindAsync(id);
            if (stockItem == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Email", stockItem.SucursalId);
            return View(stockItem);
        }

        // POST: StockItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Cantidad,ProductoId,SucursalId")] StockItem stockItem)
        {
            if (id != stockItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockItemExists(stockItem.Id))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Email", stockItem.SucursalId);
            return View(stockItem);
        }
        /*
        [Authorize(Roles = nameof(Rol.Empleado))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // POST: StockItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var stockItem = await _context.StockItem.FindAsync(id);
            _context.StockItem.Remove(stockItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool StockItemExists(Guid id)
        {
            return _context.StockItem.Any(e => e.Id == id);
        }


         public async Task<IActionResult> Cantidad(Guid? id)
         {
             if (id == null)
             {
                 return NotFound();
             }
             var sucursal = await _context.Sucursales.FirstOrDefaultAsync(m => m.Id == id);

             if(sucursal == null)
             {
                 return NotFound();
             }
             return View(sucursal);
         }
     
    }
}

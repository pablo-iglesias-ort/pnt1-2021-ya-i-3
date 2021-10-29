﻿using System;
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
    [AllowAnonymous]
    public class UsuariosController : Controller

       
    {
        private readonly MVC_Entity_FrameworkContext _context;
         public Seguridad seguridad = new Seguridad();

    public UsuariosController(MVC_Entity_FrameworkContext context)
        {
            _context = context;
        }

        [Authorize(Roles = nameof(Rol.Empleado))]
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        [Authorize]
        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        // GET: Usuarios/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,NombreUsuario,Email,FechaAlta,Password,Rol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Id = Guid.NewGuid();
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Registro()
        {
            return View();
        }



        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Cliente usuario, string pass)
        {
            if (ModelState.IsValid)
            {                
                if(seguridad.ValidarPass(pass))
                {
                    usuario.Id = Guid.NewGuid();
                    usuario.FechaAlta = DateTime.Now;
                    usuario.Password = seguridad.EncriptarPass(pass);
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } 
                else
                {
                    ModelState.AddModelError(nameof(Cliente.Password), "No cumple con los requisitos");
                }
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,NombreUsuario,Email,FechaAlta,Password,Rol")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(Guid id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        //get
        public IActionResult Ingresar(string returnUrl)
        {
            TempData["UrlIngreso"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ingresar(string usuario, string pass)
        {
            // Guardamos la URL a la que debemos redirigir al usuario
            var urlIngreso = TempData["UrlIngreso"] as string;

            // Verificamos que ambos esten informados
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(pass))
            {

                // Verificamos que exista el usuario
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == usuario);
                if (user != null)
                {

                    // Verificamos que coincida la contraseña
                    var contraseña = seguridad.EncriptarPass(pass);
                    if (contraseña.SequenceEqual(user.Password))
                    {

                        // Creamos los Claims (credencial de acceso con informacion del usuario)
                        ClaimsIdentity identidad = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                        // Agregamos a la credencial el nombre de usuario
                        identidad.AddClaim(new Claim(ClaimTypes.Name, usuario));
                        // Agregamos a la credencial el nombre del estudiante/administrador
                        identidad.AddClaim(new Claim(ClaimTypes.GivenName, user.Nombre));
                        // Agregamos a la credencial el Rol
                        identidad.AddClaim(new Claim(ClaimTypes.Role, user.Rol.ToString()));

                        ClaimsPrincipal principal = new ClaimsPrincipal(identidad);

                        // Ejecutamos el Login
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        if (!string.IsNullOrEmpty(urlIngreso))
                        {
                            return Redirect(urlIngreso);
                        }
                        else
                        {
                            // Redirigimos a la pagina principal
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            ViewBag.ErrorEnLogin = "Verifique el usuario y contraseña";
            TempData["UrlIngreso"] = urlIngreso; // Volvemos a enviarla en el TempData para no perderla
            return View();

        }
    }
}
    

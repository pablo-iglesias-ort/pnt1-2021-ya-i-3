using CarritoDeCompras.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarritoDeCompras.Controllers
{
    [AllowAnonymous]
    public class UsuariosController : Controller
    {
        private readonly MVC_Entity_FrameworkContext _context;
        private readonly ISeguridad seguridad = new Seguridad();
        public IActionResult Index()
        {
            return View();
        }

     

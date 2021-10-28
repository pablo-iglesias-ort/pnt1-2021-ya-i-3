using Microsoft.AspNetCore.Mvc;

namespace CarritoDeCompras.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly MVC_Entity_FrameworkContext _context;

        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Data
{
    public class InicializacionDeDatos : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

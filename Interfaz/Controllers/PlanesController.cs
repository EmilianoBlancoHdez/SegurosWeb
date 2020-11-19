using Interfaz.Comunes;
using Interfaz.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interfaz.Controllers
{
    public class PlanesController : Controller
    {
        // GET: Planes
        public ActionResult Index()
        {
            Services service = new Services();
            var response = service.CallGet("https://localhost:44350/api/planes/", 15000);
            var lista = service.Deserialize<List<PlanesViewModel>>(response.Json);
            return View(lista);
        }
    }
}
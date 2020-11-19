using Interfaz.Comunes;
using Interfaz.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interfaz.Controllers
{
    public class CoberturasController : Controller
    {
        // GET: Coberturas
        public ActionResult Index()
        {
            Services service = new Services();
            var response = service.CallGet("https://localhost:44350/api/coberturas/", 15000);
            var lista = service.Deserialize<List<CoberturasViewModel>>(response.Json);
            return View(lista);
        }
    }
}
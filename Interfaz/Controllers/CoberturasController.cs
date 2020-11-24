namespace Interfaz.Controllers
{
    using Interfaz.Comunes;
    using Interfaz.Models.Request;
    using Interfaz.Models.ViewModels;
    using System.Collections.Generic;
    using System.Web.Mvc;

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

        [HttpGet]
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(Coberturas cobertura)
        {
            if (!ModelState.IsValid)
            {
                return View(cobertura);
            }

            Services service = new Services();
            var response = service.CallPost<Coberturas>(cobertura, "https://localhost:44350/api/coberturas/", 15000);

            if (response.ErrorCode != 0)
            {
                cobertura.MensajeError = response.ErrorDescription;
                return View(cobertura);
            }

            var json = service.Deserialize<Interfaz.Models.Request.Response>(response.Json);

            if (json.IdError != 0)
            {
                cobertura.MensajeError = json.MessageError;
                return View(cobertura);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            Services service = new Services();
            var response = service.CallGet($"https://localhost:44350/ObtenerCobertura/{id}", 15000);
            var cobertura = service.Deserialize<CoberturasViewModel>(response.Json);
            return View(cobertura);
        }

        [HttpPost]
        public ActionResult Editar(CoberturasViewModel cobertura)
        {
            if (!ModelState.IsValid)
            {
                return View(cobertura);
            }

            Services service = new Services();
            var response = service.CallPost<CoberturasViewModel>(cobertura, "https://localhost:44350/Coberturas/Editar", 15000);

            if (response.ErrorCode != 0)
            {
                cobertura.MensajeError = response.ErrorDescription;
                return View(cobertura);
            }

            var json = service.Deserialize<Interfaz.Models.Request.Response>(response.Json);

            if (json.IdError != 0)
            {
                cobertura.MensajeError = json.MessageError;
                return View(cobertura);
            }
            return RedirectToAction("Index");
        }
    }
}
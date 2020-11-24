namespace Interfaz.Controllers
{
    using Interfaz.Comunes;
    using Interfaz.Models.Request;
    using Interfaz.Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

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

        [HttpGet]
        public ActionResult Nuevo()
        {
            ViewBag.lista = this.ObtenerCoberturas();
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(Planes planes, int[] Coberturas)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lista = this.ObtenerCoberturas();
                return View(planes);
            }

            planes.Coberturas = Coberturas;
            Services service = new Services();
            var response = service.CallPost<Planes>(planes, "https://localhost:44350/api/planes/", 15000);

            if (response.ErrorCode != 0)
            {
                planes.MensajeError = response.ErrorDescription;
                ViewBag.lista = this.ObtenerCoberturas();
                return View(planes);
            }

            var json = service.Deserialize<Interfaz.Models.Request.Response>(response.Json);

            if (json.IdError != 0)
            {
                planes.MensajeError = json.MessageError;
                ViewBag.lista = this.ObtenerCoberturas();
                return View(planes);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            Services service = new Services();
            var response = service.CallGet($"https://localhost:44350/ObtenerPlan/{id}", 15000);
            var plan = service.Deserialize<PlanesViewModel>(response.Json);
            ViewBag.lista = this.ObtenerCoberturas();
            ViewBag.listaCoberturasPlan = this.ObtenerCoberturasByPlan(id);
            return View(plan);
        }

        [HttpPost]
        public ActionResult Editar(PlanesViewModel plan, int[] Coberturas)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lista = this.ObtenerCoberturas();
                ViewBag.listaCoberturasPlan = this.ObtenerCoberturasByPlan(plan.ID);
                return View(plan);
            }

            plan.Coberturas = Coberturas;
            Services service = new Services();
            var response = service.CallPost<PlanesViewModel>(plan, "https://localhost:44350/Planes/Editar", 15000);

            if (response.ErrorCode != 0)
            {
                plan.MensajeError = response.ErrorDescription;
                ViewBag.lista = this.ObtenerCoberturas();
                ViewBag.listaCoberturasPlan = this.ObtenerCoberturasByPlan(plan.ID);
                return View(plan);
            }

            var json = service.Deserialize<Interfaz.Models.Request.Response>(response.Json);

            if (json.IdError != 0)
            {
                plan.MensajeError = json.MessageError;
                ViewBag.lista = this.ObtenerCoberturas();
                ViewBag.listaCoberturasPlan = this.ObtenerCoberturasByPlan(plan.ID);
                return View(plan);
            }
            return RedirectToAction("Index");
        }

        private List<SelectListItem> ObtenerCoberturas()
        {
            Services service = new Services();
            var response = service.CallGet("https://localhost:44350/api/coberturas/", 15000);
            var lista = service.Deserialize<List<CoberturasViewModel>>(response.Json);

            return lista.Select(x => new SelectListItem()
            {
                Text = x.Descripcion,
                Value = x.ID.ToString()
            }).ToList();
        }

        private List<CoberturasViewModel> ObtenerCoberturasByPlan(int idPlan)
        {
            Services service = new Services();
            var response = service.CallGet($"https://localhost:44350/ObtenerCoberturaByPlan/{idPlan}", 15000);
            var lista = service.Deserialize<List<CoberturasViewModel>>(response.Json);

            return lista;
        }
    }
}
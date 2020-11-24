namespace Interfaz.Controllers
{
    using Interfaz.Comunes;
    using Interfaz.Models.Request;
    using Interfaz.Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            Services service = new Services();
            var response = service.CallGet("https://localhost:44350/api/clientes/", 15000);
            var lista = service.Deserialize<List<ClienteViewModel>>(response.Json);
            return View(lista);
        }

        [HttpGet]
        public ActionResult ListadoCLientes()
        {
            Services service = new Services();
            var response = service.CallGet("https://localhost:44350/ObtenerListadoClientes/", 15000);
            var lista = service.Deserialize<List<ClienteViewModel>>(response.Json);
            return View(lista);
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            ViewBag.lista = this.ObtenerPlanes();
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(Cliente cliente, int[] planes)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.lista = this.ObtenerPlanes();
                return View(cliente);
            }

            cliente.Planes = planes;
            Services service = new Services();
            var response = service.CallPost<Cliente>(cliente, "https://localhost:44350/api/clientes/", 15000);

            if(response.ErrorCode != 0)
            {
                cliente.MensajeError = response.ErrorDescription;
                ViewBag.lista = this.ObtenerPlanes();
                return View(cliente);
            }

            var json = service.Deserialize<Interfaz.Models.Request.Response>(response.Json);

            if(json.IdError != 0)
            {
                cliente.MensajeError = json.MessageError;
                ViewBag.lista = this.ObtenerPlanes();
                return View(cliente);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            Services service = new Services();
            var response = service.CallGet($"https://localhost:44350/ObtenerCliente/{id}", 15000);
            ViewBag.lista = this.ObtenerPlanes();
            ViewBag.listaPlanes = this.ObtenerPlanByCliente(id);
            var cliente = service.Deserialize<ClienteViewModel>(response.Json);
            return View(cliente);
        }

        [HttpPost]
        public ActionResult Editar(ClienteViewModel cliente, int[] Planes)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lista = this.ObtenerPlanes();
                ViewBag.listaPlanes = this.ObtenerPlanByCliente(cliente.ID);
                return View(cliente);
            }

            cliente.Planes = Planes;
            Services service = new Services();
            var response = service.CallPost<ClienteViewModel>(cliente, "https://localhost:44350/Editar", 15000000);

            if (response.ErrorCode != 0)
            {
                cliente.MensajeError = response.ErrorDescription;
                ViewBag.lista = this.ObtenerPlanes();
                ViewBag.listaPlanes = this.ObtenerPlanByCliente(cliente.ID);
                return View(cliente);
            }

            var json = service.Deserialize<Interfaz.Models.Request.Response>(response.Json);

            if (json.IdError != 0)
            {
                cliente.MensajeError = json.MessageError;
                ViewBag.lista = this.ObtenerPlanes();
                ViewBag.listaPlanes = this.ObtenerPlanByCliente(cliente.ID);
                return View(cliente);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Eliminar(int id = 0)
        {
            Services service = new Services();
            var response = service.CallGet($"https://localhost:44350/Eliminar/{id}", 15000);
            return RedirectToAction("Index");
        }

        private List<SelectListItem> ObtenerPlanes()
        {
            Services service = new Services();
            var response = service.CallGet("https://localhost:44350/api/planes/", 15000);
            var lista = service.Deserialize<List<PlanesViewModel>>(response.Json);

            return lista.Select(x => new SelectListItem()
            {
                Text = x.Descripcion,
                Value = x.ID.ToString()
            }).ToList();
        }

        private List<PlanesViewModel> ObtenerPlanByCliente(int idCliente)
        {
            Services service = new Services();
            var response = service.CallGet($"https://localhost:44350/ObtenerPlanByCliente/{idCliente}", 15000);
            var lista = service.Deserialize<List<PlanesViewModel>>(response.Json);

            return lista;
        }
    }
}
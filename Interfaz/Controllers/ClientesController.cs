using Interfaz.Comunes;
using Interfaz.Models.Request;
using Interfaz.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interfaz.Controllers
{
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
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public Response Nuevo(Cliente cliente)
        {
            if(!ModelState.IsValid)
            {
                return new Response() 
                { 
                    IdError = 1,
                    MessageError = "Parámetros incorrectos"
                };
            }

            Services service = new Services();
            var response = service.CallPost<Cliente>(cliente, "https://localhost:44350/api/clientes/", 15000);
            Response respuesta = null;

            if(response.ErrorCode != 0)
            {
                return new Response()
                {
                    IdError = 1,
                    MessageError = "Sucedio un error al agregar cliente"
                };
            }
            else
            {
                respuesta = service.Deserialize<Response>(response.Json);
            }

            return respuesta;
        }
    }
}
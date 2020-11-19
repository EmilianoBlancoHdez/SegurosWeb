namespace Seguros.Controllers
{
    using ABDContexto;
    using Seguros.Models.Request;
    using Seguros.Models.Response;
    using Seguros.Models.ViewModels;
    using Seguros.Repositorio;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class ClientesController : ApiController
    {
        [HttpGet]
        public List<ClientesViewModel> Get()
        {
            return new ConsultarSeguros().ConsultaClientes();
        }

        [HttpPost]
        public async Task<Response> Post([FromBody] Cliente clienteRequest)
        {
            var response = new Response();
            try
            {
                using (var contexto = new Contexto())
                {
                    var clientes = new Clientes();
                    clientes.Nombre = clienteRequest.Nombre;
                    clientes.FechaModificacion = DateTime.Now;
                    contexto.Clientes.Add(clientes);
                    int resultado = await contexto.SaveChangesAsync();

                    if(resultado > 0)
                    {
                        response.IdError = 0;
                    }
                    else
                    {
                        response.IdError = 1;
                        response.MessageError = "Ocurrio un error al crear cliente";
                    }
                }
            }
            catch(Exception error)
            {
                // MANEJAR EL OBJETO DE ERROR
                response.IdError = 2;
                response.MessageError = error.Message;
            }

            return response;
        }
    }
}

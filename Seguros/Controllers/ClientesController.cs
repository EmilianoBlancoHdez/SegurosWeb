namespace Seguros.Controllers
{
    using ABDContexto;
    using Seguros.Models.Request;
    using Seguros.Models.Response;
    using Seguros.Models.ViewModels;
    using Seguros.Repositorio;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class ClientesController : ApiController
    {
        [HttpGet]
        public async Task<List<ClientesViewModel>> Get()
        {
            return await new ConsultarSeguros().ConsultaClientes(Query.CLIENTES);
        }

        [HttpGet]
        [Route("ObtenerListadoClientes")]
        public async Task<List<ClientesViewModel>> ObtenerListadoClientes()
        {
            return await new ConsultarSeguros().ConsultaClientes(Query.LISTADO);
        }

        [HttpGet]
        [Route("ObtenerCliente/{id:int}")]
        public async Task<ClientesViewModel> ObtenerCliente(int id)
        {
            return await new ConsultarSeguros().ObtenerCliente(id);
        }

        [HttpGet]
        [Route("ObtenerPlanByCliente/{idCliente:int}")]
        public async Task<List<PlanesViewModel>> ObtenerPlanByCliente(int idCliente)
        {
            return await new ConsultarPlanes().ObtenerPlanByCliente(idCliente);
        }

        [HttpPost]
        public async Task<Response> Post([FromBody] Cliente clienteRequest)
        {
            clienteRequest.Nombre = Utils.Utilidades.Formato(clienteRequest.Nombre);

            bool continuar = await new Repositorio.ConsultarSeguros().ValidarNombre(clienteRequest.Nombre);

            if (continuar)
            {
                return new Response()
                {
                    IdError = 1,
                    MessageError = string.Format("Ya existe el cliente con nombre: {0}", clienteRequest.Nombre)
                };
            }

            var response = new Response();
            int resultado = 0;
            int idCliente = 0;

            try
            {
                using (var contexto = new ContextDb())
                {
                    var clientes = new Clientes();
                    clientes.Nombre = clienteRequest.Nombre;
                    clientes.FechaModificacion = DateTime.Now;
                    clientes.Activo = true;
                    contexto.Clientes.Add(clientes);
                    resultado = await contexto.SaveChangesAsync();
                    idCliente = clientes.ID;

                    if(resultado > 0)
                    {
                        if (clienteRequest.Planes != null && clienteRequest.Planes.Count() > 0)
                        {
                            foreach (var plan in clienteRequest.Planes)
                            {
                                contexto.ClientesPlanes.Add(new ClientesPlanes()
                                {
                                    IDClientes = idCliente,
                                    IDPlanes = plan
                                });
                            }
                            resultado = await contexto.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        response.MessageError = "Ocurrio un error al crear cliente";
                    }
                }

                if (resultado > 0)
                {
                    response.IdError = 0;
                }
                else
                {
                    response.IdError = 1;
                    response.MessageError = "Ocurrio un error al asociar planes al cliente";
                }
            }
            catch(Exception error)
            {
                response.IdError = 2;
                response.MessageError = error.Message;
            }

            return response;
        }

        [HttpPost]
        [Route("Editar")]
        public async Task<Response> Editar([FromBody] ClientesViewModel clienteRequest)
        {
            clienteRequest.Nombre = Utils.Utilidades.Formato(clienteRequest.Nombre);
            bool continuar = await new Repositorio.ConsultarSeguros().ValidarNombre(clienteRequest.Nombre, clienteRequest.ID);

            if (continuar)
            {
                return new Response()
                {
                    IdError = 1,
                    MessageError = string.Format("Ya existe el cliente con nombre: {0}", clienteRequest.Nombre)
                };
            }

            var response = new Response();
            int resultado = 0;
            try
            {
                using (var contexto = new ContextDb())
                {
                    var clientes = await contexto.Clientes.FirstOrDefaultAsync(x => x.ID.Equals(clienteRequest.ID));
                    clientes.Nombre = clienteRequest.Nombre;
                    clientes.FechaModificacion = DateTime.Now;
                    clientes.Activo = true;
                    contexto.Entry(clientes).State = System.Data.Entity.EntityState.Modified;
                    resultado = await contexto.SaveChangesAsync();

                    if (resultado > 0)
                    {
                        var listado = contexto.ClientesPlanes.Where(x => x.IDClientes == clienteRequest.ID);

                        if(listado.Count() > 0)
                        {
                            contexto.ClientesPlanes.RemoveRange(listado);
                            resultado = await contexto.SaveChangesAsync();
                        }

                        if (clienteRequest.Planes != null && clienteRequest.Planes.Count() > 0)
                        {
                            foreach (var plan in clienteRequest.Planes)
                            {
                                contexto.ClientesPlanes.Add(new ClientesPlanes()
                                {
                                    IDClientes = clienteRequest.ID,
                                    IDPlanes = plan
                                });
                            }
                            resultado = await contexto.SaveChangesAsync();
                        }
                    }
                    else 
                    {
                        response.MessageError = "Ocurrio un error al modificar cliente";
                    }
                }

                if (resultado > 0)
                {
                    response.IdError = 0;
                }
                else
                {
                    response.IdError = 1;
                    response.MessageError = "Ocurrio un error al modificar los planes del cliente";
                }
            }
            catch(Exception error)
            {
                response.IdError = 2;
                response.MessageError = error.Message;
            }

            return response;
        }

        [HttpGet()]
        [Route("Eliminar/{idCliente:int}")]
        public async Task<Response> Eliminar(int idCliente)
        {
            int resultado = 0;
            try
            {
                using (var contexto = new ContextDb())
                {
                    var clientePlanes = contexto.ClientesPlanes.Where(x => x.IDClientes == idCliente);
                    contexto.ClientesPlanes.RemoveRange(clientePlanes);
                    //resultado = await contexto.SaveChangesAsync();

                    var clientes = await contexto.Clientes.FirstOrDefaultAsync(x => x.ID.Equals(idCliente));
                    clientes.FechaModificacion = DateTime.Now;
                    clientes.Activo = false;
                    contexto.Entry(clientes).State = System.Data.Entity.EntityState.Modified;
                    resultado = await contexto.SaveChangesAsync();
                }

                if (resultado > 0)
                {
                    return new Response() { IdError = 0 };
                }
                else
                {
                    return new Response() { IdError = 1, MessageError = "Sucedio un error al eliminar datos" };
                }
            }
            catch(Exception error)
            {
                return new Response() { IdError = 2, MessageError = error.Message };
            }
        }
    }
}

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
    using System.Threading.Tasks;
    using System.Web.Http;

    public class CoberturasController : ApiController
    {
        [HttpGet]
        public async Task<List<CoberturasViewModel>> Get()
        {
            return await new ConsultarCoberturas().ConsultaCoberturas();
        }

        [HttpGet]
        [Route("ObtenerCobertura/{id:int}")]
        public async Task<CoberturasViewModel> ObtenerCobertura(int id)
        {
            return await new ConsultarCoberturas().ObtenerCobertura(id);
        }

        [HttpGet]
        [Route("ObtenerCoberturaByPlan/{idPlan:int}")]
        public async Task<List<CoberturasViewModel>> ObtenerCoberturaByPlan(int idPlan)
        {
            return await new ConsultarCoberturas().ObtenerCoberturaByPlan(idPlan);
        }

        [HttpPost]
        public async Task<Response> Post([FromBody] Cobertura coberturaRequest)
        {
            coberturaRequest.Descripcion = Utils.Utilidades.Formato(coberturaRequest.Descripcion);

            bool continuar = await new Repositorio.ConsultarCoberturas().ValidarCobertura(coberturaRequest.Descripcion);

            if (continuar)
            {
                return new Response()
                {
                    IdError = 1,
                    MessageError = string.Format("Ya existe la cobertura con nombre: {0}", coberturaRequest.Descripcion)
                };
            }

            var response = new Response();
            int resultado = 0;

            try
            {
                using (var contexto = new ContextDb())
                {
                    var coberturas = new Coberturas();
                    coberturas.Descripcion = coberturaRequest.Descripcion;
                    coberturas.FechaModificacion = DateTime.Now;
                    contexto.Coberturas.Add(coberturas);
                    resultado = await contexto.SaveChangesAsync();
                }

                if (resultado > 0)
                {
                    response.IdError = 0;
                }
                else
                {
                    response.IdError = 1;
                    response.MessageError = "Ocurrio un error al crear la cobertura";
                }
            }
            catch (Exception error)
            {
                response.IdError = 2;
                response.MessageError = error.Message;
            }

            return response;
        }

        [HttpPost]
        [Route("Coberturas/Editar")]
        public async Task<Response> Editar([FromBody] CoberturasViewModel coberturaRequest)
        {
            coberturaRequest.Descripcion = Utils.Utilidades.Formato(coberturaRequest.Descripcion);

            bool continuar = await new Repositorio.ConsultarCoberturas().ValidarCobertura(coberturaRequest.Descripcion);

            if (continuar)
            {
                return new Response()
                {
                    IdError = 1,
                    MessageError = string.Format("Ya existe la cobertura con nombre: {0}", coberturaRequest.Descripcion)
                };
            }

            var response = new Response();
            int resultado = 0;
            try
            {
                using (var contexto = new ContextDb())
                {
                    var coberturas = await contexto.Coberturas.FirstOrDefaultAsync(x => x.ID.Equals(coberturaRequest.ID));
                    coberturas.Descripcion = coberturaRequest.Descripcion;
                    coberturas.FechaModificacion = DateTime.Now;
                    contexto.Entry(coberturas).State = System.Data.Entity.EntityState.Modified;
                    resultado = await contexto.SaveChangesAsync();
                }

                if (resultado > 0)
                {
                    response.IdError = 0;
                }
                else
                {
                    response.IdError = 1;
                    response.MessageError = "Ocurrio un error al modificar cobertura";
                }
            }
            catch (Exception error)
            {
                response.IdError = 2;
                response.MessageError = error.Message;
            }

            return response;
        }
    }
}

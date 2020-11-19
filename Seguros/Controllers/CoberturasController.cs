namespace Seguros.Controllers
{
    using ABDContexto;
    using Seguros.Models.Request;
    using Seguros.Models.ViewModels;
    using Seguros.Repositorio;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class CoberturasController : ApiController
    {
        [HttpGet]
        public List<CoberturasViewModel> Get()
        {
            return new ConsultarCoberturas().ConsultaCoberturas();
        }

        [HttpPost]
        public async Task Post([FromBody] Cobertura coberturarRequest)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var coberturas = new Coberturas();
                    coberturas.Descripcion = coberturarRequest.Descripcion;
                    coberturas.FechaModificacion = DateTime.Now;
                    contexto.Coberturas.Add(coberturas);
                    await contexto.SaveChangesAsync();
                }
            }
            catch (Exception error)
            {
                // MANEJAR EL OBJETO DE ERROR
            }
        }
    }
}

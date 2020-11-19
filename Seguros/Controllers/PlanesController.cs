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

    public class PlanesController : ApiController
    {
        [HttpGet]
        public List<PlanesViewModel> Get()
        {
            return new ConsultarPlanes().ConsultaPlanes();
        }

        [HttpPost]
        public async Task Post([FromBody] Plan planRequest)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var planes = new Planes();
                    planes.Descripcion = planRequest.Descripcion;
                    planes.FechaModificacion = DateTime.Now;
                    contexto.Planes.Add(planes);
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

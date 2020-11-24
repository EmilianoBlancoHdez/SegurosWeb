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

    public class PlanesController : ApiController
    {
        [HttpGet]
        public List<PlanesViewModel> Get()
        {
            return new ConsultarPlanes().ConsultaPlanes();
        }

        [HttpGet]
        [Route("ObtenerPlan/{id:int}")]
        public async Task<PlanesViewModel> ObtenerPlan(int id)
        {
            return await new ConsultarPlanes().ObtenerPlan(id);
        }

        [HttpPost]
        public async Task<Response> Post([FromBody] Plan planRequest)
        {
            planRequest.Descripcion = Utils.Utilidades.Formato(planRequest.Descripcion);

            bool continuar = await new Repositorio.ConsultarPlanes().ValidarPlan(planRequest.Descripcion);

            if (continuar)
            {
                return new Response()
                {
                    IdError = 1,
                    MessageError = string.Format("Ya existe el plan con nombre: {0}", planRequest.Descripcion)
                };
            }

            var response = new Response();
            int resultado = 0;
            int idPlan = 0;

            try
            {
                using (var contexto = new ContextDb())
                {
                    var planes = new Planes();
                    planes.Descripcion = planRequest.Descripcion;
                    planes.FechaModificacion = DateTime.Now;
                    contexto.Planes.Add(planes);
                    resultado = await contexto.SaveChangesAsync();
                    idPlan = planes.ID;

                    if (resultado > 0)
                    {
                        if (planRequest.Coberturas != null && planRequest.Coberturas.Count() > 0)
                        {
                            foreach (var cobertura in planRequest.Coberturas)
                            {
                                contexto.PlanesCobertura.Add(new PlanesCobertura()
                                {
                                    IDPlanes = idPlan,
                                    IDCoberturas = cobertura
                                });
                            }
                            resultado = await contexto.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        response.MessageError = "Ocurrio un error al crear plan";
                    }
                }

                if (resultado > 0)
                {
                    response.IdError = 0;
                }
                else
                {
                    response.IdError = 1;
                    response.MessageError = $"Ocurrio un error al asociar coberturas al plan: {planRequest.Descripcion}";
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
        [Route("Planes/Editar")]
        public async Task<Response> Editar([FromBody] PlanesViewModel planRequest)
        {
            planRequest.Descripcion = Utils.Utilidades.Formato(planRequest.Descripcion);

            bool continuar = await new Repositorio.ConsultarPlanes().ValidarPlan(planRequest.Descripcion, planRequest.ID);

            if (continuar)
            {
                return new Response()
                {
                    IdError = 1,
                    MessageError = string.Format("Ya existe el plan con nombre: {0}", planRequest.Descripcion)
                };
            }

            var response = new Response();
            int resultado = 0;

            try
            {
                using (var contexto = new ContextDb())
                {
                    var planes = await contexto.Planes.FirstOrDefaultAsync(x => x.ID.Equals(planRequest.ID));
                    planes.Descripcion = planRequest.Descripcion;
                    planes.FechaModificacion = DateTime.Now;
                    contexto.Entry(planes).State = System.Data.Entity.EntityState.Modified;
                    resultado = await contexto.SaveChangesAsync();

                    if (resultado > 0)
                    {
                        var listado = contexto.PlanesCobertura.Where(x => x.IDPlanes == planRequest.ID);

                        if (listado.Count() > 0)
                        {
                            contexto.PlanesCobertura.RemoveRange(listado);
                            resultado = await contexto.SaveChangesAsync();
                        }

                        if (planRequest.Coberturas != null && planRequest.Coberturas.Count() > 0)
                        {
                            foreach (var cobertura in planRequest.Coberturas)
                            {
                                contexto.PlanesCobertura.Add(new PlanesCobertura()
                                {
                                    IDPlanes = planRequest.ID,
                                    IDCoberturas = cobertura
                                });
                            }
                            resultado = await contexto.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        response.MessageError = "Ocurrio un error al modificar plan";
                    }
                }

                if (resultado > 0)
                {
                    response.IdError = 0;
                }
                else
                {
                    response.IdError = 1;
                    response.MessageError = "Ocurrio un error al modificar las coberturas del plan";
                }
            }
            catch (Exception error)
            {
                response.IdError = 2;
                response.MessageError = error.Message;
            }

            return response;
        }

        private string Formato(string valor)
        {
            string[] nombre = valor.Trim().Split(' ');
            for (int i = 0; i < nombre.Length; i++)
            {
                nombre[i] = nombre[i][0].ToString().ToUpper() + nombre[i].Substring(1).ToLower();
            }

            return string.Join(" ", nombre);
        }
    }
}

namespace Seguros.Repositorio
{
    using Seguros.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class ConsultarPlanes
    {
        public List<PlanesViewModel> ConsultaPlanes()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            List<PlanesViewModel> planes = new List<PlanesViewModel>();

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("SELECT * FROM Planes;", conexion);
                var datos = comando.ExecuteReader();

                while (datos.Read())
                {
                    planes.Add(new PlanesViewModel()
                    {
                        ID = int.Parse(datos["ID"].ToString()),
                        Descripcion = datos["Descripcion"].ToString(),
                        FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString())
                    });
                }
            }

            return planes;
        }

        public async Task<bool> ValidarPlan(string descripcion, int id = 0)
        {
            int contador = 0;
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("ConsultarPlan", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Descripcion", descripcion);
                comando.Parameters.AddWithValue("@Id", id);

                SqlParameter pvNewId = new SqlParameter();
                pvNewId.ParameterName = "@Contador";
                pvNewId.DbType = DbType.Int32;
                pvNewId.Direction = ParameterDirection.Output;
                comando.Parameters.Add(pvNewId);

                await comando.ExecuteNonQueryAsync();
                contador = int.Parse(comando.Parameters["@Contador"].Value.ToString());
            }

            return contador > 0;
        }

        public async Task<PlanesViewModel> ObtenerPlan(int id)
        {
            var plan = new PlanesViewModel();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("ObtenerPlan", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@ID", id);

                var datos = await comando.ExecuteReaderAsync();

                while (datos.Read())
                {
                    plan.ID = int.Parse(datos["ID"].ToString());
                    plan.Descripcion = datos["Descripcion"].ToString();
                    plan.FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString());
                }
            }

            return plan;
        }

        public async Task<List<PlanesViewModel>> ObtenerPlanByCliente(int idPlan)
        {
            var planes = new List<PlanesViewModel>();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("ObtenerPlanByCliente", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@IDCliente", idPlan);

                var datos = await comando.ExecuteReaderAsync();

                while (datos.Read())
                {
                    planes.Add(new PlanesViewModel()
                    {
                        ID = int.Parse(datos["ID"].ToString()),
                        Descripcion = datos["Descripcion"].ToString(),
                        FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString())
                    });
                }
            }

            return planes;
        }
    }
}
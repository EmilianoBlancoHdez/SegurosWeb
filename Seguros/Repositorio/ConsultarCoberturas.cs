namespace Seguros.Repositorio
{
    using Seguros.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class ConsultarCoberturas
    {
        public async Task<List<CoberturasViewModel>> ConsultaCoberturas()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            List<CoberturasViewModel> coberturas = new List<CoberturasViewModel>();

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("SELECT * FROM Coberturas;", conexion);
                var datos = await comando.ExecuteReaderAsync();

                while (datos.Read())
                {
                    coberturas.Add(new CoberturasViewModel()
                    {
                        ID = int.Parse(datos["ID"].ToString()),
                        Descripcion = datos["Descripcion"].ToString(),
                        FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString())
                    });
                }
            }

            return coberturas;
        }

        public async Task<bool> ValidarCobertura(string descripcion)
        {
            int contador = 0;
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("ConsultarCobertura", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Descripcion", descripcion);

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

        public async Task<CoberturasViewModel> ObtenerCobertura(int id)
        {
            var cobertura = new CoberturasViewModel();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("ObtenerCobertura", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@ID", id);

                var datos = await comando.ExecuteReaderAsync();

                while (datos.Read())
                {
                    cobertura.ID = int.Parse(datos["ID"].ToString());
                    cobertura.Descripcion = datos["Descripcion"].ToString();
                    cobertura.FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString());
                }
            }

            return cobertura;
        }

        public async Task<List<CoberturasViewModel>> ObtenerCoberturaByPlan(int idPlan)
        {
            var coberturas = new List<CoberturasViewModel>();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("ObtenerCoberturaByPlan", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@IDPlanes", idPlan);

                var datos = await comando.ExecuteReaderAsync();

                while (datos.Read())
                {
                    coberturas.Add(new CoberturasViewModel()
                    {
                        ID = int.Parse(datos["ID"].ToString()),
                        Descripcion = datos["Descripcion"].ToString(),
                        FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString())
                    });
                }
            }

            return coberturas;
        }
    }
}
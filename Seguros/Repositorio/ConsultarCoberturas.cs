namespace Seguros.Repositorio
{
    using Seguros.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;

    public class ConsultarCoberturas
    {
        public List<CoberturasViewModel> ConsultaCoberturas()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            List<CoberturasViewModel> coberturas = new List<CoberturasViewModel>();

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("SELECT * FROM Coberturas;", conexion);
                var datos = comando.ExecuteReader();

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
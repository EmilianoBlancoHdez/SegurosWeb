namespace Seguros.Repositorio
{
    using Seguros.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;

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
    }
}
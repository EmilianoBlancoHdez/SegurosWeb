namespace Seguros.Repositorio
{
    using Seguros.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;

    public class ConsultarSeguros
    {
        public  List<ClientesViewModel> ConsultaClientes()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            List<ClientesViewModel> clientes = new List<ClientesViewModel>();

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("SELECT * FROM Clientes;", conexion);
                var datos = comando.ExecuteReader();

                while(datos.Read())
                {
                    string aaaa = datos["Nombre"].ToString();
                    clientes.Add(new ClientesViewModel() 
                    { 
                        ID = int.Parse(datos["ID"].ToString()),
                        Nombre = datos["Nombre"].ToString(),
                        FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString())
                    });
                }
            }

            return clientes;
        }
    }
}
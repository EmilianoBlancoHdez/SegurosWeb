namespace Seguros.Repositorio
{
    using Seguros.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public enum Query
    {
        CLIENTES = 1,
        LISTADO = 2
    }

    public class ConsultarSeguros
    {
        public async Task<List<ClientesViewModel>> ConsultaClientes(Query consulta)
        {
            string query = string.Empty;
            switch(consulta)
            {
                case (Query.CLIENTES):
                    query = "SELECT * FROM Clientes WITH(NOLOCK) WHERE Activo = 1;";
                    break;
                case (Query.LISTADO):
                    query = "SELECT C.*	FROM AltaClientes AS AC WITH(NOLOCK) INNER JOIN CLIENTES AS C WITH(NOLOCK) ON AC.IDCliente = C.ID;";
                    break;
            }

            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            List<ClientesViewModel> clientes = new List<ClientesViewModel>();

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand(query, conexion);
                var datos = await comando.ExecuteReaderAsync();

                while(datos.Read())
                {
                    clientes.Add(new ClientesViewModel() 
                    { 
                        ID = int.Parse(datos["ID"].ToString()),
                        Nombre = datos["Nombre"].ToString(),
                        FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString()),
                        Activo = bool.Parse(datos["Activo"].ToString())
                    });
                }
            }

            return clientes;
        }

        public async Task<bool> ValidarNombre(string nombre, int id = 0)
        {
            int contador = 0;
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("ConsultarNombre", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Nombre", nombre);
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

        public async Task<ClientesViewModel> ObtenerCliente(int id)
        {
            var cliente = new ClientesViewModel();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var comando = new SqlCommand("ObtenerCliente", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@ID", id);

                var datos = await comando.ExecuteReaderAsync();

                while(datos.Read())
                {
                    cliente.ID = int.Parse(datos["ID"].ToString());
                    cliente.Nombre = datos["Nombre"].ToString();
                    cliente.FechaModificacion = DateTime.Parse(datos["FechaModificacion"].ToString());
                }
            }

            return cliente;
        }
    }
}
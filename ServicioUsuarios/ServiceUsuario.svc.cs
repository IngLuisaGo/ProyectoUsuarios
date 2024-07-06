using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Entidades;

namespace ServicioUsuarios
{
    public class ServiceUsuario : IServiceUsuario
    {
        private static readonly string connectionString;

        static ServiceUsuario()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DBCRUDE"].ConnectionString;
        }

        public bool InsertarUsuario(BUsuarios usuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_InsertarUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@Fecha", usuario.Fecha);
                        command.Parameters.AddWithValue("@Sexo", usuario.Sexo);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                return false;
            }
        }

        public string EditarUsuario(BUsuarios usuario)
        {
            throw new NotImplementedException();
        }

        public string EliminarUsuario(BUsuarios usuario)
        {
            throw new NotImplementedException();
        }
    }
}

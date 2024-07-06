using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Entidades;
using System.Collections.Generic;

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
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_EliminarUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", usuario.Id);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0 ? "Usuario eliminado correctamente" : "No se pudo eliminar el usuario";
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                return $"Error al eliminar usuario: {ex.Message}";
            }
        }

        public List<BUsuarios> ConsultarUsuarios()
        {
            List<BUsuarios> usuarios = new List<BUsuarios>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_ListarUsuarios", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BUsuarios usuario = new BUsuarios();
                                usuario.Id = Convert.ToInt32(reader["Id"]);
                                usuario.Nombre = Convert.ToString(reader["Nombre"]);
                                usuario.Fecha = Convert.ToDateTime(reader["Fecha"]);
                                usuario.Sexo = Convert.ToChar(reader["Sexo"]);

                                usuarios.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                throw new Exception($"Error al listar usuarios: {ex.Message}");
            }

            return usuarios;
        }
    }
}

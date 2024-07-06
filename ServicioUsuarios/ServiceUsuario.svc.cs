using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Entidades;
using System.Configuration;

namespace ServicioUsuarios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "ServiceUsuario" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione ServiceUsuario.svc o ServiceUsuario.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServiceUsuario : IServiceUsuario
    {
        private static readonly string connectionString;

        // Constructor estático para inicializar la cadena de conexión
        static ServiceUsuario()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DBCRUDE"].ConnectionString;
        }

        public string InsertarUsuario(BUsuarios usuario)
        {
            string resultado;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    // Crear un comando para ejecutar el procedimiento almacenado
                    using (SqlCommand command = new SqlCommand("sp_InsertarUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@Fecha", usuario.Fecha);
                        command.Parameters.AddWithValue("@Sexo", usuario.Sexo);

                        // Ejecutar el comando
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificar si se insertó el usuario
                        if (rowsAffected > 0)
                        {
                            resultado = "Usuario insertado exitosamente.";
                        }
                        else
                        {
                            resultado = "Error al insertar el usuario.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                resultado = $"Error: {ex.Message}";
            }

            return resultado;
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

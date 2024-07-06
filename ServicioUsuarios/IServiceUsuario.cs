using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServicioUsuarios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IServiceUsuario" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServiceUsuario
    {
        [OperationContract]
        string InsertarUsuario(BUsuarios usuario);
        [OperationContract]
        string EditarUsuario(BUsuarios usuario);
        [OperationContract]
        string EliminarUsuario(BUsuarios usuario);
    }
}

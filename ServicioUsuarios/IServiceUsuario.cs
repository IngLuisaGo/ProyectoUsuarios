using System.Collections.Generic;
using System.ServiceModel;
using Entidades;

namespace ServicioUsuarios
{
    [ServiceContract]
    public interface IServiceUsuario
    {
        [OperationContract]
        bool InsertarUsuario(BUsuarios usuario);

        [OperationContract]
        string EditarUsuario(BUsuarios usuario);

        [OperationContract]
        string EliminarUsuario(BUsuarios usuario);

        [OperationContract]
        List<BUsuarios> ConsultarUsuarios();

        [OperationContract]
        BUsuarios ObtenerUsuario(int id);
    }
}

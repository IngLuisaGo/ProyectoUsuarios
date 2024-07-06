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
    }
}

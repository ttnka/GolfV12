using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG224RolTServ
    {
        Task<IEnumerable<G224RolT>> Buscar(int torneo);
        Task<IEnumerable<G224RolT>> GetRoles();
        Task<G224RolT> GetRol(int rolId);
        Task<G224RolT> AddRol(G224RolT rol);
        Task<G224RolT> UpdateRol(G224RolT rol);
    }
}

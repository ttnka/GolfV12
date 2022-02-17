using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG224RolT
    {
        Task<IEnumerable<G224RolT>> Buscar(int torneo);
        Task<IEnumerable<G224RolT>> GetRoles();
        Task<G224RolT> GetRol(int rolId);
        Task<G224RolT> AddRol(G224RolT rol);
        Task<G224RolT> UpdateRol(G224RolT rol);
    }
}

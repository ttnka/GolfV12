using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG128Hcp
    {
        Task<IEnumerable<G128Hcp>> Buscar(int PlayerId, string apodo, string nombre, 
            string apellido, string campo);
        Task<IEnumerable<G128Hcp>> GetHcps();
        Task<G128Hcp> GetHcp(int hcpId);
        Task<G128Hcp> AddHcp(G128Hcp hcp);
        Task<G128Hcp> UpdateHcp(G128Hcp hcp);
    }
}

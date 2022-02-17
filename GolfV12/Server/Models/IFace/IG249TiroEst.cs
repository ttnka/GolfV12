using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG249TiroEst
    {
        Task<IEnumerable<G249TiroEstadistica>> Buscar(int rol, int player, int hoyo, 
            TiroTipo? tiroTipo);
        Task<IEnumerable<G249TiroEstadistica>> GetTiroEsts();
        Task<G249TiroEstadistica> GetTiroEst(int tiroEstId);
        Task<G249TiroEstadistica> AddTiroEst(G249TiroEstadistica tiroEst);
        Task<G249TiroEstadistica> UpdateTiroEst(G249TiroEstadistica tiroEst);
    }
}

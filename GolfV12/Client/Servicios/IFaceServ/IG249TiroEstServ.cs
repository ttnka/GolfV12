using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG249TiroEstServ
    {
        Task<IEnumerable<G249TiroEstadistica>> Buscar(int rol, string? player, int hoyo,
               TiroTipo? tiroTipo);
        Task<IEnumerable<G249TiroEstadistica>> GetTiroEsts();
        Task<G249TiroEstadistica> GetTiroEst(int tiroEstId);
        Task<G249TiroEstadistica> AddTiroEst(G249TiroEstadistica tiroEst);
        Task<G249TiroEstadistica> UpdateTiroEst(G249TiroEstadistica tiroEst);
    }
}

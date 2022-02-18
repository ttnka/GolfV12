using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG222PlayerTServ
    {
        Task<IEnumerable<G222PlayerT>> Buscar(int team, string? player);
        Task<IEnumerable<G222PlayerT>> GetPlayers();
        Task<G222PlayerT> GetPlayer(string playerId);
        Task<G222PlayerT> AddPlayer(G222PlayerT player);
        Task<G222PlayerT> UpdatePlayer(G222PlayerT player);
    }
}

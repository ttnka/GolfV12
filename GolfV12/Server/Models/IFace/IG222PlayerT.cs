using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG222PlayerT
    {
        Task<IEnumerable<G222PlayerT>> Buscar(int team, string? player);
        Task<IEnumerable<G222PlayerT>> GetPlayers();
        Task<G222PlayerT> GetPlayer(int playerId);
        Task<G222PlayerT> AddPlayer(G222PlayerT player);
        Task<G222PlayerT> UpdatePlayer(G222PlayerT player);
    }
}

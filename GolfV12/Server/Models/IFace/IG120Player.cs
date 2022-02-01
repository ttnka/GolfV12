using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG120Player
    {
        Task<IEnumerable<G120Player>> Buscar(string org, string apodo, string nombre, string paterno);
        Task<IEnumerable<G120Player>> GetPlayers();
        Task<G120Player> GetPlayer(int playerId);
        Task<G120Player> AddPlayer(G120Player player);
        Task<G120Player> UpdatePlayer(G120Player player);
    }
}

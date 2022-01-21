using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG120PlayerServ
    {
        Task<IEnumerable<G120Player>> Buscar(string org, string apodo, string nombre, string paterno, DateTime bday);
        Task<IEnumerable<G120Player>> GetPlayers();
        Task<G120Player> GetPlayer(int playerId);
        Task<G120Player> AddPlayer(G120Player player);
        Task<G120Player> UpdatePlayer(G120Player player);
    }
}

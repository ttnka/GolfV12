using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG120PlayerServ
    {
        /*
        Task<IEnumerable<G120Player>> Buscar(string? userId, int org, string? apodo, string? nombre, string? paterno);
        Task<IEnumerable<G120Player>> GetPlayers();
        Task<G120Player> GetPlayer(string userId);
        */
        Task<IEnumerable<G120Player>> Filtro(string? clave);
        Task<G120Player> AddPlayer(G120Player player);
        Task<G120Player> UpdatePlayer(G120Player player);
    }
}

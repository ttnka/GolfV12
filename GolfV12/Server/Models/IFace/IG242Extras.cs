using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG242Extras
    {
        /*
        Task<IEnumerable<G242Extras>> Buscar(int rol, string player, int hoyo, int tipoExtra);
        Task<IEnumerable<G242Extras>> GetExtras();
        Task<G242Extras> GetExtra(int extraId);
        */
        Task<IEnumerable<G242Extras>> Filtro(string? clave);
        Task<G242Extras> AddExtra(G242Extras extra);
        Task<G242Extras> UpdateExtra(G242Extras extra);

    }
}

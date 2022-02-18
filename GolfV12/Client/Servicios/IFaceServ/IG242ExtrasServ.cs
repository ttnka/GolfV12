using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG242ExtrasServ
    {
        Task<IEnumerable<G242Extras>> Buscar(int rol, string player, int hoyo, int tipoExtra);
        Task<IEnumerable<G242Extras>> GetExtras();
        Task<G242Extras> GetExtra(int extraId);
        Task<G242Extras> AddExtra(G242Extras extra);
        Task<G242Extras> UpdateExtra(G242Extras extra);
    }
}

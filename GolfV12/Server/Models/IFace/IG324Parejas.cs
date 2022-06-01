using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG324Parejas
    {
        Task<IEnumerable<G324Parejas>> Filtro(string? clave);
        Task<G324Parejas> AddPareja(G324Parejas pareja);
        Task<G324Parejas> UpdatePareja(G324Parejas pareja);
    }
}

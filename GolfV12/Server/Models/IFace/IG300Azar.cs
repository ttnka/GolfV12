using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG300Azar
    {
        Task<IEnumerable<G300Azar>> Filtro(string? clave);
        Task<G300Azar> AddAzar(G300Azar azar);
        Task<G300Azar> UpdateAzar(G300Azar azar);
    }
}

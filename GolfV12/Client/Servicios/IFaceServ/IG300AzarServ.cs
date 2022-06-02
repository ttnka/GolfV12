using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG300AzarServ
    {
        Task<IEnumerable<G300Azar>> Filtro(string? clave);
        Task<G300Azar> AddAzar(G300Azar azar);
        Task<G300Azar> UpdateAzar(G300Azar azar);
    }
}

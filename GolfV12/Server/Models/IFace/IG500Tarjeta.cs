using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG500Tarjeta
    {
        Task<IEnumerable<G500Tarjeta>> Filtro(string? clave);
        Task<G500Tarjeta> AddTarjeta(G500Tarjeta tarjeta);
        Task<G500Tarjeta> UpdateTarjeta(G500Tarjeta tarjeta);
    }
}

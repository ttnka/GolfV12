using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG502Tarjetas
    {
        Task<IEnumerable<G502Tarjetas>> Filtro(string? clave);
        Task<G502Tarjetas> AddTarjeta(G502Tarjetas tarjeta);
        Task<G502Tarjetas> UpdateTarjeta(G502Tarjetas tarjeta);
    }
}

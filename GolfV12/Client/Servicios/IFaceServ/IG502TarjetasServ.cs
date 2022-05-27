using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG502TarjetasServ
    {
        Task<IEnumerable<G502Tarjetas>> Filtro(string? clave);
        Task<G502Tarjetas> AddTarjeta(G502Tarjetas tarjeta);
        Task<G502Tarjetas> UpdateTarjeta(G502Tarjetas tarjeta);
    }
}

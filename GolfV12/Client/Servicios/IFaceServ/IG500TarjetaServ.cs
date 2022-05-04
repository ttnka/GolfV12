using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG500TarjetaServ
    {
        Task<IEnumerable<G500Tarjeta>> Filtro(string? clave);
        Task<G500Tarjeta> AddTarjeta(G500Tarjeta tarjeta);
        Task<G500Tarjeta> UpdateTarjeta(G500Tarjeta tarjeta);
    }
}

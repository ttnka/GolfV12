using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG510JugadorServ
    {
        Task<IEnumerable<G510Jugador>> Filtro(string? clave);
        Task<G510Jugador> AddJugador(G510Jugador jugador);
        Task<G510Jugador> UpdateJugador(G510Jugador jugador);
    }
}

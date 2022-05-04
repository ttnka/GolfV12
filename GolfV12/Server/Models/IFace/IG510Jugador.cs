using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG510Jugador
    {
        Task<IEnumerable<G510Jugador>> Filtro(string? clave);
        Task<G510Jugador> AddJugador(G510Jugador jugador);
        Task<G510Jugador> UpdateJugador(G510Jugador jugador);
    }
}

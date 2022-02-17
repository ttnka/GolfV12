using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG200Torneo
    {
        Task<IEnumerable<G200Torneo>> Buscar(int ejercicio, string? titulo, string? creador);
        Task<IEnumerable<G200Torneo>> GetTorneos();
        Task<G200Torneo> GetTorneo(int torneoId);
        Task<G200Torneo> AddTorneo(G200Torneo torneo);
        Task<G200Torneo> UpdateTorneo(G200Torneo torneo);
    }
}

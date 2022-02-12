using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG194Cita
    {
        Task<IEnumerable<G194Cita>> Buscar(string? creador, string? desc, int masterId, DateTime fini, DateTime ffin);
        Task<IEnumerable<G194Cita>> GetCitas();
        Task<G194Cita> GetCita(int citaId);
        Task<G194Cita> AddCita(G194Cita cita);
        Task<G194Cita> UpdateCita(G194Cita cita);
    }
}

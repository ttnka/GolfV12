using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG310ParticipantesServ
    {
        Task<IEnumerable<G310Participantes>> Filtro(string? clave);
        Task<G310Participantes> AddParticipante(G310Participantes participante);
        Task<G310Participantes> UpdateParticipante(G310Participantes participante);
    }
}

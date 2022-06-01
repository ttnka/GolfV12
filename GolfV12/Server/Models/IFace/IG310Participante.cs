using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG310Participante
    {
        Task<IEnumerable<G310Participantes>> Filtro(string? clave);
        Task<G310Participantes> AddParticipante(G310Participantes participante);
        Task<G310Participantes> UpdateParticipante(G310Participantes participante);
    }
}

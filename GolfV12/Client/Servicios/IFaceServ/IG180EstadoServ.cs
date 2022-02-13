using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG180EstadoServ
    {
        Task<IEnumerable<G180Estado>> Buscar(string? titulo, string? grupo);
        Task<IEnumerable<G180Estado>> GetEstados();
        Task<G180Estado> GetEstado(int estadoId);
        Task<G180Estado> AddEstado(G180Estado estado);
        Task<G180Estado> UpdateEstado(G180Estado estado);
    }
}

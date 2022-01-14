using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG170Campo
    {
        Task<IEnumerable<G170Campo>> Buscar(string corto, string nombre, string ciudad, string pais);
        Task<IEnumerable<G170Campo>> GetCampos();
        Task<G170Campo> GetCampo(int campoId);
        Task<G170Campo> AddCampo(G170Campo campo);
        Task<G170Campo> UpdateCampo(G170Campo campo);
    }
}

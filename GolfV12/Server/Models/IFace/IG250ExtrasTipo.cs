using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG250ExtrasTipo
    {
        /*
        Task<IEnumerable<G250ExtrasTipo>> Buscar(string? titulo, string? creador, string? grupo,
               bool publico);
        Task<IEnumerable<G250ExtrasTipo>> GetExtrasTipos();
        Task<G250ExtrasTipo> GetExtrasTipo(int extrasTipoId);
        */
        Task<IEnumerable<G250ExtrasTipo>> Filtro(string? clave);
        Task<G250ExtrasTipo> AddExtrasTipo(G250ExtrasTipo extrasTipo);
        Task<G250ExtrasTipo> UpdateExtrasTipo(G250ExtrasTipo extrasTipo);
    }
}

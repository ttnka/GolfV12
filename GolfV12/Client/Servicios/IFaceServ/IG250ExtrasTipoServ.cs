using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG250ExtrasTipoServ
    {
        Task<IEnumerable<G250ExtrasTipo>> Buscar(string? titulo, string? creador, string? grupo,
                  bool publico);
        Task<IEnumerable<G250ExtrasTipo>> GetExtrasTipos();
        Task<G250ExtrasTipo> GetExtrasTipo(int extrasTipoId);
        Task<G250ExtrasTipo> AddExtrasTipo(G250ExtrasTipo extrasTipo);
        Task<G250ExtrasTipo> UpdateExtrasTipo(G250ExtrasTipo extrasTipo);
    }
}

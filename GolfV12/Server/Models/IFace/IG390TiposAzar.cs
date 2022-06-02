using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG390TiposAzar
    {
        Task<IEnumerable<G390TiposAzar>> Filtro(string? clave);
        Task<G390TiposAzar> AddTiposAzar(G390TiposAzar azarT);
        Task<G390TiposAzar> UpdateTiposAzar(G390TiposAzar azarT);
    }
}

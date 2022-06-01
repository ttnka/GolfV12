using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG330Loba
    {
        Task<IEnumerable<G330Loba>> Filtro(string? clave);
        Task<G330Loba> AddLoba(G330Loba loba);
        Task<G330Loba> UpdateLoba(G330Loba loba);
    }
}

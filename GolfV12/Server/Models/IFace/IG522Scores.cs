using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG522Scores
    {
        Task<IEnumerable<G522Scores>> Filtro(string? clave);
        Task<G522Scores> AddScore(G522Scores score);
        Task<G522Scores> UpdateScore(G522Scores score);
    }
}

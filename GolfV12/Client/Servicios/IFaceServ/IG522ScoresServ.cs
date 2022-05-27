using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG522ScoresServ
    {
        Task<IEnumerable<G522Scores>> Filtro(string? clave);
        Task<G522Scores> AddScore(G522Scores score);
        Task<G522Scores> UpdateScore(G522Scores score);
    }
}

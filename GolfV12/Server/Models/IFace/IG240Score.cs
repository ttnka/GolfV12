using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG240Score
    {
        Task<IEnumerable<G240Score>> Buscar(int rol, string? player, int hoyo);
        Task<IEnumerable<G240Score>> GetScores();
        Task<G240Score> GetScore(int scoreId);
        Task<G240Score> AddScore(G240Score score);
        Task<G240Score> UpdateScore(G240Score score);
    }
}

using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG202JobT
    {
        Task<IEnumerable<G202JobT>> Buscar(int torneo, string? player, int contrincante);
        Task<IEnumerable<G202JobT>> GetJobs();
        Task<G202JobT> GetJob(int jobId);
        Task<G202JobT> AddJob(G202JobT job);
        Task<G202JobT> UpdateJob(G202JobT job);
    }
}

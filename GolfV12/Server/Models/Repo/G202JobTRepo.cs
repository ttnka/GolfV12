using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G202JobTRepo : IG202JobT
    {
        private readonly ApplicationDbContext _appDbContext;

        public G202JobTRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G202JobT> AddJob(G202JobT job)
        {
            var res = await _appDbContext.Jobs.AddAsync(job);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G202JobT>> Buscar(int torneo, int player, int contrincante)
        {
            IQueryable<G202JobT> querry = _appDbContext.Jobs;
            if (torneo > 0) querry = querry.Where(e => e.Torneo == torneo);
            if (player > 0) querry = querry.Where(e => e.Player == player);
            if (contrincante > 0) querry = querry.Where(e => e.Contrincante == contrincante);

            return await querry.ToListAsync();
        }

        public async Task<G202JobT> GetJob(int jobId)
        {
            var res = await _appDbContext.Jobs.FirstOrDefaultAsync(e => e.Id == jobId);
            return res != null ? res : new G202JobT();
        }

        public async Task<IEnumerable<G202JobT>> GetJobs()
        {
            return await _appDbContext.Jobs.ToListAsync();
        }

        public async Task<G202JobT> UpdateJob(G202JobT job)
        {
            var res = await _appDbContext.Jobs.FirstOrDefaultAsync(e => e.Id == job.Id);
            if (res != null)
            {
                if (job.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Torneo = job.Torneo;
                    res.Player = job.Player;
                    res.JobT = job.JobT;
                    res.Contrincante = job.Contrincante;
                    res.Estado = job.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G202JobT();
            }
            return res;
        }
    }
}

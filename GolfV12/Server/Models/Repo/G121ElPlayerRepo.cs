using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G121ElPlayerRepo : IG121ElPlayer
    {
        private readonly ApplicationDbContext _appDbContext;

        public G121ElPlayerRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G120Player> GetPlayer(string userId)
        {   
            var resultado = await _appDbContext.Players.FirstOrDefaultAsync(x => x.UserId.Contains(userId));
            return resultado != null ? resultado : new G120Player();
            
        }
    }
}

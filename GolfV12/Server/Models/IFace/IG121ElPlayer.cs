using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG121ElPlayer
    {
        Task<G120Player> GetPlayer(string userId);
    }
}

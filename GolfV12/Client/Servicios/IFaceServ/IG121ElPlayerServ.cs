using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG121ElPlayerServ
    {
        Task<G120Player> GetPlayer(string userId);
    }
}

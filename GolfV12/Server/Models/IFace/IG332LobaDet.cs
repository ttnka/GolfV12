using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG332LobaDet
    {
        Task<IEnumerable<G332LobaDet>> Filtro(string? clave);
        Task<G332LobaDet> AddLobaD(G332LobaDet lobaD);
        Task<G332LobaDet> UpdateLobaD(G332LobaDet lobaD);
    }
}

using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG220TeamTServ
    {
        Task<IEnumerable<G220TeamT>> Buscar(int teamNum, string? nombre);
        Task<IEnumerable<G220TeamT>> GetTeams();
        Task<G220TeamT> GetTeam(int teamId);
        Task<G220TeamT> AddTeam(G220TeamT team);
        Task<G220TeamT> UpdateTeam(G220TeamT team);
    }
}

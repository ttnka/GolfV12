using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG172BanderaServ
    {
        Task<IEnumerable<G172Bandera>> Buscar(int campo, string? color);
        Task<IEnumerable<G172Bandera>> GetBanderas();
        Task<G172Bandera> GetBandera(int banderaId);
        Task<G172Bandera> AddBandera(G172Bandera bandera);
        Task<G172Bandera> UpdateBandera(G172Bandera bandera);
    }
}

using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG176HoyoServ
    {
        Task<IEnumerable<G176Hoyo>> Buscar(int campo, string ruta, int hoyoN);
        Task<IEnumerable<G176Hoyo>> GetHoyos();
        Task<G176Hoyo> GetHoyo(int hoyoId);
        Task<G176Hoyo> AddHoyo(G176Hoyo hoyo);
        Task<G176Hoyo> UpdateHoyo(G176Hoyo hoyo);
    }
}

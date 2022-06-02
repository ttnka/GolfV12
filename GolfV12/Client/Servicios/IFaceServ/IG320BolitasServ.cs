using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG320BolitasServ
    {
        Task<IEnumerable<G320Bolitas>> Filtro(string? clave);
        Task<G320Bolitas> AddBolitas(G320Bolitas bolita);
        Task<G320Bolitas> UpdateBolitas(G320Bolitas bolita);
    }
}

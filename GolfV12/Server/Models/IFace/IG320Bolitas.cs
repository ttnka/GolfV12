using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG320Bolitas
    {
        Task<IEnumerable<G320Bolitas>> Filtro(string? clave);
        Task<G320Bolitas> AddBolitas(G320Bolitas bolitas);
        Task<G320Bolitas> UpdateBolitas(G320Bolitas bolitas);
    }
}

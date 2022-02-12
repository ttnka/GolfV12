using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG136Foto
    {
        Task<IEnumerable<G136Foto>> Buscar(string? playerId, string? titulo, DateTime bday);
        Task<IEnumerable<G136Foto>> GetFotos();
        Task<G136Foto> GetFoto(int fotoId);
        Task<G136Foto> AddFoto(G136Foto foto);
        Task<G136Foto> UpdateFoto(G136Foto foto);
    }
}

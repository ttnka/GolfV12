using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG280FormatoT
    {
        Task<IEnumerable<G280FormatoT>> Buscar(string? clave, string? titulo,
               string? desc, bool individual);
        Task<IEnumerable<G280FormatoT>> GetFormatos();
        Task<G280FormatoT> GetFormato(int formatoId);
        Task<G280FormatoT> AddFormato(G280FormatoT formato);
        Task<G280FormatoT> UpdateFormato(G280FormatoT formato);
    }
}

using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG280FormatoTServ
    {
        Task<IEnumerable<G280FormatoT>> Buscar(string? clave, string? titulo,
                  string? desc, bool individual);
        Task<IEnumerable<G280FormatoT>> GetFormatos();
        Task<G280FormatoT> GetFormato(int formatoId);
        Task<G280FormatoT> AddFormato(G280FormatoT formato);
        Task<G280FormatoT> UpdateFormato(G280FormatoT formato);
    }
}

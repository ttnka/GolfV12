using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG208CategoriaT
    {
        Task<IEnumerable<G208CategoriaT>> Buscar(int torneo, string? titulo);
        Task<IEnumerable<G208CategoriaT>> GetCategorias();
        Task<G208CategoriaT> GetCategoria(int categoriaId);
        Task<G208CategoriaT> AddCategoria(G208CategoriaT categoria);
        Task<G208CategoriaT> UpdateCategoria(G208CategoriaT categoria);
    }
}

using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG208CategoriaTServ
    {
        Task<IEnumerable<G208CategoriaT>> Buscar(int torneo, string? titulo);
        Task<IEnumerable<G208CategoriaT>> GetCategorias();
        Task<G208CategoriaT> GetCategoria(int categoriaId);
        Task<G208CategoriaT> AddCategoria(G208CategoriaT categoria);
        Task<G208CategoriaT> UpdateCategoria(G208CategoriaT categoria);
    }
}

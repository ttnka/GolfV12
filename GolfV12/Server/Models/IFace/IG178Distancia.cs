using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG178Distancia
    {
        Task<IEnumerable<G178Distancia>> Buscar(string campo, string bandera, int hoyoN);
        Task<IEnumerable<G178Distancia>> GetDistancias();
        Task<G178Distancia> GetDistancia(int distanciaId);
        Task<G178Distancia> AddDistancia(G178Distancia distancia);
        Task<G178Distancia> UpdateDistancia(G178Distancia distancia);
    }
}

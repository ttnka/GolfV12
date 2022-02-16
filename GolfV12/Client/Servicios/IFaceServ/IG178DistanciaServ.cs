using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG178DistanciaServ
    {
        Task<IEnumerable<G178Distancia>> Buscar(int bandera, int hoyoN);
        Task<IEnumerable<G178Distancia>> GetDistancias();
        Task<G178Distancia> GetDistancia(int distanciaId);
        Task<G178Distancia> AddDistancia(G178Distancia distancia);
        Task<G178Distancia> UpdateDistancia(G178Distancia distancia);
    }
}

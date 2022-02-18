using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG204FechaTServ 
    { 
        Task<IEnumerable<G204FechaT>> Buscar(int torneo, DateTime fecha);
        Task<IEnumerable<G204FechaT>> GetFechasT();
        Task<G204FechaT> GetFechaT(int fechaTId);
        Task<G204FechaT> AddFechaT(G204FechaT fechaT);
        Task<G204FechaT> UpdateFechaT(G204FechaT fechaT);
    }
}

using GolfV12.Shared;

namespace GolfV12.Client.Servicios.IFaceServ
{
    public interface IG190BitacoraServ
    {
        Task<IEnumerable<G190Bitacora>> Buscar(string userId, bool sitema,
           BitaAcciones? accion, string texto, DateTime fini, DateTime ffin);
        Task<IEnumerable<G190Bitacora>> GetBitacoraAll();
        Task<G190Bitacora> GetBitacora(int bitacoraId);
        Task<G190Bitacora> AddBitacora(G190Bitacora bitacora);
    }
}

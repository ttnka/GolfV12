using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG110Organizacion
    {
        Task<IEnumerable<G110Organizacion>> Buscar(string? clave, string? nombre, 
            string? desc);
        Task<IEnumerable<G110Organizacion>> GetOrganizaciones();
        Task<G110Organizacion> GetOrganizacion(int organizacionId);
        Task<G110Organizacion> AddOrganizacion(G110Organizacion organizacion);
        Task<G110Organizacion> UpdateOrganizacion(G110Organizacion organizacion);
    }
}

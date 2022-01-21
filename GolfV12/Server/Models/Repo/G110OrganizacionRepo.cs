using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G110OrganizacionRepo : IG110Organizacion
    {
        private readonly ApplicationDbContext _appDbContext;

        public G110OrganizacionRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G110Organizacion> AddOrganizacion(G110Organizacion organizacion)
        {
            var res = await _appDbContext.Organizaciones.AddAsync(organizacion);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G110Organizacion>> Buscar(string clave, string nombre, string desc)
        {
            IQueryable<G110Organizacion> querry = _appDbContext.Organizaciones;
            if (!string.IsNullOrEmpty(clave)) querry = querry.Where(e => e.Clave.Contains(clave));
            if (!string.IsNullOrEmpty(nombre)) querry = querry.Where(e => e.Nombre.Contains(nombre));
            if(!string.IsNullOrEmpty(desc)) querry = querry.Where(e=>e.Desc.Contains(desc));   
            
            return await querry.ToListAsync();
        }

        public async Task<G110Organizacion> GetOrganizacion(int organizacionId)
        {
            var res = await _appDbContext.Organizaciones.FirstOrDefaultAsync(e=>e.Id == organizacionId);
            return res != null ? res : new G110Organizacion();
        }

        public async Task<IEnumerable<G110Organizacion>> GetOrganizaciones()
        {
            return await _appDbContext.Organizaciones.ToListAsync();
        }

        public async Task<G110Organizacion> UpdateOrganizacion(G110Organizacion organizacion)
        {
            var res = await _appDbContext.Organizaciones.FirstOrDefaultAsync(e => e.Id == organizacion.Id);
            if (res != null)
            {
                if (organizacion.Status == false)
                {
                    res.Status = false;
                } else
                {
                    res.Clave = organizacion.Clave;
                    res.Nombre = organizacion.Nombre;
                    res.Desc = organizacion.Desc;
                    res.Estado = organizacion.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G110Organizacion();
            }
            return res;
        }
    }
}

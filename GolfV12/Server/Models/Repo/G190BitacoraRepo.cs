using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G190BitacoraRepo : IG190Bitacora
    {
        private readonly ApplicationDbContext _appDbContext;

        public G190BitacoraRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G190Bitacora> AddBitacora(G190Bitacora bitacora)
        {
            var res = await _appDbContext.Bitacoras.AddAsync(bitacora);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G190Bitacora>> Buscar(string? userId, 
            bool sitema, BitaAcciones? accion, string? texto, DateTime fini, DateTime ffin)
        {
            IQueryable<G190Bitacora> querry = _appDbContext.Bitacoras;
            if (!string.IsNullOrEmpty(userId)) querry = querry.Where (e => e.UsuarioId == userId);
            if (sitema) querry = querry.Where(e => e.Sistema == true);
            if (accion != null) querry = querry.Where(e => e.Accion == accion);
            if (!string.IsNullOrEmpty(texto)) querry = querry.Where(e => e.Desc.Contains(texto));
            if (fini > DateTime.MinValue) querry = querry.Where(e => e.Fecha.Date >= fini.Date);
            if (ffin > DateTime.MinValue) querry = querry.Where(e => e.Fecha.Date <= ffin.Date);

            return await querry.ToListAsync();
        }

        public async Task<G190Bitacora> GetBitacora(int bitacoraId)
        {
            var res = await _appDbContext.Bitacoras.FirstOrDefaultAsync(e => e.Id == bitacoraId);
            return res != null ? res : new G190Bitacora();
        }

        public async Task<IEnumerable<G190Bitacora>> GetBitacoraAll()
        {
            return await _appDbContext.Bitacoras.OrderByDescending(e => e.Id).ToListAsync();
        }
    }
}

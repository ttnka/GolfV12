using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G136FotoRepo : IG136Foto
    {
        private readonly ApplicationDbContext _appDbContext;

        public G136FotoRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G136Foto> AddFoto(G136Foto foto)
        {
            var res = await _appDbContext.Fotos.AddAsync(foto);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G136Foto>> Buscar(int playerId, string titulo, DateTime bday)
        {
            IQueryable<G136Foto> querry = _appDbContext.Fotos;
            if (playerId > -1) querry = querry.Where(e => e.PlayerId == playerId);
            if (!string.IsNullOrEmpty(titulo)) querry = querry.Where(e => e.Titulo.Contains(titulo));
            
            return await querry.ToListAsync();
        }

        public async Task<G136Foto> GetFoto(int fotoId)
        { 
            var res = await _appDbContext.Fotos.FirstOrDefaultAsync(e => e.Id == fotoId);
            return res != null ? res : new G136Foto();
        }

        public async Task<IEnumerable<G136Foto>> GetFotos()
        {
            return await _appDbContext.Fotos.ToListAsync();
        }

        public async Task<G136Foto> UpdateFoto(G136Foto foto)
        {
            var res = await _appDbContext.Fotos.FirstOrDefaultAsync(e => e.Id == foto.Id);
            if (res != null)
            {
                if (foto.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Fecha = foto.Fecha;
                    res.Titulo = foto.Titulo;
                    res.PlayerId = foto.PlayerId;
                    res.Grupo = foto.Grupo;
                    res.Foto = foto.Foto;
                    res.Privada = foto.Privada;
                    res.Estado = foto.Estado;
                    res.Status = foto.Status;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G136Foto();
            }
            return res;
        }
    }
}

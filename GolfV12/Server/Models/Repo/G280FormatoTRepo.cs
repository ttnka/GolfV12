using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G280FormatoTRepo : IG280FormatoT
    {
        private readonly ApplicationDbContext _appDbContext;

        public G280FormatoTRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G280FormatoT> AddFormato(G280FormatoT formato)
        {
            var res = await _appDbContext.FormatosT.AddAsync(formato);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G280FormatoT>> Buscar(string? clave, string? titulo, 
            string? desc, bool individual)
        {
            IQueryable<G280FormatoT> querry = _appDbContext.FormatosT;
            if (!string.IsNullOrEmpty(clave)) querry = querry.Where(e => e.Clave.Contains(clave));
            if (!string.IsNullOrEmpty(titulo)) querry = querry.Where(e => e.Titulo.Contains(titulo));
            if (!string.IsNullOrEmpty(desc)) querry = querry.Where(e => e.Desc.Contains(desc));
            if (individual) querry = querry.Where(e => e.Individual == true);

            return await querry.ToListAsync();
        }

        public async Task<G280FormatoT> GetFormato(int formatoId)
        {
            var res = await _appDbContext.FormatosT.FirstOrDefaultAsync(e => e.Id == formatoId);
            return res != null ? res : new G280FormatoT();
        }

        public async Task<IEnumerable<G280FormatoT>> GetFormatos()
        {
            return await _appDbContext.FormatosT.ToListAsync();
        }

        public async Task<G280FormatoT> UpdateFormato(G280FormatoT formato)
        {
            var res = await _appDbContext.FormatosT.FirstOrDefaultAsync(e => e.Id == formato.Id);
            if (res != null)
            {
                if (formato.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Clave = formato.Clave;
                    res.Titulo = formato.Titulo;
                    res.Desc = formato.Desc;
                    res.Ronda = formato.Ronda;
                    res.Individual = formato.Individual;
                    res.Integrantes = formato.Integrantes;
                    res.Estado = formato.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G280FormatoT();
            }
            return res;
        }
    }
}

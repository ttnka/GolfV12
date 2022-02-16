using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G194CitaRepo : IG194Cita
    {
        private readonly ApplicationDbContext _appDbContet;

        public G194CitaRepo(ApplicationDbContext applicationDbContet)
        {
            this._appDbContet = applicationDbContet;
        }

        public async Task<G194Cita> AddCita(G194Cita cita)
        {
            var res = await _appDbContet.Citas.AddAsync(cita);
            await _appDbContet.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G194Cita>> Buscar(string? creador, string? desc, 
                                        int masterId, DateTime fini, DateTime ffin)
        {
            IQueryable<G194Cita> querry = _appDbContet.Citas;
            if (!string.IsNullOrEmpty(creador)) querry = querry.Where(e => e.Creador == creador);
            if (!string.IsNullOrEmpty(desc)) querry = querry.Where(e => e.Desc.Contains(desc));
            if (masterId > 0) querry = querry.Where(e => e.MasterId == masterId);
            if (fini > DateTime.MinValue) querry = querry.Where(e => e.FIni.Date > fini.Date);
            if(ffin > DateTime.MinValue) querry = querry.Where(e => e.FFin.Date > ffin.Date);

            return await querry.ToListAsync();
        }

        public async Task<G194Cita> GetCita(int citaId)
        {
            var res = await _appDbContet.Citas.FirstOrDefaultAsync(e => e.Id == citaId);
            return res != null ? res : new G194Cita();
        }

        public async Task<IEnumerable<G194Cita>> GetCitas()
        {
            return await _appDbContet.Citas.ToListAsync();
        }

        public async Task<G194Cita> UpdateCita(G194Cita cita)
        {
            var res = await _appDbContet.Citas.FirstOrDefaultAsync(e => e.Id == cita.Id);
            if (res != null)
            {
                if (cita.Status == false)
                {
                    res.Status = false;
                } else
                {
                    res.FIni = cita.FIni;
                    res.FFin = cita.FFin;
                    res.Desc = cita.Desc;
                    res.Creador = cita.Creador;
                    res.MasterId = cita.MasterId;
                    res.Estado = cita.Estado;
                    res.Status = cita.Status;
                }
                await _appDbContet.SaveChangesAsync();
            } else
            {
                res = new G194Cita();
            }
            return res;
        }
    }
}

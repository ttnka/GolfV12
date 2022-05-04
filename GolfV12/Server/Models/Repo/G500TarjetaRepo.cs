using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G500TarjetaRepo : IG500Tarjeta
    {
        private readonly ApplicationDbContext _appDbContext;

        public G500TarjetaRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G500Tarjeta> AddTarjeta(G500Tarjeta tarjeta)
        {
            var res = await _appDbContext.Tarjetas.AddAsync(tarjeta);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G500Tarjeta>> Filtro(string? clave)
        {
            // clave = org1
            // ejeplo = organizaciones?clave=org1_-_nombre_-_ivan_-_desc_-_conocido
            IQueryable<G500Tarjeta> querry = _appDbContext.Tarjetas;
            if (string.IsNullOrWhiteSpace(clave)) return await querry.ToListAsync();

            string[] parametros = clave.Split("_-_");
            string Data1 = string.Empty;
            string Data2 = string.Empty;

            if (parametros[0] == "tar1")
            {
                for (int i = 1; i < parametros.Length; i++)
                {
                    Data1 = parametros[i];
                    Data2 = parametros[i + 1];
                    switch (Data1)
                    {
                        case "id":
                            querry = querry.Where(e => e.Id.Contains(Data2));
                            break;
                        case "creador":
                            querry = querry.Where(e => e.Creador.Contains(Data2));
                            break;
                        case "fecha":
                            querry = querry.Where(e => e.Fecha.Equals(Data2));
                            break;
                        case "campo":
                            querry = querry.Where(e => e.Campo == int.Parse(Data2));
                            break;
                        case "Titulo":
                            querry = querry.Where(e => e.Titulo.Contains(Data2));
                            break;
                        case "Estado":
                            querry = querry.Where(e => e.Estado == int.Parse(Data2));
                            break;
                        case "Status":
                            querry = querry.Where(e => e.Status.Equals(Data2));
                            break;
                    }
                    return await querry.ToListAsync();
                }
            }
            return await querry.ToListAsync();
        }

        public async Task<G500Tarjeta> UpdateTarjeta(G500Tarjeta tarjeta)
        {
            var res = await _appDbContext.Tarjetas.FirstOrDefaultAsync(e => e.Id == tarjeta.Id);
            if (res != null)
            {
                if (tarjeta.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Fecha = tarjeta.Fecha;
                    res.Creador = tarjeta.Creador;
                    res.Campo = tarjeta.Campo;
                    res.Titulo = tarjeta.Titulo;
                    res.Captura = tarjeta.Captura;
                    res.Consulta = tarjeta.Consulta;
                    res.Estado = tarjeta.Estado;
                    res.Status = tarjeta.Status;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G500Tarjeta();
            }
            return res;
                
        }
    }
}

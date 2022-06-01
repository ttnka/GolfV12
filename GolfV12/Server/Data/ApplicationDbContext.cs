using Duende.IdentityServer.EntityFramework.Options;
using GolfV12.Server.Models;
using GolfV12.Shared;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GolfV12.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        // Aqui van DBSET
        public DbSet<G110Organizacion> Organizaciones { get; set; }
        public DbSet<G120Player> Players { get; set; }
        public DbSet<G128Hcp> Hcps { get; set; }
        public DbSet<G136Foto> Fotos { get; set; }
        public DbSet<G170Campo> Campos { get; set; }
        public DbSet<G172Bandera> Banderas { get; set; }
        public DbSet<G176Hoyo> Hoyos { get; set; }
        public DbSet<G178Distancia> Distancias { get; set; }
        public DbSet<G180Estado> Estados { get; set; }
        public DbSet<G190Bitacora> Bitacoras { get; set; }
        public DbSet<G194Cita> Citas { get; set; }
        public DbSet<G200Torneo> Torneos { get; set; }
        public DbSet<G202JobT> Jobs { get; set; }
        public DbSet<G204FechaT> FechasT { get; set; }
        public DbSet<G208CategoriaT> CategoriasT { get; set; }
        public DbSet<G220TeamT> TeamsT { get; set; }
        public DbSet<G222PlayerT> PlayersT { get; set; }
        public DbSet<G224RolT> RolsT { get; set; }
        public DbSet<G240Score> Scores { get; set; }
        public DbSet<G242Extras> Extras { get; set; }
        public DbSet<G249TiroEstadistica> TirosEst { get; set; }
        public DbSet<G250ExtrasTipo> ExtrasTipos { get; set; }
        public DbSet<G280FormatoT> FormatosT { get; set; }

        public DbSet<G300Azar> Azar { get; set; }
        public DbSet<G310Participantes> Participantes { get; set; }
        public DbSet<G320Bolitas> Bolitas { get; set; }
        public DbSet<G324Parejas> Parejas { get; set; }
        public DbSet<G330Loba> Loba { get; set; }
        public DbSet<G332LobaDet> LobaDet { get; set; }
        public DbSet<G390TiposAzar> TiposAzar { get; set; }

        public DbSet<G500Tarjeta> Tarjetas { get; set; }
        public DbSet<G502Tarjetas> Tarjetas502 { get; set; }
        public DbSet<G510Jugador> Jugadores { get; set; }
        public DbSet<G520Score> MyScore { get; set; }
        public DbSet<G522Scores> MyScore522 { get; set; }
        /*
        public DbSet<> MyProperty { get; set; }
        public DbSet<> MyProperty { get; set; }
        public DbSet<> MyProperty { get; set; }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}
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
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}
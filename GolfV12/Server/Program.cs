using GolfV12.Server.Data;
using GolfV12.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GolfV12.Server.Models.IFace;
using GolfV12.Server.Models.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddScoped<IG110Organizacion,   G110OrganizacionRepo>();
builder.Services.AddScoped<IG120Player,         G120PlayerRepo>();
builder.Services.AddScoped<IG121ElPlayer,       G121ElPlayerRepo>();
builder.Services.AddScoped<IG128Hcp,            G128HcpRepo>();
builder.Services.AddScoped<IG136Foto,           G136FotoRepo>();
builder.Services.AddScoped<IG170Campo,          G170campoRepo>();
builder.Services.AddScoped<IG172Bandera,        G172BanderaRepo>();
builder.Services.AddScoped<IG176Hoyo,           G176HoyoRepo>();
builder.Services.AddScoped<IG178Distancia,      G178DistanciaRepo>();
builder.Services.AddScoped<IG180Estado,         G180EstadoRepo>();
builder.Services.AddScoped<IG190Bitacora,       G190BitacoraRepo>(); 
builder.Services.AddScoped<IG194Cita,           G194CitaRepo>();
builder.Services.AddScoped<IG200Torneo,         G200TorneoRepo>();
builder.Services.AddScoped<IG202JobT,           G202JobTRepo>();
builder.Services.AddScoped<IG204FechaT,         G204FechaTRepo>();
builder.Services.AddScoped<IG208CategoriaT,     G208CategoriaTRepo>();
builder.Services.AddScoped<IG220TeamT,          G220TeamTRepo>();
builder.Services.AddScoped<IG222PlayerT,        G222PlayerTRepo>();
builder.Services.AddScoped<IG224RolT,           G224RolTRepo>();
builder.Services.AddScoped<IG242Extras,         G242ExtrasRepo>();
builder.Services.AddScoped<IG249TiroEst,        G249TiroEstRepo>();
builder.Services.AddScoped<IG250ExtrasTipo,     G250ExtrasTipoRepo>();
builder.Services.AddScoped<IG280FormatoT,       G280FormatoTRepo>();



builder.Services.AddScoped<IG500Tarjeta, G500TarjetaRepo>();
builder.Services.AddScoped<IG502Tarjetas, G502TarjetasRepo>();
builder.Services.AddScoped<IG510Jugador, G510JugadorRepo>();
builder.Services.AddScoped<IG520Score, G520ScoreRepo>();
builder.Services.AddScoped<IG522Scores, G522ScoresRepo>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

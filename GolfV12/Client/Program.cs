using GolfV12.Client;
using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Client.Servicios.Serv;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("GolfV12.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GolfV12.ServerAPI"));

// Servicios HTTP
builder.Services.AddHttpClient<IG110OrganizacionServ, G110OrganizacionServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG120PlayerServ, G120PlayerServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG121ElPlayerServ, G121ElPlayerServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG128HcpServ, G128HcpServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG170CampoServ, G170CampoServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG172BanderaServ, G172BanderaServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG176HoyoServ, G176HoyoServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG178DistanciaServ, G178DistanciaServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG180EstadoServ, G180EstadoServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddHttpClient<IG190BitacoraServ, G190BitacoraServ>(client =>
{ client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });


builder.Services.AddApiAuthorization();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

// Servicios REDZAD
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

await builder.Build().RunAsync();

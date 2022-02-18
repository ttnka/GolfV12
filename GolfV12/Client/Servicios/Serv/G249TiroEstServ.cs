using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G249TiroEstServ : IG249TiroEstServ
    {
        private readonly HttpClient _httpClient;

        public G249TiroEstServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G249TiroEstadistica> AddTiroEst(G249TiroEstadistica tiroEst)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G249TiroEstadistica>("/api/G249TiroEstadistica", tiroEst);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G249TiroEstadistica>() :
                null;
        }

        public async Task<IEnumerable<G249TiroEstadistica>> Buscar(int rol, string? player, int hoyo, 
            TiroTipo? tiroTipo)
        {
            var resultado = "";
            if (rol > 0) { resultado = "rol=" + rol + "&"; }
            if (!string.IsNullOrEmpty(player)) { resultado = resultado + "player=" + player + "&"; }
            if (hoyo > 0) { resultado = resultado + "hoyo=" + hoyo + "&"; }
            if (tiroTipo.Value >=0 ) { resultado = resultado + "tirotipo=" + tiroTipo.Value + "&"; }
            if (resultado != "") { resultado = "/api/G249TiroEstadistica/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G249TiroEstadistica>>(resultado);
        }

        public async Task<G249TiroEstadistica> GetTiroEst(int tiroEstId)
        {
            return await _httpClient.GetFromJsonAsync<G249TiroEstadistica>($"/api/G249TiroEstadistica/{tiroEstId}");

        }

        public async Task<IEnumerable<G249TiroEstadistica>> GetTiroEsts()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G249TiroEstadistica>>("/api/G249TiroEstadistica/");

        }

        public async Task<G249TiroEstadistica> UpdateTiroEst(G249TiroEstadistica tiroEst)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G249TiroEstadistica>("/api/G249TiroEstadistica/", tiroEst);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G249TiroEstadistica>() : null;
        }
    }
}

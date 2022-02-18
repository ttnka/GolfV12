using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G200TorneoServ : IG200TorneoServ
    {
        private readonly HttpClient _httpClient;

        public G200TorneoServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G200Torneo> AddTorneo(G200Torneo torneo)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G200Torneo>("/api/G200Torneo", torneo);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G200Torneo>() :
                null;
        }

        public async Task<IEnumerable<G200Torneo>> Buscar(int ejercicio, string? titulo, string? creador)
        {
            var resultado = "";
            if (ejercicio > 0) { resultado = resultado + "ejercicio=" + ejercicio + "&"; }
            if (!string.IsNullOrEmpty(titulo)) { resultado = resultado + "titulo=" + titulo + "&"; }
            if (!string.IsNullOrEmpty(creador)) { resultado = resultado + "creador=" + creador + "&"; }
            if (resultado != "") { resultado = "/api/G200Torneo/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G200Torneo>>(resultado);
        }

        public async Task<G200Torneo> GetTorneo(int torneoId)
        {
            return await _httpClient.GetFromJsonAsync<G200Torneo>($"/api/G200Torneo/{torneoId}");
        }

        public async Task<IEnumerable<G200Torneo>> GetTorneos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G200Torneo>>("/api/G200Torneo/");
        }

        public async Task<G200Torneo> UpdateTorneo(G200Torneo torneo)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G200Torneo>("/api/G200Torneo/", torneo);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G200Torneo>() : null;
        }
    }
}

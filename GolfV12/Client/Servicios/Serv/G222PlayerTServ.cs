using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G222PlayerTServ : IG222PlayerTServ
    {
        private readonly HttpClient _httpClient;

        public G222PlayerTServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G222PlayerT> AddPlayer(G222PlayerT player)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G222PlayerT>("/api/G222PlayerT", player);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G222PlayerT>() :
                null;
        }

        public async Task<IEnumerable<G222PlayerT>> Buscar(int team, string? player)
        {
            var resultado = "";
            if (team > 0) { resultado = "team=" + team + "&"; }
            if (!string.IsNullOrEmpty(player)) { resultado = resultado + "player=" + player ; }
//            if (!string.IsNullOrEmpty(desc)) { resultado = resultado + "desc=" + desc + "&"; }
            if (resultado != "") { resultado = "/api/G222PlayerT/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G222PlayerT>>(resultado);
        }

        public async Task<G222PlayerT> GetPlayer(int playerId)
        {
            return await _httpClient.GetFromJsonAsync<G222PlayerT>($"/api/G222PlayerT/{playerId}");
        }

        public async Task<IEnumerable<G222PlayerT>> GetPlayers()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G222PlayerT>>("/api/G222PlayerT/");
        }

        public async Task<G222PlayerT> UpdatePlayer(G222PlayerT player)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G222PlayerT>("/api/G222PlayerT/", player);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G222PlayerT>() : null;
        }
    }
}

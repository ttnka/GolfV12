using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G242ExtrasServ : IG242ExtrasServ
    {
        private readonly HttpClient _httpClient;

        public G242ExtrasServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G242Extras> AddExtra(G242Extras extra)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G242Extras>("/api/G242Extras", extra);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G242Extras>() :
                null;
        }

        public async Task<IEnumerable<G242Extras>> Buscar(int rol, string player, int hoyo, int tipoExtra)
        {
            var resultado = "";
            if (rol > 0) { resultado = "rol=" + rol + "&"; }
            if (!string.IsNullOrEmpty(player)) { resultado = resultado + "player=" + player + "&"; }
            if (hoyo > 0) { resultado = resultado + "hoyo=" + hoyo + "&"; }
            if (tipoExtra > 0) { resultado = resultado + "tipoextra=" + tipoExtra + "&"; }
            if (resultado != "") { resultado = "/api/G242Extras/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G242Extras>>(resultado);
        }

        public async Task<G242Extras> GetExtra(int extraId)
        {
            return await _httpClient.GetFromJsonAsync<G242Extras>($"/api/G242Extras/{extraId}");

        }

        public async Task<IEnumerable<G242Extras>> GetExtras()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G242Extras>>("/api/G242Extras/");
        }

        public async Task<G242Extras> UpdateExtra(G242Extras extra)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G242Extras>("/api/G242Extras/", extra);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G242Extras>() : null;
        }
    }
}

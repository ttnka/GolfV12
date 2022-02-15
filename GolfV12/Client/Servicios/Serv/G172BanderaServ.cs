using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G172BanderaServ : IG172BanderaServ
    {
        private readonly HttpClient _httpClient;

        public G172BanderaServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G172Bandera> AddBandera(G172Bandera bandera)
        {
            var newBandera = await _httpClient.PostAsJsonAsync<G172Bandera>("/api/G172Bandera", bandera);
            return newBandera.IsSuccessStatusCode ?
                await newBandera.Content.ReadFromJsonAsync<G172Bandera>() :
                null;
        }

        public async Task<IEnumerable<G172Bandera>> Buscar(int campo, string? color)
        {
            var resultado = "";
            if (!string.IsNullOrEmpty(color)) { resultado = "color=" + color + "&"; }
            if (campo > 0) { resultado = resultado + "campo=" + campo; }
            if (resultado != "") { resultado = "/api/G172Bandera/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G172Bandera>>(resultado);
        }

        public async Task<G172Bandera> GetBandera(int banderaId)
        {
            return await _httpClient.GetFromJsonAsync<G172Bandera>($"/api/G172Bandera/{banderaId}");
        }

        public async Task<IEnumerable<G172Bandera>> GetBanderas()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G172Bandera>>("/api/G172Bandera/");
        }

        public async Task<G172Bandera> UpdateBandera(G172Bandera bandera)
        {
            var newBandera = await _httpClient.PutAsJsonAsync<G172Bandera>("/api/G172Bandera", bandera);
            return newBandera.IsSuccessStatusCode ?
                await newBandera.Content.ReadFromJsonAsync<G172Bandera>() : null;
        }
    }
}

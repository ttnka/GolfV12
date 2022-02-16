using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G176HoyoServ : IG176HoyoServ
    {
        private readonly HttpClient _httpClient;

        public G176HoyoServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G176Hoyo> AddHoyo(G176Hoyo hoyo)
        {
            var newHoyo = await _httpClient.PostAsJsonAsync<G176Hoyo>("/api/G176Hoyo", hoyo);
            return newHoyo.IsSuccessStatusCode ?
                await newHoyo.Content.ReadFromJsonAsync<G176Hoyo>() :
                null;
        }

        public async Task<IEnumerable<G176Hoyo>> Buscar(int campo, string? ruta, int hoyoN)
        {
            var resultado = "";
            if (campo > 0) { resultado = "campo=" + campo + "&"; }
            if (!string.IsNullOrEmpty(ruta)) { resultado = resultado + "ruta=" + ruta + "&"; }
            if (hoyoN > 0) { resultado = resultado + "hoyoN=" + hoyoN + "&"; }
            if (resultado != "") { resultado = "/api/G176Hoyo/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G176Hoyo>>(resultado);
        }

        public async Task<G176Hoyo> GetHoyo(int hoyoId)
        {
            return await _httpClient.GetFromJsonAsync<G176Hoyo>($"/api/G176Hoyo/{hoyoId}");
        }

        public async Task<IEnumerable<G176Hoyo>> GetHoyos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G176Hoyo>>("/api/G176Hoyo/");

        }

        public async Task<G176Hoyo> UpdateHoyo(G176Hoyo hoyo)
        {
            var newHoyo = await _httpClient.PutAsJsonAsync<G176Hoyo>("/api/G176Hoyo", hoyo);
            return newHoyo.IsSuccessStatusCode ?
                await newHoyo.Content.ReadFromJsonAsync<G176Hoyo>() : null;
        }
    }
}

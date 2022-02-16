using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G178DistanciaServ : IG178DistanciaServ
    {
        private readonly HttpClient _httpClient;

        public G178DistanciaServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<G178Distancia> AddDistancia(G178Distancia distancia)
        {
            var newDist = await _httpClient.PostAsJsonAsync<G178Distancia>("/api/G178Distancia/", distancia);
            return newDist.IsSuccessStatusCode ?
                await newDist.Content.ReadFromJsonAsync<G178Distancia>() :
                null;
        }

        public async Task<IEnumerable<G178Distancia>> Buscar(int bandera, int hoyoN)
        {
            var resultado = "";
            if (bandera > 0) { resultado = "bandera=" + bandera + "&"; }
            if (hoyoN > 0) { resultado = resultado + "hoyoN=" + hoyoN + "&"; }
            if (resultado != "") { resultado = "/api/G178Distancia/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G178Distancia>>(resultado);
        }

        public async Task<G178Distancia> GetDistancia(int distanciaId)
        {
            return await _httpClient.GetFromJsonAsync<G178Distancia>($"/api/G178Distancia/{distanciaId}");
        }

        public async Task<IEnumerable<G178Distancia>> GetDistancias()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G178Distancia>>("/api/G178Distancia/");
        }

        public async Task<G178Distancia> UpdateDistancia(G178Distancia distancia)
        {
            var newDist = await _httpClient.PutAsJsonAsync<G178Distancia>("/api/G178Distancia", distancia);
            return newDist.IsSuccessStatusCode ?
                await newDist.Content.ReadFromJsonAsync<G178Distancia>() : null;
        }
    }
}

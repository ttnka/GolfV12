using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G170CampoServ : IG170CampoServ
    {
        private readonly HttpClient _httpClient;

        public G170CampoServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<G170Campo> AddCampo(G170Campo campo)
        {
            var newCampo = await _httpClient.PostAsJsonAsync<G170Campo>("/api/G170Campo/", campo);
            return newCampo.IsSuccessStatusCode ?
                await newCampo.Content.ReadFromJsonAsync<G170Campo>() :
                null;
        }

        public async Task<IEnumerable<G170Campo>> Buscar(string? corto, string? nombre, 
            string? ciudad, string? pais)
        {
            var resultado = "";
            if (!string.IsNullOrEmpty(corto)) { resultado = "corto=" + corto + "&"; }
            if (!string.IsNullOrEmpty(nombre)) { resultado = resultado + "nombre=" + nombre + "&"; }
            if (!string.IsNullOrEmpty(ciudad)) { resultado = resultado + "ciudad=" + ciudad + "&"; }
            if (!string.IsNullOrEmpty(pais)) { resultado = resultado + "pais=" + ciudad + "&"; }
            if (resultado != "") { resultado = "/api/G170Campo/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G170Campo>>(resultado);
        }

        public async Task<G170Campo> GetCampo(int campoId)
        {
            return await _httpClient.GetFromJsonAsync<G170Campo>($"/api/G170Campo/{campoId}");
        }

        public async Task<IEnumerable<G170Campo>> GetCampos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G170Campo>>($"/api/G170Campo/");
        }

        public async Task<G170Campo> UpdateCampo(G170Campo campo)
        {
            var newCampo = await _httpClient.PutAsJsonAsync <G170Campo>("/api/G170Campo", campo);
            return newCampo.IsSuccessStatusCode ?
                await newCampo.Content.ReadFromJsonAsync<G170Campo>() : null;
        }
    }
}

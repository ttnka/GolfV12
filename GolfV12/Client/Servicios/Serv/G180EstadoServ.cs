using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G180EstadoServ : IG180EstadoServ
    {
        private readonly HttpClient _httpClient;

        public G180EstadoServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<G180Estado> AddEstado(G180Estado estado)
        {
            var newEstado = await _httpClient.PostAsJsonAsync<G180Estado>("/api/G180Estado", estado);
            return newEstado.IsSuccessStatusCode ?
                await newEstado.Content.ReadFromJsonAsync<G180Estado>() :
                null;
        }

        public async Task<IEnumerable<G180Estado>> Buscar(string titulo, string grupo)
        {
            var resultado = "";
            if (!string.IsNullOrEmpty(titulo))  resultado = resultado + "titulo=" + titulo + "&"; 
            if (!string.IsNullOrEmpty(grupo))  resultado = resultado + "grupo=" + grupo + "&"; 
            
            if (resultado != "") { resultado = "/api/G180estado/filtro/" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G180Estado>>(resultado);
        }

        public async Task<G180Estado> GetEstado(int estadoId)
        {
            return await _httpClient.GetFromJsonAsync<G180Estado>($"/api/G180Estado/{estadoId}");
        }

        public async Task<IEnumerable<G180Estado>> GetEstados()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G180Estado>>($"/api/G180Estado");
        }

        public async Task<G180Estado> UpdateEstado(G180Estado estado)
        {
            var newEstado = await _httpClient.PutAsJsonAsync<G180Estado>("/api/G180estado", estado);
            return newEstado.IsSuccessStatusCode ?
                await newEstado.Content.ReadFromJsonAsync<G180Estado>() : null;
        }
    }
}

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G280FormatoTServ : IG280FormatoTServ
    {
        private readonly HttpClient _httpClient;

        public G280FormatoTServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G280FormatoT> AddFormato(G280FormatoT formato)
        {
            var newFormato = await _httpClient.PostAsJsonAsync<G280FormatoT>("/api/G280FormatoT", formato);
            return newFormato.IsSuccessStatusCode ?
                await newFormato.Content.ReadFromJsonAsync<G280FormatoT>() :
                null;
        }

        public async Task<IEnumerable<G280FormatoT>> Buscar(string? clave, string? titulo, 
            string? desc, bool individual)
        {
            var resultado = "";
            if (!string.IsNullOrEmpty(clave)) { resultado = resultado + "clave=" + clave + "&"; }
            if (!string.IsNullOrEmpty(titulo)) { resultado = resultado + "titulo=" + titulo + "&"; }
            if (!string.IsNullOrEmpty(desc)) { resultado = resultado + "desc=" + desc + "&"; }
            if (resultado != "") { resultado = "/api/G280FormatoT/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G280FormatoT>>(resultado);
        }

        public async Task<G280FormatoT> GetFormato(int formatoId)
        {
            return await _httpClient.GetFromJsonAsync<G280FormatoT>($"/api/G280FormatoT/{formatoId}");
        }

        public async Task<IEnumerable<G280FormatoT>> GetFormatos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G280FormatoT>>("/api/G280FormatoT/");
        }

        public async Task<G280FormatoT> UpdateFormato(G280FormatoT formato)
        {
            var newFormato = await _httpClient.PutAsJsonAsync<G280FormatoT>("/api/G280FormatoT/", formato);
            return newFormato.IsSuccessStatusCode ?
                await newFormato.Content.ReadFromJsonAsync<G280FormatoT>() : null;
        }
    }
}

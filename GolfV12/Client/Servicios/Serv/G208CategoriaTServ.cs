using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G208CategoriaTServ : IG208CategoriaTServ
    {
        private readonly HttpClient _httpClient;

        public G208CategoriaTServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G208CategoriaT> AddCategoria(G208CategoriaT categoria)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G208CategoriaT>("/api/G208CategoriaT", categoria);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G208CategoriaT>() :
                null;
        }

        public async Task<IEnumerable<G208CategoriaT>> Buscar(int torneo, string? titulo)
        {
            var resultado = "";
            if (torneo>0) { resultado = resultado + "torneo=" + torneo + "&"; }
            if (!string.IsNullOrEmpty(titulo)) { resultado = resultado + "titulo=" + titulo + "&"; }
//            if (!string.IsNullOrEmpty(desc)) { resultado = resultado + "desc=" + desc + "&"; }
            if (resultado != "") { resultado = "/api/G208CategoriaT/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G208CategoriaT>>(resultado);
        }

        public async Task<G208CategoriaT> GetCategoria(int categoriaId)
        {
            return await _httpClient.GetFromJsonAsync<G208CategoriaT>($"/api/G208CategoriaT/{categoriaId}");
        }

        public async Task<IEnumerable<G208CategoriaT>> GetCategorias()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G208CategoriaT>>("/api/G208CategoriaT/");
        }

        public async Task<G208CategoriaT> UpdateCategoria(G208CategoriaT categoria)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G208CategoriaT>("/api/G208CategoriaT/", categoria);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G208CategoriaT>() : null;
        }
    }
}

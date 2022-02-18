using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G250ExtrasTipoServ : IG250ExtrasTipoServ
    {
        private readonly HttpClient _httpClient;

        public G250ExtrasTipoServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G250ExtrasTipo> AddExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            var newExtraT = await _httpClient.PostAsJsonAsync<G250ExtrasTipo>("/api/G250ExtrasTipo", extrasTipo);
            return newExtraT.IsSuccessStatusCode ?
                await newExtraT.Content.ReadFromJsonAsync<G250ExtrasTipo>() :
                null;
        }

        public async Task<IEnumerable<G250ExtrasTipo>> Buscar(string? titulo, string? creador, 
            string? grupo, bool publico)
        {
            var resultado = "";
            if (!string.IsNullOrEmpty(titulo)) { resultado = resultado + "titulo=" + titulo + "&"; }
            if (!string.IsNullOrEmpty(creador)) { resultado = resultado + "creador=" + creador + "&"; }
            if (!string.IsNullOrEmpty(grupo)) { resultado = resultado + "grupo=" + grupo + "&"; }
            if (publico == true) { resultado = resultado + "publico=true"; }
            if (resultado != "") { resultado = "/api/G250ExtrasTipo/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G250ExtrasTipo>>(resultado);
        }

        public async Task<G250ExtrasTipo> GetExtrasTipo(int extrasTipoId)
        {
            return await _httpClient.GetFromJsonAsync<G250ExtrasTipo>($"/api/G250ExtrasTipo/{extrasTipoId}");
        }

        public async Task<IEnumerable<G250ExtrasTipo>> GetExtrasTipos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G250ExtrasTipo>>("/api/G250ExtrasTipo/");
        }

        public async Task<G250ExtrasTipo> UpdateExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G250ExtrasTipo>("/api/G250ExtrasTipo/", extrasTipo);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G250ExtrasTipo>() : null;
        }
    }
}

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G224RolTServ : IG224RolTServ
    {
        private readonly HttpClient _httpClient;

        public G224RolTServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G224RolT> AddRol(G224RolT rol)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G224RolT>("/api/G224RolT", rol);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G224RolT>() :
                null;
        }

        public async Task<IEnumerable<G224RolT>> Buscar(int torneo)
        {
            var resultado = "";
            if (torneo > 0) { resultado = "torneo=" + torneo + "&"; }
//            if (!string.IsNullOrEmpty(nombre)) { resultado = resultado + "nombre=" + nombre + "&"; }
//            if (!string.IsNullOrEmpty(desc)) { resultado = resultado + "desc=" + desc + "&"; }
            if (resultado != "") { resultado = "/api/G224RolT/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G224RolT>>(resultado);
        }

        public async Task<G224RolT> GetRol(int rolId)
        {
            return await _httpClient.GetFromJsonAsync<G224RolT>($"/api/G224RolT/{rolId}");
        }

        public async Task<IEnumerable<G224RolT>> GetRoles()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G224RolT>>("/api/G224RolT/");
        }

        public async Task<G224RolT> UpdateRol(G224RolT rol)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G224RolT>("/api/G224RolT/", rol);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G224RolT>() : null;
        }
    }
}

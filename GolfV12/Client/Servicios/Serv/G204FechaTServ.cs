using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G204FechaTServ : IG204FechaTServ
    {
        private readonly HttpClient _httpClient;

        public G204FechaTServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G204FechaT> AddFechaT(G204FechaT fechaT)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G204FechaT>("/api/G204FechaT", fechaT);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G204FechaT>() :
                null;
        }

        public async Task<IEnumerable<G204FechaT>> Buscar(int torneo, DateTime fecha)
        {
            var resultado = "";
            if (torneo>0) { resultado = resultado + "torneo=" + torneo + "&"; }
            if (fecha > DateTime.MinValue) { resultado = resultado + "fecha=" + fecha + "&"; }
 //           if (!string.IsNullOrEmpty(desc)) { resultado = resultado + "desc=" + desc + "&"; }
            if (resultado != "") { resultado = "/api/G204FechaT/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G204FechaT>>(resultado);
        }

        public async Task<IEnumerable<G204FechaT>> GetFechasT()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G204FechaT>>("/api/G204FechaT/");
        }

        public async Task<G204FechaT> GetFechaT(int fechaTId)
        {
            return await _httpClient.GetFromJsonAsync<G204FechaT>($"/api/G204FechaT/{fechaTId}");
        }

        public async Task<G204FechaT> UpdateFechaT(G204FechaT fechaT)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G204FechaT>("/api/G204FechaT/", fechaT);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G204FechaT>() : null;
        }
    }
}

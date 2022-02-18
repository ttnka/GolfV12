using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G202JobTServ : IG202JobTServ
    {
        private readonly HttpClient _httpClient;

        public G202JobTServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G202JobT> AddJob(G202JobT job)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G202JobT>("/api/G202JobT", job);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G202JobT>() :
                null;
        }

        public async Task<IEnumerable<G202JobT>> Buscar(int torneo, string? player, string? contrincante)
        {
            var resultado = "";
            if (torneo > 0) { resultado = resultado + "torneo=" + torneo + "&"; }
            if (!string.IsNullOrEmpty(player)) { resultado = resultado + "player=" + player + "&"; }
            if (!string.IsNullOrEmpty(contrincante)) { resultado = resultado + "contrincate=" + contrincante + "&"; }
            if (resultado != "") { resultado = "/api/G202JobT/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G202JobT>>(resultado);

        }
        public async Task<G202JobT> GetJob(int jobId)
        {
            return await _httpClient.GetFromJsonAsync<G202JobT>($"/api/G202JobT/{jobId}");

        }

        public async Task<IEnumerable<G202JobT>> GetJobs()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G202JobT>>("/api/G202JobT/");

        }

        public async Task<G202JobT> UpdateJob(G202JobT job)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G202JobT>("/api/G202JobT/", job);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G202JobT>() : null;
        }
    }
}

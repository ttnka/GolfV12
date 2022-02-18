using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G240ScoreServ : IG240ScoreServ
    {
        private readonly HttpClient _httpClient;

        public G240ScoreServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G240Score> AddScore(G240Score score)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G240Score>("/api/G240Score", score);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G240Score>() :
                null;
        }

        public async Task<IEnumerable<G240Score>> Buscar(int rol, string? player, int hoyo)
        {
            var resultado = "";
            if (rol > 0) { resultado = "rol=" + rol + "&"; }
            if (!string.IsNullOrEmpty(player)) { resultado = resultado + "player=" + player + "&"; }
            if (hoyo > 0) { resultado = resultado + "hoyo=" + hoyo + "&"; }
            if (resultado != "") { resultado = "/api/G240Score/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G240Score>>(resultado);
        }

        public async Task<G240Score> GetScore(int scoreId)
        {
            return await _httpClient.GetFromJsonAsync<G240Score>($"/api/G240Score/{scoreId}");

        }

        public async Task<IEnumerable<G240Score>> GetScores()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G240Score>>("/api/G240Score/");

        }

        public async Task<G240Score> UpdateScore(G240Score score)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G240Score>("/api/G240Score/", score);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G240Score>() : null;
        }
    }
}

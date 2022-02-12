using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G128HcpServ : IG128HcpServ
    {
        private readonly HttpClient _httpClient;

        public G128HcpServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<G128Hcp> AddHcp(G128Hcp hcp)
        {
            var newHcp = await _httpClient.PostAsJsonAsync<G128Hcp>("/api/G128Hcp/", hcp);
            return newHcp.IsSuccessStatusCode ?
                await newHcp.Content.ReadFromJsonAsync<G128Hcp>() :
                null;
        }

        public async Task<IEnumerable<G128Hcp>> Buscar(string playerId)
        {
            //var resultado = "";

            //if (!string.IsNullOrEmpty(playerId)) { resultado = "playerId=" + playerId; }

            //if (resultado != "") { resultado = "/api/G128Hcp/filtro/" + resultado; }
            
            //return await _httpClient.GetFromJsonAsync<IEnumerable<G128Hcp>>(resultado);
            var res2 = await _httpClient.GetFromJsonAsync<IEnumerable<G128Hcp>>($"/api/G128Hcp/filtro?playerid={playerId}");
            return res2.ToList();

            //return await _httpClient.GetFromJsonAsync<IEnumerable<G128Hcp>>($"/api/G128Hcp/filtro/{playerId}");
        }

        public async Task<G128Hcp> GetHcp(int hcpId)
        {
            return await _httpClient.GetFromJsonAsync<G128Hcp>($"/api/G128Hcp/{hcpId}");
        }

        public async Task<IEnumerable<G128Hcp>> GetHcps()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G128Hcp>>("/api/G128Hcp/");
        }

        public async Task<G128Hcp> UpdateHcp(G128Hcp hcp)
        {
            var newHcp = await _httpClient.PutAsJsonAsync<G128Hcp>("/api/G128Hcp/", hcp);
            return newHcp.IsSuccessStatusCode ?
                await newHcp.Content.ReadFromJsonAsync<G128Hcp>() : null;
        }
    }
}

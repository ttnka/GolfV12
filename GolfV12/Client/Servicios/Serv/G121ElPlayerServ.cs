using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G121ElPlayerServ : IG121ElPlayerServ
    {
        private readonly HttpClient _httpClient;

        public G121ElPlayerServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G120Player> GetPlayer(string userId)
        {
            return await _httpClient.GetFromJsonAsync<G120Player>($"/api/G121elPlayer/{userId}");
        }
    }
}

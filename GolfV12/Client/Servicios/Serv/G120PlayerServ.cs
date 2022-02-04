using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G120PlayerServ : IG120PlayerServ
    {
        private readonly HttpClient _httpClient;

        public G120PlayerServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<G120Player> AddPlayer(G120Player player)
        {
            var newPlayer = await _httpClient.PostAsJsonAsync<G120Player>("/api/G120player", player);
            return newPlayer.IsSuccessStatusCode ? await newPlayer.Content.ReadFromJsonAsync<G120Player>() :
             null;
        }
        /*
        public async Task<IEnumerable<G120Player>> Buscar(string? userId, int org, string? apodo, string? nombre, string? paterno)
        {
            var resultado = "";
            if (!string.IsNullOrEmpty(userId)) resultado = "userId=" + userId + "&";
            if (org > -1)  resultado = resultado + "org=" + org + "&"; 
            if (!string.IsNullOrEmpty(apodo)) { resultado = resultado + "apodo=" + apodo + "&"; }
            if (!string.IsNullOrEmpty(nombre)) { resultado = resultado + "nombre=" + nombre + "&"; }
            if (!string.IsNullOrEmpty(paterno)) { resultado = resultado + "paterno=" + paterno + "&"; }
            if (resultado != "") { resultado = "/api/G120player/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G120Player>>(resultado);
        }

        */
        public async Task<G120Player> GetPlayer(string userId)
        {
            return await _httpClient.GetFromJsonAsync<G120Player>($"/api/G120player/{userId}");
        }
        
        public async Task<IEnumerable<G120Player>> GetPlayers()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G120Player>>("/api/G120player/");
        }

        public async Task<G120Player> UpdatePlayer(G120Player player)
        {
            var newPlayer = await _httpClient.PutAsJsonAsync<G120Player>("/api/G120player/", player);
            if (newPlayer.IsSuccessStatusCode)
            {
                return await newPlayer.Content.ReadFromJsonAsync<G120Player>();
            }
            return null;
            /*
             return newPlayer.IsSuccessStatusCode ? 
                await newPlayer.Content.ReadFromJsonAsync<G120Player>() : 
                null;
            */
        }
    }
}

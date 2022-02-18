using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G220TeamTServ : IG220TeamTServ
    {
        private readonly HttpClient _httpClient;

        public G220TeamTServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G220TeamT> AddTeam(G220TeamT team)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G220TeamT>("/api/G220TeamT", team);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G220TeamT>() :
                null;
        }

        public async Task<IEnumerable<G220TeamT>> Buscar(int teamNum, string? nombre)
        {
            var resultado = "";
            if (teamNum > 0) { resultado =  "teamNum=" + teamNum + "&"; }
            if (!string.IsNullOrEmpty(nombre)) { resultado = resultado + "nombre=" + nombre + "&"; }
 //           if (!string.IsNullOrEmpty(desc)) { resultado = resultado + "desc=" + desc + "&"; }
            if (resultado != "") { resultado = "/api/G220TeamT/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G220TeamT>>(resultado);
        }
        
        public async Task<G220TeamT> GetTeam(int teamId)
        {
            return await _httpClient.GetFromJsonAsync<G220TeamT>($"/api/G220TeamT/{teamId}");
        }

        public async Task<IEnumerable<G220TeamT>> GetTeams()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G220TeamT>>("/api/G220TeamT/");
        }

        public async Task<G220TeamT> UpdateTeam(G220TeamT team)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G220TeamT>("/api/G220TeamT/", team);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G220TeamT>() : null;
        }
    }
}

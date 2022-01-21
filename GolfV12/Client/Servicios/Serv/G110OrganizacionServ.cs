using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G110OrganizacionServ : IG110OrganizacionServ
    {
        private readonly HttpClient _httpClient;

        public G110OrganizacionServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<G110Organizacion> AddOrganizacion(G110Organizacion organizacion)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G110Organizacion>("/api/G110Organizacion", organizacion);
            return newOrg.IsSuccessStatusCode ? 
                await newOrg.Content.ReadFromJsonAsync<G110Organizacion>() :
                null;
        }

        public async Task<IEnumerable<G110Organizacion>> Buscar(string clave, string nombre, string desc)
        {
            var resultado = "";
            if (!string.IsNullOrEmpty(clave)) { resultado = resultado + "clave=" + clave + "&"; }
            if (!string.IsNullOrEmpty(nombre)) { resultado = resultado + "nombre=" + nombre + "&"; }
            if (!string.IsNullOrEmpty(desc)) { resultado = resultado + "desc=" + desc + "&"; }
            if (resultado != "") { resultado = "/api/G110Organizacion/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G110Organizacion>>(resultado);
        }

        public async Task<G110Organizacion> GetOrganizacion(int organizacionId)
        {
            return await _httpClient.GetFromJsonAsync<G110Organizacion>($"/api/G110organizacion/{organizacionId}");
        }

        public async Task<IEnumerable<G110Organizacion>> GetOrganizaciones()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G110Organizacion>>("/api/G110organizacion");
        }

        public async Task<G110Organizacion> UpdateOrganizacion(G110Organizacion organizacion)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G110Organizacion>("/api/G110Organizacion", organizacion);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G110Organizacion>() : null;
        }
    }
}

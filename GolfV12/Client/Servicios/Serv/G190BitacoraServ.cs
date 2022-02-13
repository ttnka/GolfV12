using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G190BitacoraServ : IG190BitacoraServ
    {
        private readonly HttpClient _httpClient;

        public G190BitacoraServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G190Bitacora> AddBitacora(G190Bitacora bitacora)
        {
            var newBitacora = await _httpClient.PostAsJsonAsync<G190Bitacora>("/api/G190Bitacora", bitacora);
            return newBitacora.IsSuccessStatusCode ?
                await newBitacora.Content.ReadFromJsonAsync<G190Bitacora>() :
                null;
        }


        public async Task<IEnumerable<G190Bitacora>> Buscar(string? userId, bool sitema,
           BitaAcciones? accion, string? texto, DateTime fini, DateTime ffin)
        {
            var resultado = "";
            if (!string.IsNullOrEmpty(userId)) resultado = "userId=" + userId + "&";
            if (sitema) resultado = resultado + "sistema=true" +  "&";
            if (accion != null)  resultado = resultado + "accion=" + accion + "&"; 
            if (!string.IsNullOrEmpty(texto))  resultado = resultado + "texto=" + texto + "&"; 
            if (fini > DateTime.MinValue) resultado = resultado + "fini=" + fini + "&";
            if (ffin > DateTime.MinValue) resultado = resultado + "fini=" + fini + "&";
            if (resultado != "") { resultado = "/api/G190Bitacora/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G190Bitacora>>(resultado);
        }

        public async Task<G190Bitacora> GetBitacora(int bitacoraId)
        {
            return await _httpClient.GetFromJsonAsync<G190Bitacora>($"/api/G190Bitacora/{bitacoraId}");
        }

        public async Task<IEnumerable<G190Bitacora>> GetBitacoraAll()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G190Bitacora>>("/api/G190Bitacora");
        }
    }
}

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G500TarjetaServ : IG500TarjetaServ
    {
        private readonly HttpClient _httpClient;

        public G500TarjetaServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G500Tarjeta> AddTarjeta(G500Tarjeta tarjeta)
        {
            var newTarjeta = await _httpClient.PostAsJsonAsync<G500Tarjeta>("/api/G500Tarjeta", tarjeta);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G500Tarjeta>() :
                null;
        }

        public async Task<IEnumerable<G500Tarjeta>> Filtro(string? clave)
        {
            // clave = tar1
            // ejeplo = G500Tarjeta/filtro?clave=tar1_-_titulo=juegodellunes_-_campo=1
            var resultado = "/api/G500Tarjeta/filtro?clave=";
            if (!string.IsNullOrEmpty(clave) & clave.Count()> 13)
            {
                var parametros = clave.Split("_-_");
                string titulo = "id,creador,fecha,campo,titulo,estado,status";
                var titulos = titulo.Split(",");
                if (parametros[0] == "tar1")
                {
                    resultado = resultado + "tar1_-_";
                    for (int i = 1; i < parametros.Length; i+=2)
                    {
                        foreach (var t in titulos)
                        {
                            if (parametros[i] == t) resultado = resultado + t + "_-_" + parametros[i + 1] + "_-_";
                        }

                    }
                }
                resultado = resultado.Substring(0, resultado.Length - 3);
            }
            
            
            return await _httpClient.GetFromJsonAsync<IEnumerable<G500Tarjeta>>(resultado);
        }

        public async Task<G500Tarjeta> UpdateTarjeta(G500Tarjeta tarjeta)
        {
            var newTarjeta = await _httpClient.PutAsJsonAsync<G500Tarjeta>("/api/G500Tarjeta/", tarjeta);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G500Tarjeta>() : null;
        }
    }
}

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G510JugadorServ : IG510JugadorServ
    {
        private readonly HttpClient _httpClient;

        public G510JugadorServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G510Jugador> AddJugador(G510Jugador jugador)
        {
            var newJugador = await _httpClient.PostAsJsonAsync<G510Jugador>("/api/G510Jugador", jugador);
            return newJugador.IsSuccessStatusCode ?
                await newJugador.Content.ReadFromJsonAsync<G510Jugador>() :
                null;
        }

        public async Task<IEnumerable<G510Jugador>> Filtro(string? clave)
        {
            // clave = jug1
            // ejeplo = G510Jugador/filtro?clave=jug1_-_titulo_-_juegodellunes_-_campo_-_1
            var resultado = "/api/G510Jugador/filtro?clave=";
            if (!string.IsNullOrEmpty(clave))
            {
                var parametros = clave.Split("_-_");
                string titulo = "id,tarjeta,player,estado,status";
                var titulos = titulo.Split(",");
                if (parametros[0] == "jug1")
                {
                    resultado = resultado + "jug1_-_";
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
            return await _httpClient.GetFromJsonAsync<IEnumerable<G510Jugador>>(resultado);
        }

        public async Task<G510Jugador> UpdateJugador(G510Jugador jugador)
        {
            var newJugador = await _httpClient.PutAsJsonAsync<G510Jugador>("/api/G510Jugador/", jugador);
            return newJugador.IsSuccessStatusCode ?
                await newJugador.Content.ReadFromJsonAsync<G510Jugador>() : null;
        }
    }
}

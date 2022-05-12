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
                // "id,tarjeta,player,estado,status";
                Dictionary<string, string> Paradic = new Dictionary<string, string>();
                for (int i = 1; i < parametros.Length; i+=2)
                {
                    if (!Paradic.ContainsKey(parametros[i])) 
                        Paradic.Add(parametros[i], parametros[i+1]);
                }
                switch (parametros[0])
                {
                    case "jug1id":
                        resultado += "jug1id_-_id_-_" + Paradic["id"];
                        break;
                    case "jug2id":
                        resultado += "jug2id_-_id_-_" + Paradic["id"] + 
                            "_-_status_-_true";
                        break;
                    case "jug1tarjeta":
                        resultado += "jug1tarjeta_-_tarjeta_-_" + Paradic["tarjeta"];
                        break;
                    case "jug2tarjeta":
                        resultado += "jug2tarjeta_-_tarjeta_-_" + Paradic["tarjeta"] +
                            "_-_status_-_true";
                        break;
                    case "jug1player":
                        resultado += "jug1player_-_player_-_" + Paradic["player"];
                        break;
                    case "jug2player":
                        resultado += "jug2player_-_player_-_" + Paradic["player"] +
                            "_-_status_-_true";
                        break;
                }
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

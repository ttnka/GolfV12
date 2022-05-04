using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G520ScoreServ : IG520ScoreServ
    {
        private readonly HttpClient _httpClient;

        public G520ScoreServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G520Score> AddScore(G520Score score)
        {
            var newScore = await _httpClient.PostAsJsonAsync<G520Score>("/api/G520Score/", score);
            return newScore.IsSuccessStatusCode ?
                await newScore.Content.ReadFromJsonAsync<G520Score>() : null;
        }

        public async Task<IEnumerable<G520Score>> Filtro(string? clave)
        {
            // clave = sco1
            // ejeplo = G520Score/filtro?clave=sco1_-_titulo_-_juegodellunes_-_campo_-_1
            var resultado = "/api/G520Score/filtro?clave=";
            if (!string.IsNullOrEmpty(clave))
            {
                var parametros = clave.Split("_-_");
                string titulo = "id,tarjeta,campo,player,hcp,publico,hoyo,score,estado,status";
                var titulos = titulo.Split(",");
                if (parametros[0] == "sco1")
                {
                    resultado = resultado + "sco1_-_";
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
            return await _httpClient.GetFromJsonAsync<IEnumerable<G520Score>>(resultado);
        }

        public async Task<G520Score> UpdateScore(G520Score score)
        {
            var newJugador = await _httpClient.PutAsJsonAsync<G520Score>("/api/G520Score/", score);
            return newJugador.IsSuccessStatusCode ?
                await newJugador.Content.ReadFromJsonAsync<G520Score>() : null;
        }
    }
}

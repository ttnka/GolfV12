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
                // "id,tarjeta,campo,player,hcp,publico,hoyo,score,estado,status";
                Dictionary <string, string> ScoreDic = new Dictionary<string, string>();

                for (int i = 1; i < parametros.Length; i+=2)
                {
                    if (!ScoreDic.ContainsKey(parametros[i])) 
                        ScoreDic.Add(parametros[i], parametros[i+1]);
                }

                switch (parametros[0])
                {
                    case "sco1id":
                        resultado += "sco1id_-_id_-_" + ScoreDic["id"];
                        break;
                    case "sco2id":
                        resultado += "sco2id_-_id_-_" + ScoreDic["id"] 
                                    + "_-_status_-_true";
                        break;
                    case "sco1tarjeta":
                        resultado += "sco1tarjeta_-_tarjeta_-_" + ScoreDic["tarjeta"];
                        break;
                    case "sco2tarjeta":
                        resultado += "sco2tarjeta_-_tarjeta_-_" + ScoreDic["tarjeta"] 
                                    + "_-_status_-_true";
                        break;
                    case "sco3tarjeta":
                        resultado += "sco3tarjeta_-_tarjeta_-_" + ScoreDic["tarjeta"]
                                    + "_-_status_-_true";
                        break;
                    case "sco1player":
                        resultado += "sco1id_-_player_-_" + ScoreDic["player"];
                        break;
                    case "sco2player":
                        resultado += "sco1id_-_player_-_" + ScoreDic["player"] 
                                    + "_-_status_-_true";
                        break;

                }
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

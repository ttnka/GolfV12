using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G310ParticipantesServ : IG310ParticipantesServ 
    {
        private readonly HttpClient _httpClient;

        public G310ParticipantesServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G310Participantes> AddParticipante(G310Participantes participante)
        {
            var newTarjeta = await _httpClient.PostAsJsonAsync<G310Participantes>("/api/G310Participantes/", participante);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G310Participantes>() :
                null;
        }

        public async Task<IEnumerable<G310Participantes>> Filtro(string? clave)
        {
            // clave = tar1
            // ejeplo = G310Participantes/filtro?clave=tar1_-_titulo=juegodellunes_-_campo=1
            var resultado = "/api/G310Participantes/filtro?clave=";
            Dictionary<string, string> ParaDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(clave) & clave.Count() > 13)
            {
                var parametros = clave.Split("_-_");
                /*
                string titulo = "id,creador,fecha,campo,titulo,estado,status";
                var titulos = titulo.Split(",");
                */
                for (int i = 1; i < parametros.Length; i += 2)
                {
                    if (!ParaDic.ContainsKey(parametros[i]))
                        ParaDic.Add(parametros[i], parametros[i + 1]);
                }
                switch (parametros[0])
                {
                    case "part1id":
                        resultado += "part1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "part2id":
                        resultado += "part2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    case "part3id":
                        resultado += "part2id_-_id_-_" + ParaDic["id"] + "_-_estado_-_" + ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "part1azar":
                        resultado += "part1azar_-_azar_-_" + ParaDic["azar"];
                        break;
                    case "part2azar":
                        resultado += "part2azar_-_azar_-_" + ParaDic["azar"] + "_-_status_-_true";
                        break;
                    case "part3azar":
                        resultado += "part3azar_-_azar_-_" + ParaDic["azar"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break ;
                    case "part1tarjeta":
                        resultado += "part1tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"];
                        break;
                    case "part2tarjeta":
                        resultado += "part2tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_status_-_true";
                        break;
                    case "part3tarjeta":
                        resultado += "part3tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "part1j1":
                        resultado += "part1j1_-_j1_-_" + ParaDic["j1"] + "_-_tarjeta_-_" + ParaDic["tarjeta"];
                        break;
                    case "part2j1":
                        resultado += "part2j1_-_j1_-_" + ParaDic["j1"] + "_-_tarjeta_-_" + ParaDic["tarjeta"] +
                             "_-_azar_-_" + ParaDic["azar"] + "_-_status_-_true";
                        break;
                    case "part3j1":
                        resultado += "part3j1_-_j_-_" + ParaDic["j"] + "_-_tarjeta_-_" + ParaDic["tarjeta"] + 
                                "_-_status_-_true";
                        break;
                    case "part4j1":
                        resultado += "part3j1_-_j_-_" + ParaDic["j"] + "_-_tarjeta_-_" + ParaDic["tarjeta"] + 
                            "_-_azar_-_" + ParaDic["azar"] +"_-_status_-_true";
                        break;

                    case "part1j2":
                        resultado += "part1j2_-_j2_-_" + ParaDic["j2"] + "_-_tarjeta_-_" + ParaDic["tarjeta"];
                        break;
                    case "part2j2":
                        resultado += "part2j2_-_j2_-_" + ParaDic["j2"] + "_-_tarjeta_-_" + ParaDic["tarjeta"] +
                            "_-_azar_-_" + ParaDic["azar"] + "_-_status_-_true";
                        break;
                      
                }

            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G310Participantes>>(resultado);
        }

        public async Task<G310Participantes> UpdateParticipante(G310Participantes participante)
        {
            var newTarjeta = await _httpClient.PutAsJsonAsync<G310Participantes>("/api/G310Participantes/", participante);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G310Participantes>() : null;
        }
    }
}

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G324ParejasServ : IG324ParejasServ 
    {
        private readonly HttpClient _httpClient;

        public G324ParejasServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G324Parejas> AddPareja(G324Parejas pareja)
        {
            var newTarjeta = await _httpClient.PostAsJsonAsync<G324Parejas>("/api/G324Parejas/", pareja);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G324Parejas>() :
                null;
        }

        public async Task<IEnumerable<G324Parejas>> Filtro(string? clave)
        {
            // clave = tar1
            // ejeplo = G324Parejas/filtro?clave=tar1_-_titulo=juegodellunes_-_campo=1
            var resultado = "/api/G324Parejas/filtro?clave=";
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
                    case "par1id":
                        resultado += "par1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "par2id":
                        resultado += "par2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    case "par3id":
                        resultado += "par2id_-_id_-_" + ParaDic["id"] + "_-_estado_-_" + ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "par1azar":
                        resultado += "par1azar_-_azar_-_" + ParaDic["azar"];
                        break;
                    case "par2azar":
                        resultado += "par2azar_-_azar_-_" + ParaDic["azar"] + "_-_status_-_true";
                        break;
                    case "par3azar":
                        resultado += "par3azar_-_azar_-_" + ParaDic["azar"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "par1tarjeta":
                        resultado += "par1tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_azar_-_" + ParaDic["azar"];
                        break;
                    case "par2tarjeta":
                        resultado += "par2tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_azar_-_" + ParaDic["azar"] + 
                                "_-_status_-_true";
                        break;
                    case "par3tarjeta":
                        resultado += "par3tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] +
                                "_-_status_-_true";
                        break;

                    case "par1jugador":
                        resultado += "par1jugador_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_azar_-_" + ParaDic["azar"] +
                                "_-_j1_-_" + ParaDic["j1"] +"_-_status_-_true";
                        break;
                    case "par2jugador":
                        resultado += "par2jugador_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_azar_-_" + ParaDic["azar"] +
                                "_-_j2_-_" + ParaDic["j2"] + "_-_status_-_true";
                        break;
                    case "par3jugador":
                        resultado += "par3jugador_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_azar_-_" + ParaDic["azar"] +
                                "_-_j3_-_" + ParaDic["j3"] + "_-_status_-_true";
                        break;
                    case "par4jugador":
                        resultado += "par4jugador_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_azar_-_" + ParaDic["azar"] +
                                "_-_j4_-_" + ParaDic["j4"] + "_-_status_-_true";
                        break;
                    case "par5jugador":
                        resultado += "par5jugador_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_azar_-_" + ParaDic["azar"] +
                                "_-_j_-_" + ParaDic["j"] + "_-_status_-_true";
                        break;

                }

            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G324Parejas>>(resultado);
        }

        public async Task<G324Parejas> UpdatePareja(G324Parejas pareja)
        {
            var newTarjeta = await _httpClient.PutAsJsonAsync<G324Parejas>("/api/G324Parejas/", pareja);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G324Parejas>() : null;
        }
    }
}

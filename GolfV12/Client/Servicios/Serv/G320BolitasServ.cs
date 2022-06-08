using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G320BolitasServ : IG320BolitasServ 
    {
        private readonly HttpClient _httpClient;

        public G320BolitasServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G320Bolitas> AddBolitas(G320Bolitas bolita)
        {
            var newTarjeta = await _httpClient.PostAsJsonAsync<G320Bolitas>("/api/G320Bolitas/", bolita);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G320Bolitas>() :
                null;
        }

        public async Task<IEnumerable<G320Bolitas>> Filtro(string? clave)
        {
            // clave = tar1
            // ejeplo = G320Bolitas/filtro?clave=tar1_-_titulo=juegodellunes_-_campo=1
            var resultado = "/api/G320Bolitas/filtro?clave=";
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
                    case "bol1id":
                        resultado += "bol1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "bol2id":
                        resultado += "bol2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    case "bol3id":
                        resultado += "bol2id_-_id_-_" + ParaDic["id"] + "_-_estado_-_" + ParaDic["estado"] + "_-_status_-_true";
                        break;

                    case "bol1azar":
                        resultado += "bol1azar_-_azar_-_" + ParaDic["azar"];
                        break;
                    case "bol2azar":
                        resultado += "bol2azar_-_azar_-_" + ParaDic["azar"] + "_-_status_-_true";
                        break;
                    case "bol3azar":
                        resultado += "bol3azar_-_azar_-_" + ParaDic["azar"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;

                    case "bol1tarjeta":
                        resultado += "bol1tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"];
                        break;
                    case "bol2tarjeta":
                        resultado += "bol2tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_status_-_true";
                        break;
                    case "bol1jugador":
                        resultado += "bol1jugador_-_j1_-_" + ParaDic["j1"] + "_-_azar_-_" +
                                ParaDic["azar"] + "_-_status_-_true";
                        break;
                    case "bol2jugador":
                        resultado += "bol2jugador_-_j2_-_" + ParaDic["j2"] + "_-_azar_-_" +
                                ParaDic["azar"] + "_-_status_-_true";
                        break;
                    case "bol3jugador":
                        resultado += "bol3jugador_-_j_-_" + ParaDic["j"] + "_-_azar_-_" +
                                ParaDic["azar"] + "_-_status_-_true" ;
                        break;
                        
                }

            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G320Bolitas>>(resultado);
        }

        public async Task<G320Bolitas> UpdateBolitas(G320Bolitas bolita)
        {
            var newTarjeta = await _httpClient.PutAsJsonAsync<G320Bolitas>("/api/G320Bolitas/", bolita);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G320Bolitas>() : null;
        }
    }
}

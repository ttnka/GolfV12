using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G300AzarServ : IG300AzarServ 
    {
        private readonly HttpClient _httpClient;

        public G300AzarServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G300Azar> AddAzar(G300Azar azar)
        {
            var newTarjeta = await _httpClient.PostAsJsonAsync<G300Azar>("/api/G300Azar/", azar);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G300Azar>() :
                null;
        }

        public async Task<IEnumerable<G300Azar>> Filtro(string? clave)
        {
            // clave = tar1
            // ejeplo = G300Azar/filtro?clave=tar1_-_titulo=juegodellunes_-_campo=1
            var resultado = "/api/G300Azar/filtro?clave=";
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
                    case "azar1id":
                        resultado += "azar1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "azar2id":
                        resultado += "azar2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    case "azar3id":
                        resultado += "azar2id_-_id_-_" + ParaDic["id"] + "_-_estado_-_" + ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "azar1creador":
                        resultado += "azar1creador_-_creador_-_" + ParaDic["creador"];
                        break;
                    case "azar2creador":
                        resultado += "azar2creador_-_creador_-_" + ParaDic["creador"] + "_-_status_-_true";
                        break;
                    case "azar3creador":
                        resultado += "azar3creador_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "azar1tarjeta":
                        resultado += "azar1tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"];
                        break;
                    case "azar2tarjeta":
                        resultado += "azar2tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_status_-_true";
                        break;
                    case "azar3tarjeta":
                        resultado += "azar3tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_creador_-_" +
                                ParaDic["creador"] + "_-_status_-_true";
                        break;
/*
                    case "azar4creador":
                        resultado += "azar4creador_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
*/
                }

            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G300Azar>>(resultado);
        }

        public async Task<G300Azar> UpdateAzar(G300Azar azar)
        {
            var newTarjeta = await _httpClient.PutAsJsonAsync<G300Azar>("/api/G300Azar/", azar);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G300Azar>() : null;
        }
    }
}

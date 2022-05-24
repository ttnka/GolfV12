using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G176HoyoServ : IG176HoyoServ
    {
        private readonly HttpClient _httpClient;

        public G176HoyoServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G176Hoyo> AddHoyo(G176Hoyo hoyo)
        {
            var newHoyo = await _httpClient.PostAsJsonAsync<G176Hoyo>("/api/G176Hoyo", hoyo);
            return newHoyo.IsSuccessStatusCode ?
                await newHoyo.Content.ReadFromJsonAsync<G176Hoyo>() :
                null;
        }

        public async Task<IEnumerable<G176Hoyo>> Filtro(string? clave)
        {
            // clave = hoy1
            // ejeplo = G176Hoyo/filtro?clave=hoy1_-_hoyo_-_1_-_campo_-_2
            var resultado = "/api/G176Hoyo/filtro?clave=";
            Dictionary<string, string> ParaDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(clave) & clave.Count() > 13)
            {
                var parametros = clave.Split("_-_");
                
                for (int i = 1; i < parametros.Length; i += 2)
                {
                    if (!ParaDic.ContainsKey(parametros[i]))
                        ParaDic.Add(parametros[i], parametros[i + 1]);
                }
                switch (parametros[0])
                {
                    case "hoy1id":
                        resultado += "hoy1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "hoy2id":
                        resultado += "hoy2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    case "hoy1campo":
                        resultado += "hoy1campo_-_campo_-_" + ParaDic["campo"];
                        break;
                    case "hoy2campo":
                        resultado += "hoy2campo_-_campo_-_" + ParaDic["campo"] + "_-_status_-_true";
                        break;
                    /*    
                    case "tar3creador":
                        resultado += "tar3creador_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "tar4creador":
                        resultado += "tar4creador_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                    */
                }

            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G176Hoyo>>(resultado);
        }

        /*
        public async Task<IEnumerable<G176Hoyo>> Buscar(int campo, string? ruta, int hoyoN)
        {
            var resultado = "";
            if (campo > 0) { resultado = "campo=" + campo + "&"; }
            if (!string.IsNullOrEmpty(ruta)) { resultado = resultado + "ruta=" + ruta + "&"; }
            if (hoyoN > 0) { resultado = resultado + "hoyoN=" + hoyoN + "&"; }
            if (resultado != "") { resultado = "/api/G176Hoyo/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G176Hoyo>>(resultado);
        }

        public async Task<G176Hoyo> GetHoyo(int hoyoId)
        {
            return await _httpClient.GetFromJsonAsync<G176Hoyo>($"/api/G176Hoyo/{hoyoId}");
        }

        public async Task<IEnumerable<G176Hoyo>> GetHoyos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G176Hoyo>>("/api/G176Hoyo/");

        }
        */
        public async Task<G176Hoyo> UpdateHoyo(G176Hoyo hoyo)
        {
            var newHoyo = await _httpClient.PutAsJsonAsync<G176Hoyo>("/api/G176Hoyo", hoyo);
            return newHoyo.IsSuccessStatusCode ?
                await newHoyo.Content.ReadFromJsonAsync<G176Hoyo>() : null;
        }
    }
}

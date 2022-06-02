using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G390TiposAzarServ : IG390TiposAzarServ
    {
        private readonly HttpClient _httpClient;

        public G390TiposAzarServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G390TiposAzar> AddTiposAzar(G390TiposAzar azarT)
        {
            var newTarjeta = await _httpClient.PostAsJsonAsync<G390TiposAzar>("/api/G390TiposAzar/", azarT);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G390TiposAzar>() :
                null;
        }

        public async Task<IEnumerable<G390TiposAzar>> Filtro(string? clave)
        {
            // clave = tar1
            // ejeplo = G390TiposAzar/filtro?clave=tar1_-_titulo=juegodellunes_-_campo=1
            var resultado = "/api/G390TiposAzar/filtro?clave=";
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
                    case "azart1id":
                        resultado += "azart1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "azart2id":
                        resultado += "azart2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    
                    case "azart1creador":
                        resultado += "azart1creador_-_creador_-_" + ParaDic["creador"];
                        break;
                    case "azart2creador":
                        resultado += "azart2creador_-_creador_-_" + ParaDic["creador"] + "_-_status_-_true";
                        break;
                    case "azart3creador":
                        resultado += "azart3creador_-_creador_-_" + ParaDic["creador"] + "_-_publico_-_" +
                                ParaDic["publico"] + "_-_status_-_true";
                        break;
/*
                    case "azart1tarjeta":
                        resultado += "azart1tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"];
                        break;
                    case "azart2tarjeta":
                        resultado += "azart2tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_status_-_true";
                        break;
                    case "azart3tarjeta":
                        resultado += "azart3tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_creador_-_" +
                                ParaDic["creador"] + "_-_status_-_true";
                        break;
                        
                    case "azar4creador":
                        resultado += "azar4creador_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                        */
                }

            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G390TiposAzar>>(resultado);
        }

        public async Task<G390TiposAzar> UpdateTiposAzar(G390TiposAzar azarT)
        {
            var newTarjeta = await _httpClient.PutAsJsonAsync<G390TiposAzar>("/api/G390TiposAzar/", azarT);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G390TiposAzar>() : null;
        }
    }
}

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G120PlayerServ : IG120PlayerServ
    {
        private readonly HttpClient _httpClient;

        public G120PlayerServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<G120Player> AddPlayer(G120Player player)
        {
            var newPlayer = await _httpClient.PostAsJsonAsync<G120Player>("/api/G120player", player);
            return newPlayer.IsSuccessStatusCode ? await newPlayer.Content.ReadFromJsonAsync<G120Player>() :
             null;
        }
   
        public async Task<IEnumerable<G120Player>> Filtro(string? clave)
        {

            // clave = Play1
            // ejeplo = G120Player/filtro?clave=play1_-_userId=abc12_-_campo=1
            var resultado = "/api/G120Player/filtro?clave=";
            Dictionary<string, string> ParaDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(clave) && clave.Count() > 13)
            {
                var parametros = clave.Split("_-_");
                
                for (int i = 1; i < parametros.Length; i += 2)
                {
                    if (!ParaDic.ContainsKey(parametros[i]))
                        ParaDic.Add(parametros[i], parametros[i + 1]);
                }
                switch (parametros[0])
                {
                    case "play1id":
                        resultado += "play1id_-_userid_-_" + ParaDic["userid"];
                        break;
                    case "play2id":
                        resultado += "play2id_-_userid_-_" + ParaDic["userid"] + "_-_status_-_true";
                        break;
                    case "play3id":
                        resultado += "pla32id_-_userid_-_" + ParaDic["userid"] + 
                                        "nivel_-_" + ParaDic["nivel"] +"_-_status_-_true";
                        break;
                    case "play1nombre":
                        resultado += "play1nombre_-_nombre_-_" + ParaDic["nombre"];
                        break;
                    case "play2nombre":
                        resultado += "play2nombre_-_nombre_-_" + ParaDic["nombre"] + "_-_paterno_-_" + ParaDic["paterno"];
                        break;
                    case "play3nombre":
                        resultado += "play3nombre_-_nombre_-_" + ParaDic["nombre"] + "_-_paterno_-_" + ParaDic["paterno"] + 
                            "_-_materno_-_" + ParaDic["materno"];
                        break;
                    case "play1nivel":
                        resultado += "play1nivel_-_nivel_-_" + ParaDic["nivel"];
                        break;
                    case "play1status":
                        resultado += "play1status_-_status_-_" + ParaDic["status"];
                        break;
                    case "all":
                        resultado += "all";
                        break;
                }
            }
           
            return await _httpClient.GetFromJsonAsync<IEnumerable<G120Player>>(resultado);
        }
        public async Task<G120Player> UpdatePlayer(G120Player player)
        {
            var newPlayer = await _httpClient.PutAsJsonAsync<G120Player>("/api/G120player/", player);
            if (newPlayer.IsSuccessStatusCode)
            {
                return await newPlayer.Content.ReadFromJsonAsync<G120Player>();
            }
            return null;
            /*
             return newPlayer.IsSuccessStatusCode ? 
                await newPlayer.Content.ReadFromJsonAsync<G120Player>() : 
                null;
            */
        }
    }
}

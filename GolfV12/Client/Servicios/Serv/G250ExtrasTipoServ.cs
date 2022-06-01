using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G250ExtrasTipoServ : IG250ExtrasTipoServ
    {
        private readonly HttpClient _httpClient;

        public G250ExtrasTipoServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G250ExtrasTipo> AddExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            var newExtraT = await _httpClient.PostAsJsonAsync<G250ExtrasTipo>("/api/G250ExtrasTipo", extrasTipo);
            return newExtraT.IsSuccessStatusCode ?
                await newExtraT.Content.ReadFromJsonAsync<G250ExtrasTipo>() :
                null;
        }
        
        public async Task<IEnumerable<G250ExtrasTipo>> Filtro(string? clave)
        {
            // clave = exttipo1tipo
            // ejeplo = G242extratipo/filtro?clave=exttipo1tipo_-_userId=abc12_-_campo=1
            var resultado = "/api/G250ExtrasTipo/filtro?clave=";
            Dictionary<string, string> ParaDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(clave) && clave.Count() > 2)
            {
                var parametros = clave.Split("_-_");

                for (int i = 1; i < parametros.Length; i += 2)
                {
                    if (!ParaDic.ContainsKey(parametros[i]))
                        ParaDic.Add(parametros[i], parametros[i + 1]);
                }
                switch (parametros[0])
                {
                    case "exttipo1id":
                        resultado += "exttipo1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "exttipo2id":
                        resultado += "exttipo2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    case "exttipo3id":
                        resultado += "extipo3id_-_id_-_" + ParaDic["id"] +
                                        "nivel_-_" + ParaDic["nivel"] + "_-_status_-_true";
                        break;
                    case "exttipo1creador":
                        resultado += "exttipo1creador_-_creador_-_" + ParaDic["creador"];
                        break;
                    case "exttipo2creador":
                        resultado += "exttipo2creador_-_creador_-_" + ParaDic["creador"] ;
                        break;
                    case "exttipo3creador":
                        resultado += "exttipo3creador_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" + ParaDic["estado"];
                        break;
                    case "exttipo4creador":
                        resultado += "exttipo4creador_-_creador_-_" + ParaDic["creador"];
                        break;
                    case "exttipo1publico":
                        resultado += "exttipo1publico_-_publico_-_" + ParaDic["publico"];
                        break;
                    case "exttipo2publico":
                        resultado += "exttipo2publico_-_publico_-_" + ParaDic["publico"] + "_-_status_-_" + ParaDic["status"];
                        break;
                    
                    case "all":
                        resultado += "all";
                        break;
                }
            }

            return await _httpClient.GetFromJsonAsync<IEnumerable<G250ExtrasTipo>>(resultado);
        }
            public async Task<G250ExtrasTipo> UpdateExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G250ExtrasTipo>("/api/G250ExtrasTipo/", extrasTipo);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G250ExtrasTipo>() : null;
        }
    }
}

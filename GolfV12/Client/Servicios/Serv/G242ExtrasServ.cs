using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G242ExtrasServ : IG242ExtrasServ
    {
        private readonly HttpClient _httpClient;

        public G242ExtrasServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G242Extras> AddExtra(G242Extras extra)
        {
            var newOrg = await _httpClient.PostAsJsonAsync<G242Extras>("/api/G242Extras", extra);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G242Extras>() :
                null;
        }
        /*
        public async Task<IEnumerable<G242Extras>> Buscar(int rol, string exter, int hoyo, int tipoExtra)
        {
            var resultado = "";
            if (rol > 0) { resultado = "rol=" + rol + "&"; }
            if (!string.IsNullOrEmpty(exter)) { resultado = resultado + "exter=" + exter + "&"; }
            if (hoyo > 0) { resultado = resultado + "hoyo=" + hoyo + "&"; }
            if (tipoExtra > 0) { resultado = resultado + "tipoextra=" + tipoExtra + "&"; }
            if (resultado != "") { resultado = "/api/G242Extras/filtro?" + resultado; }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G242Extras>>(resultado);
        }

        public async Task<G242Extras> GetExtra(int extraId)
        {
            return await _httpClient.GetFromJsonAsync<G242Extras>($"/api/G242Extras/{extraId}");

        }
        
        public async Task<IEnumerable<G242Extras>> GetExtras()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<G242Extras>>("/api/G242Extras/");
        }
        */
        public async Task<IEnumerable<G242Extras>> Filtro(string? clave)
        {
            // clave = ext1
            // ejeplo = G242Extra/filtro?clave=ext1_-_Id_-_abc12_-_
            var resultado = "/api/G242Extras/filtro?clave=";
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
                    case "ext1id":
                        resultado += "ext1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "ext2id":
                        resultado += "ext2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    
                    case "ext1tarjeta":
                        resultado += "ext1tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"];
                        break;
                    case "ext2tarjeta":
                        resultado += "ext2tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"];
                        break;
                    case "ext3tarjeta":
                        resultado += "ext3tarjeta_-_tarjeta_-_" + ParaDic["tarjeta"] + "_-_tipoextra_-_" + ParaDic["tipoextra"];
                        break;
                    /*
                    case "ext1nivel":
                        resultado += "ext1nivel_-_nivel_-_" + ParaDic["nivel"];
                        break;
                    case "ext1status":
                        resultado += "ext1status_-_status_-_" + ParaDic["status"];
                        break;
                    case "all":
                        resultado += "all";
                        break;
                    */
                }
            }

            return await _httpClient.GetFromJsonAsync<IEnumerable<G242Extras>>(resultado);
        }
            public async Task<G242Extras> UpdateExtra(G242Extras extra)
        {
            var newOrg = await _httpClient.PutAsJsonAsync<G242Extras>("/api/G242Extras/", extra);
            return newOrg.IsSuccessStatusCode ?
                await newOrg.Content.ReadFromJsonAsync<G242Extras>() : null;
        }
    }
}

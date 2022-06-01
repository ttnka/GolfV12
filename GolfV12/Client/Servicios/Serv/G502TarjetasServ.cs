﻿using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using System.Net.Http.Json;

namespace GolfV12.Client.Servicios.Serv
{
    public class G502TarjetasServ : IG502TarjetasServ
    {
        private readonly HttpClient _httpClient;

        public G502TarjetasServ(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<G502Tarjetas> AddTarjeta(G502Tarjetas tarjeta)
        {
            var newTarjeta = await _httpClient.PostAsJsonAsync<G502Tarjetas>("/api/G502Tarjetas", tarjeta);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G502Tarjetas>() :
                null;
        }

        public async Task<IEnumerable<G502Tarjetas>> Filtro(string? clave)
        {
            // clave = tar1
            // ejeplo = G502Tarjetas/filtro?clave=tar1_-_titulo=juegodellunes_-_campo=1
            var resultado = "/api/G502Tarjetas/filtro?clave=";
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
                    case "tar1id":
                        resultado += "tar1id_-_id_-_" + ParaDic["id"];
                        break;
                    case "tar2id":
                        resultado += "tar2id_-_id_-_" + ParaDic["id"] + "_-_status_-_true";
                        break;
                    case "tar1creador":
                        resultado += "tar1creador_-_creador_-_" + ParaDic["creador"];
                        break;
                    case "tar2creador":
                        resultado += "tar2creador_-_creador_-_" + ParaDic["creador"] + "_-_status_-_true";
                        break;
                    case "tar3creador":
                        resultado += "tar3creador_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "tar4creador":
                        resultado += "tar4creador_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "tar1participante":
                        resultado += "tar1participante_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                    case "tar2participante":
                        resultado += "tar2participante_-_creador_-_" + ParaDic["creador"] + "_-_estado_-_" +
                                ParaDic["estado"] + "_-_status_-_true";
                        break;
                }

            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<G502Tarjetas>>(resultado);
        }

        public async Task<G502Tarjetas> UpdateTarjeta(G502Tarjetas tarjeta)
        {
            var newTarjeta = await _httpClient.PutAsJsonAsync<G502Tarjetas>("/api/G502Tarjetas/", tarjeta);
            return newTarjeta.IsSuccessStatusCode ?
                await newTarjeta.Content.ReadFromJsonAsync<G502Tarjetas>() : null;
        }
    }
}
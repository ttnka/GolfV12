using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class MisTarjetasBase : ComponentBase 
    {
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        
        [Parameter]
        public int ElEstado { get; set; } = 0;
        [Parameter]
        public Dictionary<string, string> DicHijo { get; set; } = new Dictionary<string, string>();
        
        [Parameter]
        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } =
                    new List<KeyValuePair<string, string>>();
        [Parameter]
        public IEnumerable<KeyValuePair<int, string>> LosCampos { get; set; } = 
                        new List<KeyValuePair<int, string>>();
        [Parameter]
        public EventCallback<G500Tarjeta> OnInsertJugador { get; set; }
        [Inject]
        public IG500TarjetaServ TarjetaIServ { get; set; }
        public G500Tarjeta LaTarjeta { get; set; } = new();
        public IEnumerable<G500Tarjeta> LasTarjetas { get; set; } 
        [Inject]
        public IG522ScoresServ ScoresIServ  { get; set; }
        
        [Inject]
        public IG510JugadorServ JugadorIServ { get; set; }
        
        [Inject]
        public IG176HoyoServ HoyoIServ { get; set; }
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<G500Tarjeta> TarjetaGrid { get; set; } = new();

        public IEnumerable<Torneo2Edit> LasCapturas { get; set; } = Enum.GetValues(typeof(Torneo2Edit)).Cast<Torneo2Edit>().ToList();
        public IEnumerable<TorneoView> LasConsultas { get; set; } = Enum.GetValues(typeof(TorneoView)).Cast<TorneoView>().ToList();

        

        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            await LeerDatos();
            
        }
        protected void JugXtarjeta(string tarjId, int cantidad)
        {
            if ( !DatosDic.ContainsKey($"JugXtarjeta_{tarjId}"))
            {
                DatosDic.Add($"JugXtarjeta_{tarjId}", cantidad.ToString());
            } else
            {
                DatosDic[$"JugXtarjeta_{tarjId}"] = cantidad.ToString();
            }
        }
        
        protected async Task LeerDatos()
        {
            List<G500Tarjeta> LsParticipa = new List<G500Tarjeta>();
            
            var JugTar = await JugadorIServ.Filtro($"jug2player_-_player_-_{UserIdLog}");
            if (JugTar != null)
            {
                foreach (var jt in JugTar)
                {
                    ListaParticipa(jt.Tarjeta);        
                }
            }
            string[] ParticipaArray = new string[0];
            if (DatosDic.ContainsKey($"Participa_{UserIdLog}"))
                {
                    var PATemp = DatosDic[$"Participa_{UserIdLog}"];
                    ParticipaArray = PATemp.Split(",");
                }
            var TarjAll = await TarjetaIServ.Filtro("All");
            if (TarjAll != null )
            {
                foreach (var Tarj in TarjAll)
                {
                    var registrala = 0;
                    if (ElEstado == 3)
                    {
                        if ((ParticipaArray.Any(x => x == Tarj.Id) || Tarj.Creador == UserIdLog) && Tarj.Estado == 3)
                        { 
                            if (!ParticipaArray.Any(x => x==Tarj.Id)) ListaParticipa(Tarj.Id);
                            registrala = 1;
                        }
                    } 
                    else
                    {
                        if ((ParticipaArray.Any(x => x == Tarj.Id) || Tarj.Creador == UserIdLog) && Tarj.Estado != 3)
                        {
                            if (!ParticipaArray.Any(x => x == Tarj.Id)) ListaParticipa(Tarj.Id);
                            registrala=1;
                        }
                    }
                    if (registrala == 1)
                    {
                        LsParticipa.Add(Tarj);
                        var numJug = await JugadorIServ.Filtro($"tar1id_-_id_-_{Tarj.Id}");
                        if (numJug != null)
                        {
                            foreach (var item in numJug)
                            {
                                JugXtarjeta(item.Tarjeta, numJug.Count(x => x.Tarjeta == item.Tarjeta));
                            }
                        }
                    }      
                }
            }
                LasTarjetas = LsParticipa.AsEnumerable();
        }

        protected void ListaParticipa(string tarjId)
        {
            if (DatosDic.ContainsKey($"Participa_{UserIdLog}"))
            {
                DatosDic[$"Participa_{UserIdLog}"] += $",{tarjId}";
            }
            else
            {
                DatosDic.Add($"Participa_{UserIdLog}", tarjId);
            }
        }

        protected async Task LeerPermisos()
        {
            LaTarjeta = (await TarjetaIServ.Filtro($"tar1id_-_id_-_{TarjetaId}")).FirstOrDefault();
            
            if (!DatosDic.ContainsKey($"PermisoLeer_{TarjetaId}_Usuario_{UserIdLog}"))
            {
                DatosDic.Add($"PermisoLeer_{TarjetaId}_Usuario_{UserIdLog}", "0");
                DatosDic.Add($"PermisoEscribir_{TarjetaId}_Usuario_{UserIdLog}", "0");
            }
            if (LaTarjeta != null)
            {
                string[] involucrados = DatosDic[$"Involucrados_{TarjetaId}"].Split(",");
                // LEER creador captura, jugador todos
                if (LaTarjeta.Consulta == TorneoView.Capturista)
                {
                    // PENDIENTE
                }
                else
                {   
                    if (involucrados.Any(UserIdLog.Contains))
                        DatosDic[$"PermisoLeer_{TarjetaId}_Usuario_{UserIdLog}"] = "1";
                }
                // Escribir Creador Captura dif jugador

                if (LaTarjeta.Estado != 3)
                {                
                    if ((LaTarjeta.Captura == Torneo2Edit.Jugadores &&
                                involucrados.Any(UserIdLog.Contains)))
                                            DatosDic[$"PermisoEscribir_{TarjetaId}_Usuario_{UserIdLog}"] = "1";

                    /* Esta caso es solo cada jugador sin creador 
                        if ((LaTarjeta.Captura == Torneo2Edit.Creador &&
                                    LaTarjeta.Creador == UserIdLog))
                                                DatosDic[$"PermisoEscribir_{TarjetaId}_Usuario_{UserIdLog}"] = "2";
                    */

                    if (LaTarjeta.Creador == UserIdLog)
                        DatosDic[$"PermisoEscribir_{TarjetaId}_Usuario_{UserIdLog}"] = "2";
                }

                // Leer Par y Dificultad

                var ParTemp = await HoyoIServ.Filtro($"hoy1campo_-_campo_-_{LaTarjeta.Campo}");
                if (ParTemp != null)
                {
                    foreach (var item in ParTemp)
                    {
                        if (!DatosDic.ContainsKey($"HoyoPar_{item.Hoyo}"))
                        {
                            DatosDic.Add($"HoyoPar_{item.Hoyo}", item.Par.ToString());
                            DatosDic.Add($"HoyoH_{item.Hoyo}", item.HcpHombres.ToString());
                            DatosDic.Add($"HoyoW_{item.Hoyo}", item.HcpMujeres.ToString());
                        }
                    }
                }
            }
        }

        public async Task SaveTarjeta()
        {
            G500Tarjeta resultado = new G500Tarjeta();

            resultado = await TarjetaIServ.AddTarjeta(LaTarjeta);
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                $"El usuario agrego un nuevo tarjeta de juego {resultado.Id} {resultado.Titulo} ");

            //if (resultado != null) NM.NavigateTo($"/admin/hcp/{PlayerId}");
        }


        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; }
        [Inject]
        public IG190BitacoraServ BitacoraServ { get; set; }
        private G190Bitacora WriteBitacora { get; set; } = new G190Bitacora();
        public async Task EscribirBitacoraUno(string userId, BitaAcciones accion, bool Sistema, string desc)
        {
            WriteBitacora.Fecha = DateTime.Now;
            WriteBitacora.Accion = accion;
            WriteBitacora.Sistema = Sistema;
            WriteBitacora.UsuarioId = userId;
            WriteBitacora.Desc = desc;
            await BitacoraServ.AddBitacora(WriteBitacora);
        }

    }
}

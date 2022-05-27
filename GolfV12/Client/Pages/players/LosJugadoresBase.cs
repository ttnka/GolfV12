using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class LosJugadoresBase : ComponentBase
    {
        /*
        [Parameter]
        public G500Tarjeta TarjetaHijo { get; set; } = new();
        */
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        public G500Tarjeta TarjetaUnica { get; set; } = new G500Tarjeta();
        /*
        [Parameter]
        public Dictionary<string, string> DicHijo { get; set; } = new Dictionary<string, string>();
        */
        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }

        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } =
            new List<KeyValuePair<string, string>>();
        protected List<KeyValuePair<string, string>> NombresList { get; set; } =
                new List<KeyValuePair<string, string>>();

        [Inject]
        public IG500TarjetaServ TarjetaIServ { get; set; }
        [Inject]
        public IG510JugadorServ JugadorIServ { get; set; }
        [Parameter]
        public IEnumerable<G510Jugador> LosJugadores { get; set; } = new List<G510Jugador>();
        public G510Jugador ElJugador { get; set; } = new G510Jugador();
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<G510Jugador> JugadoresGrid { get; set; } = new();
        [Inject]
        public NavigationManager NM { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(TarjetaId)) 
                NM.NavigateTo("/players/misdatos");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerNombres();
            await LeerTarjeta(); 
            await LeerJugadores();
            
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El usuario consulto el listado de jugadores de la tarjeta {TarjetaUnica.Titulo} {TarjetaId}");
        }
        protected async Task LeerNombres()
        {
            var NameTemp = await PlayerIServ.Filtro("all");
            foreach (var t in NameTemp)
            {
                if (!DatosDic.ContainsKey($"Nombre_{t.UserId}"))
                {
                    DatosDic.Add($"Nombre_{t.UserId}", $"{t.Nombre} {t.Apodo} {t.Paterno}");
                    NombresList.Add(new KeyValuePair<string, string>(t.UserId, $"{t.Nombre} {t.Apodo} {t.Paterno}"));
                }
            }
            LosNombres = NombresList.AsEnumerable();
        }
        protected async Task LeerTarjeta()
        {
            var TUTemp = await TarjetaIServ.Filtro($"tar2id_-_id_-_{TarjetaId}");
            if (TUTemp != null)
            {
                TarjetaUnica = TUTemp.FirstOrDefault();
            }
            else
            {
                NM.NavigateTo("/players/misdatos");
            }
            
        }
        protected async Task LeerJugadores()
        {
            if (!string.IsNullOrEmpty(TarjetaId))
            {
                LosJugadores = await JugadorIServ.Filtro($"jug2tarjeta_-_tarjeta_-_{TarjetaId}");
                if (LosJugadores.Any())
                { 
                    int renglon = 1;
                    foreach (var jugadorx in LosJugadores)
                    {
                        if(!DatosDic.ContainsKey($"RenglonJugador_{jugadorx.Player}")) {
                            DatosDic.Add($"RenglonJugador_{jugadorx.Player}", renglon.ToString());
                            DatosDic.Add($"Tarjeta_{TarjetaId}_Jugador_{jugadorx.Player}", jugadorx.Player.ToString());
                            renglon++;
                        }
                    }
                }
            }
        }
        public async Task SaveJugador()
        {
            G510Jugador resultado = new G510Jugador();

            resultado = await JugadorIServ.AddJugador(ElJugador);
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                $"El usuario agrego un nuevo jugador {DatosDic[$"Nombre_{resultado.Player}"]} {resultado.Player}");
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

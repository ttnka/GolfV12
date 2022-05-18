using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class LosJugadoresBase : ComponentBase 
    {
        [Parameter]
        public G500Tarjeta TarjetaHijo { get; set; } = new();
        
        
        [Parameter]
        public Dictionary<string, string> DicHijo { get; set; } = new Dictionary<string, string>();
        
        [Parameter]
        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } =
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
        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            await LeerJugadores();
            
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El usuario consulto el listado de jugadores de la tarjeta {TarjetaHijo.Titulo} {TarjetaHijo.Id}");
        }

        protected async Task LeerJugadores()
        {
            if (!string.IsNullOrEmpty(TarjetaHijo.Id))
            {
                LosJugadores = await JugadorIServ.Filtro($"jug2tarjeta_-_tarjeta_-_{TarjetaHijo.Id}");
                if (LosJugadores.Any())
                {
                    int renglon = 1;
                    foreach (var jugadorx in LosJugadores)
                    {
                        if(!DatosDic.ContainsKey($"RenglonJugador_{jugadorx.Id}")) {
                            DatosDic.Add($"RenglonJugador_{jugadorx.Id}", renglon.ToString());
                            DatosDic.Add($"Tarjeta_{TarjetaHijo.Id}_Jugador_{jugadorx.Id}", jugadorx.Id.ToString());
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

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.Tarjeta
{
    public class ScoreListaBase : ComponentBase
    {
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        public Dictionary<string, string> LosDatos { get; set; } = new Dictionary<string, string>();

        [Inject]
        public IG120PlayerServ PlayersIServ { get; set; }
        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } = new List<KeyValuePair<string, string>>();
        [Inject]
        public IG510JugadorServ JugadoresIServ { get; set; }
        public IEnumerable<G510Jugador> LosJugadores { get; set; } = new List<G510Jugador>();
        
        [Inject]
        public IG520ScoreServ ScoresIServ { get; set; }
        public IEnumerable<G520Score> LosScores { get; set; } = new List<G520Score>();
        [Inject]
        public NavigationManager NM { get; set; }

        protected List<KeyValuePair<string, string>> NamesTemp { get; set; } =
                new List<KeyValuePair<string, string>>();
        protected List<KeyValuePair<string, string>> PlayerTemp { get; set; } =
                new List<KeyValuePair<string, string>>();
        public string ElScoreRenglon { get; set; } = string.Empty;
        public RadzenDataGrid<G520Score> ScoreGrid { get; set; } = new();

        protected async override Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(TarjetaId)) NM.NavigateTo("/tarjeta/tarjeta/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerNombres();
            await LeerJugadores();
            await LeerScores();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El Usuario consulto el listado de jugadores de una tarjeta");
        }

        protected async Task LeerNombres()
        {
            //List<KeyValuePair<string, string>> NamesTemp;
            var AllNames = await PlayersIServ.Filtro("All");
            
            foreach (var name in AllNames)
            {
               // NamesTemp.Add(new KeyValuePair<string, string>(name.UserId,
               //    $"{name.Nombre} {name.Apodo} {name.Paterno}"));
                if (!LosDatos.ContainsKey($"Nombre_{name.UserId}"))
                {
                    LosDatos.Add($"Nombre_{name.UserId}", $"{name.Nombre} {name.Apodo} {name.Paterno}");
                }

            }
            if (!LosDatos.ContainsKey("Vacio")) LosDatos.Add("Vacio", "No se encontro Jugador!");
        }
        protected async Task LeerJugadores()
        {
            //List<KeyValuePair<string, string>> NamesTemp;
            int renglon = 1;
            LosJugadores = await JugadoresIServ.Filtro($"jug1tarjeta_-_tarjeta_-_{TarjetaId}");
            foreach (var jugadors in LosJugadores)
            {
                if (!LosDatos.ContainsKey($"Jugador_{jugadors.Player}"))
                {
                    LosDatos.Add($"Jugador_{jugadors.Player}", jugadors.Player);
                    LosDatos.Add($"Renglon_{jugadors.Player}", renglon.ToString());
                    NamesTemp.Add(new KeyValuePair<string, string>(jugadors.Player, LosDatos[$"Nombre_{jugadors.Player}"])); 
                    renglon++;
                }
            }
            LosNombres = NamesTemp.AsEnumerable();
        }
        protected async Task LeerScores()
        {
            LosScores = await ScoresIServ.Filtro($"sco3tarjeta_-_tarjeta_-_{TarjetaId}");
            foreach (var LS in LosScores)
            {
                if (!LosDatos.ContainsKey($"Jugador_{LS.Player}_Hoyo_{LS.Hoyo}"))
                    LosDatos.Add($"Jugador_{LS.Player}_Hoyo_{LS.Hoyo}", LS.Score.ToString());
            }
        }
        public NotificationMessage ElMesage { get; set; } =
            new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Cuerpo",
                Detail = "Detalles ",
                Duration = 3000
            };

        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; } = string.Empty;
        
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

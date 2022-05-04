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
        public string ElScoreRenglon { get; set; } = string.Empty;

        public RadzenDataGrid<G520Score> ScoreGrid { get; set; } = new();


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

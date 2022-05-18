using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.torneo
{   
    public class JobTBase : ComponentBase
    {
        [Parameter]
        public int TorneoId { get; set; }
        [Inject]
        public IG202JobTServ JobIServ { get; set; }
        public IEnumerable<G202JobT> LosJobs { get; set; } = Enumerable.Empty<G202JobT>();
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        //public IEnumerable<G200Torneo> LosTorneos { get; set; } = new List<G200Torneo>();
        [Inject]
        public IG120PlayerServ playerIServ { get; set; }

        public string ElTorneo { get; set; } = string.Empty;
        
        public Dictionary<string, string> LosNombres { get; set; } = new Dictionary<string, string>();
        [Inject]
        public NavigationManager NM { get; set; }
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            LosJobs = await JobIServ.Buscar(TorneoId, "", "");
            await LeerTorneos();
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El Usuario consulto el listado de capturistas de un torneo {ElTorneo}");
        }
        protected async Task LeerTorneos()
        {
            var LosPlayers = await playerIServ.Filtro("All");
            foreach (var player in LosPlayers)
            {
                if (!LosNombres.ContainsKey(player.UserId)) LosNombres.Add(player.UserId,
                        $"{player.Nombre} {player.Apodo} {player.Paterno}");
            }
                LosNombres.Add("Vacio", "Nombre no encontrado");

            var t = await TorneoIServ.GetTorneo(TorneoId);
            ElTorneo = $"{t.Id} {t.Titulo} {LosNombres[t.Creador]}";
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

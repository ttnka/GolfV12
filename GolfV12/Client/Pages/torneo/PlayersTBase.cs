using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.torneo
{
    public class PlayersTBase : ComponentBase 
    {
        [Parameter]
        public int TeamTId { get; set; }
        [Parameter]
        public int TorneoId { get; set; }

        [Inject]
        public IG220TeamTServ TeamsTIServ { get; set; }

        [Inject]
        public IG222PlayerTServ PlayersTIServ { get; set; }
        public IEnumerable<G222PlayerT> LosPlayers { get; set; } = 
                Enumerable.Empty<G222PlayerT>();

        [Inject]
        public IG120PlayerServ JugadoresIServ { get; set; }
        public IEnumerable<G120Player> LosJugadores { get; set; } =
                Enumerable.Empty<G120Player>();
        public Dictionary<string, string> LosNombres { get; set; } = new Dictionary<string, string>();

        [Inject]
        public NavigationManager NM { get; set; }
        public Dictionary<string, int> Datos { get; set; } = new Dictionary<string, int>();

        protected async override Task OnInitializedAsync()
        {
            if (TeamTId == 0 || TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            await LeerDatos();
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El Usuario consulto el listado de jugadores de un equipo del torneo ");
        }

        protected async Task LeerDatos()
        {
            var ElTeam = await TeamsTIServ.GetTeam(TeamTId);
            LosPlayers = await PlayersTIServ.Buscar(TeamTId,"");
            LosJugadores = await JugadoresIServ.GetPlayers();
            foreach (var player in LosJugadores)
            {
                if (!LosNombres.ContainsKey(player.UserId)) 
                    LosNombres.Add(player.UserId,
                    $"{player.Nombre} {player.Apodo} {player.Paterno}");
            }
            LosNombres.Add("Vacio", "No se encontro Jugador!");
            LosNombres.Add("ElTeam", ElTeam.Nombre);
            Datos.Add("NumMaxPlayers", ElTeam.NumJugadores);
            Datos.Add("Players",  LosPlayers.Count());
            
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

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.torneo
{
    public class TorneoBase : ComponentBase 
    {
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public IEnumerable<G200Torneo> LosTorneos { get; set; } 
        [Inject]
        public IG120PlayerServ playerIServ { get; set; }
        public Dictionary<string, string> AllPlayers { get; set; } = new Dictionary<string, string>();
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            LosTorneos = await TorneoIServ.GetTorneos();
            await LeerPlayers();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario Consulto el listado de Torneos");
        }

        protected async Task LeerPlayers()
        {
            var Players = await playerIServ.GetPlayers();
            foreach (var player in Players)
            {
                if (!AllPlayers.ContainsKey(player.UserId)) AllPlayers.Add(player.UserId,
                    $"{player.Nombre} {player.Apodo} {player.Paterno}");
            }
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

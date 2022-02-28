using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.torneo
{
    public class TeamTBase : ComponentBase 
    {
        [Parameter]
        public int TorneoId { get; set; }

        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public string ElTorneo { get; set; } = string.Empty;

        [Inject]
        public IG220TeamTServ TeamIServ { get; set; }
        public IEnumerable<G220TeamT> LosTeams { get; set; } = Enumerable.Empty<G220TeamT>();
        
        [Inject]
        public IG121ElPlayerServ PlayerIServ { get; set; } 
        public Dictionary<string, string> LosNombres { get; set; } = new Dictionary<string,string>();
        
        [Inject]
        public NavigationManager NM { get; set; }
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            LosTeams = await TeamIServ.Buscar(TorneoId, 0, "");
            await LeerDatos();
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El Usuario consulto el listado de equipos del torneo {ElTorneo}");
        }
        protected async Task LeerDatos()
        {
            var CreadorTId = await TorneoIServ.GetTorneo(TorneoId);
            var LosPlayers = await PlayerIServ.GetPlayer(CreadorTId.Creador);  
            
            if (!LosNombres.ContainsKey(LosPlayers.UserId)) LosNombres.Add(LosPlayers.UserId,
                        $"{LosPlayers.Nombre} {LosPlayers.Apodo} {LosPlayers.Paterno}");
            
            LosNombres.Add("Vacio", "Nombre no encontrado");
            
            var t = await TorneoIServ.GetTorneo(TorneoId);
            ElTorneo = $"{t.Titulo} {t.Id} {LosNombres[t.Creador]}";
            
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

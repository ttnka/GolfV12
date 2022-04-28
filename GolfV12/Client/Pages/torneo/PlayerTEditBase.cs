using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.torneo
{
    public class PlayerTEditBase : ComponentBase 
    {
        [Parameter]
        public int TorneoId { get; set; }
        [Parameter]
        public int TeamTId { get; set; }
        [Parameter]
        public int PlayerTId { get; set; }
        public G222PlayerT ElPlayer { get; set; } = new G222PlayerT();
        [Inject]
        public IG120PlayerServ PlayersIServ { get; set; }
        [Inject]
        public IG222PlayerTServ JugadoresTIServ { get; set; }
        [Inject]
        public IG220TeamTServ TeamsIServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        public IEnumerable<KeyValuePair<string, string>> LosJugadores { get; set; }
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public G200Torneo ElTorneo { get; set; } = new G200Torneo();
        public G220TeamT ElTeam { get; set; } = new G220TeamT();
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            ElTorneo = await TorneoIServ.GetTorneo(TorneoId);
            ElTeam = await TeamsIServ.GetTeam(TeamTId);
            await LeerNombres();
           
            if (PlayerTId == 0)
            {
                ElPlayer.Team = TeamTId;
                ElPlayer.Player = "";
                ElPlayer.Hcp = 0;

                ButtonTexto = "Agregar";
            }
            else
            { ElPlayer = await JugadoresTIServ.GetPlayer(PlayerTId);  }

        }
        protected List<KeyValuePair<string, string>> NamesTemp { get; set; } =
                new List<KeyValuePair<string, string>>();
        protected async Task LeerNombres()
        {
            //List<KeyValuePair<string, string>> NamesTemp;
            var AllNames = await PlayersIServ.GetPlayers();
            foreach (var name in AllNames)
            {
                NamesTemp.Add(new KeyValuePair<string, string>(name.UserId,
                    $"{name.Nombre} {name.Apodo} {name.Paterno}"));
            }
            LosJugadores = NamesTemp.AsEnumerable();
        }
        public async Task SavePlayer()
        {
            G222PlayerT resultado = new G222PlayerT();
            if (PlayerTId == 0)
            {
                resultado = await JugadoresTIServ.AddPlayer(ElPlayer);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo jugador al equipo {resultado.Team} Con HCP {resultado.Hcp}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";
            }
            else
            {
                resultado = await JugadoresTIServ.UpdatePlayer(ElPlayer);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo los datos de un jugador {resultado.Id} ");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/torneo/playert/{TorneoId}/{TeamTId}");
        }
        
        public NotificationMessage ElMesage { get; set; } = new NotificationMessage()
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

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.torneo
{
    public class TeamTEditBase : ComponentBase 
    {
        [Parameter]
        public int TorneoId { get; set; }
        [Parameter]
        public int TeamId { get; set; }
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public G200Torneo ElTorneo { get; set; } = new G200Torneo();
        [Inject]
        public IG220TeamTServ TeamTServ { get; set; }
        public G220TeamT ElTeam { get; set; } = new G220TeamT();
        public int TeamIdNext { get; set; } = 1;
           
        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            await LeerTeam(); 
        }

        public async Task LeerTeam()
        {
            ElTorneo = await TorneoIServ.GetTorneo(TorneoId);
            var teams = await TeamTServ.Buscar(TorneoId, 0, "");
            if (teams != null) 
            {  
                foreach (var team in teams)
                {
                    if (TeamIdNext <= team.TeamNum) TeamIdNext = team.TeamNum + 1;
                    ButtonTexto = "Agregar";
                }
            }

            if (TeamId == 0)
            {
                ElTeam.Torneo = TorneoId;
                ElTeam.TeamNum = TeamIdNext;
                ElTeam.Nombre = "Nuevo";
                ElTeam.NumJugadores = 1;

                ButtonTexto = "Agregar";
            }
            else
            {
                ElTeam = await TeamTServ.GetTeam(TeamId);
            }
        }
        public async Task SaveTeam()
        {
            G220TeamT resultado = new G220TeamT();
            if (TeamId == 0)
            {
                resultado = await TeamTServ.AddTeam(ElTeam) ;
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo equipo al torneo {ElTorneo.Titulo} {ElTorneo.Campo}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";

            }
            else
            {
                resultado = await TeamTServ.UpdateTeam(ElTeam) ;
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo el equipo de del registro {resultado.Nombre} del torneo {ElTorneo.Titulo} {ElTorneo.Campo} ");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/torneo/teamt/{TorneoId}");
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

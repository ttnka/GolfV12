using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.torneo
{
    public class JobTEditBase : ComponentBase
    {
        [Parameter]
        public int JobTId { get; set; }
        [Parameter]
        public int TorneoId { get; set; }
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public G200Torneo ElTorneo { get; set; } = new G200Torneo();
        [Inject]
        public IG120PlayerServ PlayerISer { get; set; }
        public Dictionary<int, string> LosPlayerDic { get; set; } = new Dictionary<int, string>();
        public IEnumerable<G120Player> LosPlayers { get; set; }
        public IEnumerable<KeyValuePair<string, string>> LosPlayersDD { get; set; }
        public G120Player ElPlayer { get; set; }
        [Inject]
        public IG202JobTServ JobTIServ { get; set; }
        public G202JobT ElJobT { get; set; } = new G202JobT();

        public IEnumerable<JobTorneo> LosJobsT { get; set; } = Enum.GetValues(typeof(JobTorneo)).Cast<JobTorneo>().ToList();

        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;


            await LeerNombres();
            //LosPlayers = await PlayerISer.GetPlayers();
            ElTorneo = await TorneoIServ.GetTorneo(TorneoId);

            if (JobTId == 0)
            {
                ElJobT.Torneo = TorneoId;
                ElJobT.JobT = JobTorneo.Capturista;
                ElJobT.Contrincante = "";

                ButtonTexto = "Agregar";
            }
            else
            { ElJobT = await JobTIServ.GetJob(JobTId); }
        }
        protected List<KeyValuePair<string, string>> NamesTemp {get; set;} = 
                new List<KeyValuePair<string, string>>();  
        protected async Task LeerNombres()
        {
            //List<KeyValuePair<string, string>> NamesTemp;
            var AllNames = await PlayerISer.GetPlayers();
            foreach (var name in AllNames)
            {
                NamesTemp.Add(new KeyValuePair<string, string>(name.UserId,
                    $"{name.Nombre} {name.Apodo} {name.Paterno}"));
            }
            LosPlayersDD = NamesTemp.AsEnumerable();

        }
        
        public async Task SaveJob()
        {
            G202JobT resultado = new G202JobT();
            if (JobTId == 0)
            {
                resultado = await JobTIServ.AddJob(ElJobT);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo capturista al torneo {resultado.Id} {resultado.Torneo}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";

            }
            else
            {
                resultado = await JobTIServ.UpdateJob(ElJobT);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo el captura de del registro {resultado.Id} ");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/torneo/jobt/{TorneoId}");
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

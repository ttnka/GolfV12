using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.torneo
{
    public class FechaTEditBase : ComponentBase 
    { 
        [Parameter]
        public int TorneoId { get; set; }
        [Parameter]
        public int FechaId { get; set; }
        [Inject]
        public IG204FechaTServ FechaIServ { get; set; }
        public G204FechaT LaFecha { get; set; } = new G204FechaT();
        public IEnumerable<G204FechaT> LasFechas { get; set; } = Enumerable.Empty<G204FechaT>();
        public Dictionary<string, int> DatosFechas { get; set; } = new Dictionary<string, int>();

        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public G200Torneo ElTorneo { get; set; } = new G200Torneo(); 
        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            ElTorneo = await TorneoIServ.GetTorneo(TorneoId);
            
            if (FechaId == 0)
            {
                await LeerDatos();
            }
            else 
            { 
                LaFecha = await FechaIServ.GetFechaT(FechaId);
            }
        }

        protected async Task LeerDatos()
        {
            LaFecha.Torneo = TorneoId;
            LaFecha.Fecha = DateTime.Now;
            
            int rondaMayor = 0;
            LasFechas = await FechaIServ.Buscar(TorneoId, DateTime.MinValue);
            foreach (var f in LasFechas)
            {
                if (f.Ronda > rondaMayor) rondaMayor = f.Ronda;
            }

            DatosFechas.Add("RondaNext", rondaMayor + 1);
            DatosFechas.Add("DiaNext", DateTime.Now.Day);
            DatosFechas.Add("MesNext", DateTime.Now.Month);
            DatosFechas.Add("AnoNext", DateTime.Now.Year);
            LaFecha.Ronda = rondaMayor + 1;
        }

        public async Task SaveFecha()
        {
            G204FechaT resultado = new G204FechaT();
            if (FechaId == 0)
            {
                resultado = await FechaIServ.AddFechaT(LaFecha);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego una nueva fecha a las rondas al torneo {resultado.Ronda} {resultado.Torneo}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";
            }
            else
            {
                resultado = await FechaIServ.UpdateFechaT(LaFecha);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo la fecha de la ronda {resultado.Ronda} ");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/torneo/fechat/{TorneoId}");
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

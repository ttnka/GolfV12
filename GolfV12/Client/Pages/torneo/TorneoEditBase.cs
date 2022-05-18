using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.torneo
{
    public class TorneoEditBase : ComponentBase 
    {
        [Parameter]
        public int TorneoId { get; set; } 
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public G200Torneo ElTorneo { get; set; } = new G200Torneo();
        [Inject]
        public IG280FormatoTServ FormatoIServ { get; set; }
        public IEnumerable<G280FormatoT> LosFormatos { get; set; }
        [Inject]
        public IG170CampoServ CampoIServ { get; set;}
        public IEnumerable<G170Campo> LosCampos { get; set; } = new List<G170Campo>();
        
        public IEnumerable<TorneoView> TorneoViews { get; set; } = Enum.GetValues(typeof(TorneoView)).Cast<TorneoView>().ToList();
        public IEnumerable<Torneo2Edit> TorneoEdits { get; set; } = Enum.GetValues(typeof(Torneo2Edit)).Cast<Torneo2Edit>().ToList();  
        [Inject]
        public IG204FechaTServ FechaTServ { get; set; }
        public G204FechaT LaFecha { get; set; } = new G204FechaT();

        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            ElUser = (await PlayerIServ.Filtro($"play1id_-_userid_-_{UserIdLog}")).FirstOrDefault();

            LosFormatos = await FormatoIServ.GetFormatos();
            LosCampos = await CampoIServ.GetCampos();

            if (TorneoId == 0)
            {
                ElTorneo.Ejercicio = DateTime.Now.Year;
                ElTorneo.Titulo = "Nuevo";
                ElTorneo.Campo = 1;
                ElTorneo.Formato = 1;
                ElTorneo.Creador = UserIdLog;
                ButtonTexto = "Agregar nuevo ";
            } 
            else 
           {
                ElTorneo = await TorneoIServ.GetTorneo(TorneoId);
            }
        }
        
        public async Task SaveTorneo()
        {
            G200Torneo resultado = null;
            if (string.IsNullOrEmpty(ElTorneo.Desc)) ElTorneo.Desc = " ";
            if (TorneoId == 0)
            {
                if (LaFecha.Fecha.Date < DateTime.Now.Date) LaFecha.Fecha = DateTime.Now.Date;
                if (LaFecha.Fecha.Date > DateTime.Now.AddDays(370)) LaFecha.Fecha = DateTime.Now.Date;
                
                ElTorneo.Ejercicio = LaFecha.Fecha.Year; 
                resultado = await TorneoIServ.AddTorneo(ElTorneo);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo Torneo {resultado.Titulo} {resultado.Ejercicio}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";

                LaFecha.Torneo = resultado.Id;
                LaFecha.Ronda = 1;
                
                LaFecha.Estado = 1;
                await FechaTServ.AddFechaT(LaFecha); 
            }
            else
            {
                resultado = await TorneoIServ.UpdateTorneo(ElTorneo);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo la info del Torneo {resultado.Titulo} {resultado.Ejercicio}");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }

            if (resultado != null) NM.NavigateTo("/torneo/torneo");
        }
        public NotificationMessage ElMesage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Cuerpo",
            Detail = "Detalles ",
            Duration = 3000
        };

        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }
        public G120Player ElUser { get; set; } = new G120Player();
        
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

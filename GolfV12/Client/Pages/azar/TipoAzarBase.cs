using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.azar
{
    public class TipoAzarBase : ComponentBase 
    {
        public IEnumerable<G390TiposAzar> TipoAzar { get; set; } = new List<G390TiposAzar>();
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<G390TiposAzar> TiposAzarGrid { get; set; } = new();
        [Inject]
        public NavigationManager NM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerDatos();
            await LeerNombres();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario Consulto sus datos");
        }
        public NotificationMessage elMesage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Cuerpo",
            Detail = "Detalles ",
            Duration = 3000
        };
        [Inject]
        public NotificationService NS { get; set; } = new();

        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; } = String.Empty;
        // Bitacora

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

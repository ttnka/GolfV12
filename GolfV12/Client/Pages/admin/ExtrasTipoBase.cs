using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.admin
{
    public class ExtrasTipoBase : ComponentBase 
    {
        [Inject]
        public IG250ExtrasTipoServ ExtTipoIServ { get; set; } 
        public IEnumerable<G250ExtrasTipo> LosExtrasTipo { get; set; } = new List<G250ExtrasTipo>();
        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }
        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } =
            new List<KeyValuePair<string, string>>();
        protected List<KeyValuePair<string, string>> NombresList { get; set; } =
                new List<KeyValuePair<string, string>>();
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<G250ExtrasTipo> ExtrasTipoGrid { get; set; } = new();
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

        public async Task LeerDatos()
        {
            LosExtrasTipo = await ExtTipoIServ.Filtro("All");
        }
        protected async Task LeerNombres()
        {
            var NameTemp = await PlayerIServ.Filtro("all");
            foreach (var t in NameTemp)
            {
                if (!DatosDic.ContainsKey($"Nombre_{t.UserId}"))
                {
                    DatosDic.Add($"Nombre_{t.UserId}", $"{t.Nombre} {t.Apodo} {t.Paterno}");
                    NombresList.Add(new KeyValuePair<string, string>(t.UserId, $"{t.Nombre} {t.Apodo} {t.Paterno}"));
                }
            }
            LosNombres = NombresList.AsEnumerable();
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

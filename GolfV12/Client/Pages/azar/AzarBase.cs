using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.azar
{
    public class AzarBase : ComponentBase
    {
        [Inject]
        public IG300AzarServ AzarIServ { get; set; }
        [Inject]
        public IG390TiposAzarServ TiposAzarIServ { get; set; }

        public IEnumerable<G300Azar> LosAzar { get; set; } = new List<G300Azar>();
        public IEnumerable<KeyValuePair<string, string>> TiposAzar { get; set; } =
            new List<KeyValuePair<string, string>>();
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<G300Azar> AzarGrid { get; set; } = new();
        [Inject]
        public NavigationManager NM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerDatos();
            //await LeerNombres();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario Consulto los retos");
        }
        protected async Task LeerDatos()
        {
            LosAzar = await AzarIServ.Filtro($"azart4creador_-_creador_-_{UserIdLog}");
            if (LosAzar != null)
            {
                int renglon = 1;
                foreach (var tipo in LosAzar)
                {
                    if (!DatosDic.ContainsKey($"Renglon_{tipo.Id}"))
                    {
                        DatosDic.Add($"Renglon_{tipo.Id}", (renglon).ToString());
                        renglon++;
                    }
                }
            }
        
        List<KeyValuePair<string, string>> ListTiposATemp = new List<KeyValuePair<string, string>>();
        var ListTA = await TiposAzarIServ.Filtro("All");
            if (ListTA != null)
            {
                foreach (var item in ListTA)
                {
                    if (!DatosDic.ContainsKey($"TipoAzar_{item.Id}"))
                    {
                        DatosDic.Add($"TipoAzar_{item.Id}", item.Titulo);
                        ListTiposATemp.Add(new KeyValuePair<string, string>(item.Id, item.Titulo));
                    }
                }
            }
            TiposAzar = ListTiposATemp;
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

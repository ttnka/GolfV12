using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players.retos
{
    public class LasBolitasBase : ComponentBase 
    {
        [Inject]
        public IG320BolitasServ BolitasIServ { get; set; }
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        [Parameter]
        public string AzarId { get; set; } = string.Empty;
        [Parameter]
        public Dictionary<string, G120Player> JugadoresDic { get; set; } = new Dictionary<string, G120Player>();
        public IEnumerable<G320Bolitas> LasBolitas { get; set; } = new List<G320Bolitas>();
        public IEnumerable<KeyValuePair<string, string>> TiposAzar { get; set; } =
            new List<KeyValuePair<string, string>>();
        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }
        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } =
            new List<KeyValuePair<string, string>>();
        [Inject]
        public IG300AzarServ AzarIServ { get; set; }
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<G320Bolitas> BolitasGrid { get; set; } = new();
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
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            
            foreach (var item in JugadoresDic)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(item.Key, $"{item.Value.Nombre} {item.Value.Apodo} {item.Value.Paterno}"));
            }
            LosNombres = keyValuePairs.AsEnumerable();
            LasBolitas = await BolitasIServ.Filtro($"bol2tarjeta_-_tarjeta_-_{TarjetaId}");
            if (LasBolitas != null)
            {
                int renglon = 1;
                foreach (var tipo in LasBolitas)
                {
                    if (!DatosDic.ContainsKey($"Renglon_{tipo.Id}"))
                    {
                        DatosDic.Add($"Renglon_{tipo.Id}", (renglon).ToString());
                        renglon++;
                    }
                }
            }
        }
        public NotificationMessage elMessage { get; set; } = new NotificationMessage()
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

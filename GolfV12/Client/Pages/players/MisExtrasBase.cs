using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class MisExtrasBase : ComponentBase 
    {
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        [Parameter]
        public G500Tarjeta LaTarjeta { get; set; } = new G500Tarjeta();
        [Inject]
        public IG242ExtrasServ ExtrasIServ { get; set; }
        [Parameter]
        public IEnumerable<G242Extras> LosExtras { get; set; } = new List<G242Extras>();
        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }
        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } =
            new List<KeyValuePair<string, string>>();
        
        [Inject]
        public IG250ExtrasTipoServ ExtTipoIServ { get; set; }
        public IEnumerable<KeyValuePair<int, string>> LosExtrasTipo { get; set; } =
            new List<KeyValuePair<int, string>>();

        [Parameter]
        public IEnumerable<G510Jugador> LosJugadores { get; set; } = new List<G510Jugador>();
        [Parameter]
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public IEnumerable<int> LosHoyos { get; set; } = new List<int>();
        public RadzenDataGrid<G242Extras> ExtrasGrid { get; set; } = new();
        [Inject]
        public NavigationManager NM { get; set; }
        
        [Parameter]
        public EventCallback<G242Extras> OnInsertExtra { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerDatos();
            LeerLosHoyos();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario Consulto sus datos");
        }
        protected async Task LeerDatos()
        {
            List<KeyValuePair<string, string>> NombresList = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<int, string>> ExtTipoList = new List<KeyValuePair<int, string>>();

            LosExtras = await ExtrasIServ.Filtro($"ext2tarjeta_-_tarjeta_-_{TarjetaId}");

            if (LosJugadores != null)
            {
                
                foreach (var item in LosJugadores)
                {
                    if (!DatosDic.ContainsKey($"Nombre_{item.Player}"))
                    {
                        var NameTemp = (await PlayerIServ.Filtro($"play1id_-_userid_-_{item.Player}")).FirstOrDefault();
                        if (NameTemp != null)
                        {
                            DatosDic.Add($"Nombre_{NameTemp.UserId}", 
                                                    $"{NameTemp.Nombre} {NameTemp.Apodo} {NameTemp.Paterno}");
                            
                        }
                    }
                    NombresList.Add(new KeyValuePair<string, string>(item.Player,
                                                    DatosDic[$"Nombre_{item.Player}"]));
                }   
            }
            LosNombres = NombresList.AsEnumerable();
            var ExtraTTemp = await ExtTipoIServ.Filtro($"exttipo4creador_-_creador_-_{UserIdLog}");
            if (ExtraTTemp != null)
            {
                foreach (var item in ExtraTTemp)
                {
                    if (!DatosDic.ContainsKey($"ExtraTipo_{item.Id}"))
                    {
                        DatosDic.Add($"ExtraTipo_{item.Id}",
                                    $"{item.Titulo} Valor {item.Valor} ");
                        DatosDic.Add($"ExtraValor_{item.Id}",
                                    $"{item.Valor}");
                        ExtTipoList.Add(new KeyValuePair<int, string>(item.Id,
                                    $"{item.Titulo} Valor {item.Valor}"));
                    }
                }
            }
            LosExtrasTipo = ExtTipoList.AsEnumerable();
        }
        protected void LeerLosHoyos()
        {
            IList<int> ListaTemp = new List<int>();
            for (int i = 1; i < 19; i++)
            {
                ListaTemp.Add(i);
            }
            LosHoyos = ListaTemp.AsEnumerable();
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

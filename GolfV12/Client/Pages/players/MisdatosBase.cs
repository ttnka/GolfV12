using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;


namespace GolfV12.Client.Pages.players
{
    public class MisdatosBase : ComponentBase
    {

        [Inject]
        protected IG121ElPlayerServ ElPlayerServ { get; set; }
        [Inject]
        protected IG120PlayerServ PlayerIServ { get; set; }

        public IEnumerable<KeyValuePair<string, string>> NombresGeneral { get; set; } =
            new List<KeyValuePair<string, string>>();
        protected List<KeyValuePair<string, string>> NombresList { get; set; } =
                new List<KeyValuePair<string, string>>();

        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        public IEnumerable<KeyValuePair<int, string>> CamposGeneral { get; set; } =
                new List<KeyValuePair<int, string>>();
        protected List<KeyValuePair<int, string>> CamposList { get; set; } =
                new List<KeyValuePair<int, string>>();

        [Inject]
        public IG500TarjetaServ TarjetaIServ { get; set; }
        public G500Tarjeta TarjetaGeneral { get; set; } = new G500Tarjeta();
        //public G502Tarjetas CardGeneral { get; set; } = new G502Tarjetas();
        
        [Inject]
        public IG510JugadorServ JugadorIServ { get; set; }
        public IEnumerable<G510Jugador> JugadoresGeneral { get; set; } = new List<G510Jugador>();

        public Dictionary<string, string> DicGeneral { get; set; } = new Dictionary<string, string>();

        public int selectedIndex { get; set; } = 0;
        
        public bool EditarMisDatos { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerNombres();
            await LeerCampos();
            //await LeerJugadores();

            
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario Consulto sus datos");
        }
        protected void AgregarJugadores(G500Tarjeta tarj)
        {
            selectedIndex = 1;
            TarjetaGeneral = tarj;
            //await LeerJugadores();
        } 
        
        protected async Task LeerNombres()
        {
            var NameTemp = await PlayerIServ.Filtro("all");
            foreach (var t in NameTemp)
            {
                if (!DicGeneral.ContainsKey($"Nombre_{t.UserId}"))
                {
                    DicGeneral.Add($"Nombre_{t.UserId}", $"{t.Nombre} {t.Apodo} {t.Paterno}");
                    NombresList.Add(new KeyValuePair<string, string>(t.UserId, $"{t.Nombre} {t.Apodo} {t.Paterno}"));
                }
            }
            NombresGeneral = NombresList.AsEnumerable();
        }
        protected async Task LeerCampos()
        {
            var CampoTemp = await CampoIServ.GetCampos();
            foreach (var C in CampoTemp)
            {
                if (!DicGeneral.ContainsKey($"Campo_{C.Id}"))
                {
                    DicGeneral.Add($"Campo_{C.Id}", $"{C.Corto} {C.Ciudad}");
                    CamposList.Add(new KeyValuePair<int, string>(C.Id, $"{C.Corto} {C.Ciudad}"));
                }
            }
            CamposGeneral = CamposList.AsEnumerable();
        }
        /*
        protected async Task LeerJugadores()
        {
            var JugadorTemp = await JugadorIServ.Filtro("All");
            
            if (JugadorTemp.Any())
            {
                foreach (var t in JugadorTemp)
                {
                    if(!DicGeneral.ContainsKey($"Tarjeta_{t.Tarjeta}_jugador_{t.Player}"))
                    {
                        DicGeneral.Add($"Tarjeta_{t.Tarjeta}_jugador_{t.Player}", t.Player.ToString());
                    }
                    if (t.Player == UserIdLog) ListaParticipa(t.Tarjeta);
                    if (!DicGeneral.ContainsKey($"JugadoresXtarjeta_{t.Tarjeta}"))
                    {
                        DicGeneral.Add($"JugadoresXtarjeta_{t.Tarjeta}", t.Player);
                    }
                    else
                    {
                        DicGeneral[$"JugadoresXtarjeta_{t.Tarjeta}"] += $",{t.Player}";
                    }       
                }

                
//           DicGeneral.Add($"JugadoresTarjeta_{t}", JugadorTemp.Count(e => e.Tarjeta == t).ToString());

            } 

            JugadoresGeneral = (await JugadorIServ.Filtro($"jug2tarjeta_-_tarjeta_-_{TarjetaGeneral.Id}")).ToList();
        }
        */
        /*
        protected void ListaParticipa(string tarjId)
        {
            if (DicGeneral.ContainsKey($"Participa_{UserIdLog}"))
            {
                DicGeneral[$"Participa_{UserIdLog}"] += $",{tarjId}";
            }
            else
            {
                DicGeneral.Add($"Participa_{UserIdLog}", tarjId);
            }
        }
        */
        // Mensaje de Actualizacion
        public NotificationMessage elMesage { get; set; } = new NotificationMessage() { 
                Severity = NotificationSeverity.Success, Summary = "Cuerpo", Detail = "Detalles ", Duration = 3000 };
    
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

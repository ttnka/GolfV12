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
        
        [Inject]
        public IG510JugadorServ JugadorIServ { get; set; }
        public IEnumerable<G510Jugador> JugadoresGeneral { get; set; } = new List<G510Jugador>();
        protected List<KeyValuePair<int, string>> JugadoresList { get; set; } =
                new List<KeyValuePair<int, string>>();

        public Dictionary<string, string> DicGeneral { get; set; } = new Dictionary<string, string>();

        public int selectedIndex { get; set; } = 0;
        protected G120Player Midata { get; set; } = new G120Player();
        //protected WBita WB { get; set; } = new WBita();
        public bool EditarMisDatos { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerNombres();
            await LeerCampos();
            await LeerTarjetas();
            await LeerJugadores();
            

            Midata = await ElPlayerServ.GetPlayer(UserIdLog);
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "Consulto sus datos");
        }
        protected async void AgregarJugadores(G500Tarjeta tarj)
        {
            selectedIndex = 1;
            TarjetaGeneral = tarj;
            await LeerJugadores();
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
        protected async Task LeerTarjetas()
        {
            var TarjTemp = await TarjetaIServ.Filtro($"tar2creador_-_creador_-_{UserIdLog}");
            var renglon = 1;
            foreach (var t in TarjTemp)
            {

                if (!DicGeneral.ContainsKey($"CreadorTarjeta_{t.Creador}"))
                {
                    DicGeneral.Add($"CreadorTarjeta_{t.Creador}", t.Id);
                    DicGeneral.Add($"RenglonTarjeta_{t.Id}", renglon.ToString());
                    renglon++;
                }
                if (!DicGeneral.ContainsKey($"TarjetasCreador"))
                {
                    DicGeneral.Add($"TarjetasCreador", $"{t.Id}");
                } else
                {
                    DicGeneral[$"TarjetasCreador"] += $",{t.Id}";
                }
            }
        }
        
        protected async Task LeerJugadores()
        {
            var JugadorTemp = await JugadorIServ.Filtro("All");
            if (DicGeneral.ContainsKey($"TarjetasCreador"))
            {
                var tarjetasTemp = DicGeneral[$"TarjetasCreador"].Split(",");
                if (tarjetasTemp.Any())
                {
                    foreach (var t in tarjetasTemp)
                    {
                        if (!DicGeneral.ContainsKey($"JugadoresTarjeta_{t}"))
                        {
                            DicGeneral.Add($"JugadoresTarjeta_{t}", JugadorTemp.Count(e => e.Tarjeta == t).ToString());
                        }
                    }
                }
            }
            if (JugadorTemp.Any())
            {
                foreach (var t in JugadorTemp)
                {
                    if(!DicGeneral.ContainsKey($"Tarjeta_{t.Tarjeta}_jugador_{t.Player}"))
                    {
                        DicGeneral.Add($"Tarjeta_{t.Tarjeta}_jugador_{t.Player}", t.Player.ToString());
                    }               
                }
            } 

            JugadoresGeneral = (await JugadorIServ.Filtro($"jug2tarjeta_-_tarjeta_-_{TarjetaGeneral.Id}")).ToList();
        }
        
        public async Task MisDatosUpdate()
        {
            var resultado = await PlayerIServ.UpdatePlayer(Midata);
            if (resultado != null)
            {
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"Actualizo sus datos Nombre {Midata.Nombre} Apellido {Midata.Paterno} {Midata.Materno} " +
                    $"Apodo {Midata.Apodo} {Midata.Estado}");
                elMesage.Summary = "Registro Actualizado ";
                elMesage.Detail = "Exitosamente!!!";
                EditarMisDatos = true;
            }
        }

        

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

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Authorization;


namespace GolfV12.Client.Pages.admin
{
    public class PlayerEditBase : ComponentBase
    {
        [Inject]
        public IG110OrganizacionServ iOrgServ { get; set; }
        [Inject]
        public IG120PlayerServ iPlayerServ { get; set; }
        [Inject]
        public IG121ElPlayerServ elPlayerServ { get; set; }
        [Inject]
        public IG180EstadoServ iEstadoServ { get; set; }
        
        [Inject]
        public NavigationManager NM { get; set; }
        
        public IEnumerable<G110Organizacion> LasOrg { get; set; }
        public int UnaOrg { get; set; }  
        public IEnumerable<G180Estado> LosEstados { get; set; } 
        //public int ElEstado { get; set; } 
        public IEnumerable<Niveles> LosNiveles  { get; set; } = Enum.GetValues(typeof(Niveles)).Cast<Niveles>().ToList();
        //public int ElNivel { get; set; }

        [Parameter]
        public string userId { get; set; }

        public G120Player ElPlayer { get; set; } = new G120Player();
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            LasOrg = await iOrgServ.GetOrganizaciones();
            if (userId.Contains("Temp"))
            {
                ElPlayer = new G120Player
                {
                    //UserId = "Temp" + DateTime.Now.ToString(),
                    UserId = "Temp" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                            DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString(), 
                    Nombre = "Nombre Jugador Temporal", Paterno = "Paterno Jugador Temporal",
                    Materno = "", Apodo = " ",
                    Bday = DateTime.Now, OrganizacionId = 2, Nivel=0,
                    Estado = 1, Status = true, Temporal = true
                };
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Agregar, true,
                    "El usuario agrego un jugador temporal");
            } else
            {
                ElPlayer = await elPlayerServ.GetPlayer(userId);
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false,
                    $"El usuario consulto un jugador {ElPlayer.Nombre} {ElPlayer.Paterno} {ElPlayer.Materno}");
            }
            LosEstados = await iEstadoServ.Buscar("V", "Player");
            //ElEstado = ElPlayer.Estado;
        }

        protected async Task OnSubmit(G120Player updatePlayer)
        {
            G120Player resultado = null;
            
            if (userId.Contains("Temp"))
            {
                resultado = await iPlayerServ.AddPlayer(updatePlayer);
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Agregar, false, 
                    $"Agrego un nuevo registro {updatePlayer.Nombre} {updatePlayer.Apodo} {updatePlayer.Paterno} {updatePlayer.Materno}");
            } else
            {
                resultado = await iPlayerServ.UpdatePlayer(updatePlayer);
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Editar, false,
                    $"Edito registro {updatePlayer.Id} {updatePlayer.Apodo} {updatePlayer.Paterno} {updatePlayer.Materno}");
            }
            if( resultado != null) NM.NavigateTo("/admin/player");
        }

        IEnumerable<DateTime> dates = new DateTime[] {DateTime.Today};    
        protected void DateRender(DateRenderEventArgs args)
        {
            for (int i = 1; i < 30; i++)
                { dates.Append<DateTime>(DateTime.Today.AddDays(-i)); }
            
            args.Disabled = dates.Contains(args.Date);
        }
        
        [CascadingParameter]
        public Task<AuthenticationState> authStateTask { get; set; }
        public string userIdLog { get; set; }
        [Inject]
        public IG190BitacoraServ bitacoraServ { get; set; }
        private G190Bitacora writeBitacora { get; set; } = new G190Bitacora();
        public async Task EscribirBitacoraUno(string userId, BitaAcciones accion, bool Sistema, string desc)
        {
            writeBitacora.Fecha = DateTime.Now;
            writeBitacora.Accion = accion;
            writeBitacora.Sistema = Sistema;
            writeBitacora.UsuarioId = userId;
            writeBitacora.Desc = desc;
            await bitacoraServ.AddBitacora(writeBitacora);
        }
        
    }
}

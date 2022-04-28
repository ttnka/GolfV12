using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.admin
{
    public class PlayerBase : ComponentBase
    {
        [Inject]
        public IG120PlayerServ iG120PlayerServ { get; set; }
        [Inject]
        public IG110OrganizacionServ  iG110OrgServ { get; set; }
        public IEnumerable<G120Player> LosJugadores { get; set; } = Enumerable.Empty<G120Player>();
        public Dictionary<int, string> LasOrg { get; set; } = new Dictionary<int, string>();      
        public NavigationManager MN { get; set; }
        //protected WBita WB { get; set; } = new WBita(); 
        public int renglonNumber  { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            LosJugadores = await iG120PlayerServ.GetPlayers();
            await OrganizacionesLeer();
            await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false,
                "Consulto el listado de jugadores");
           
        }
        public async Task OrganizacionesLeer()
        {
            LasOrg.Add(0, "No hay datos");
            var OrgAll = await iG110OrgServ.Filtro("org1_-_id_-_0");
            foreach(var org in OrgAll)
            {
                if (!LasOrg.ContainsKey(org.Id)) LasOrg.Add(org.Id, $"{org.Clave} {org.Nombre}");
            }
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

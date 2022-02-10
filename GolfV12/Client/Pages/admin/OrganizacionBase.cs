using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.admin
{
    public class OrganizacionBase : ComponentBase
    {
        //[Inject]
        //public NavigationManager MN { get; set; }
        
        [Inject]
        public IG110OrganizacionServ organizacionIServ { get; set; }
        public IEnumerable<G110Organizacion> LasOrganizaciones { get; set; }
        //protected WBita WB { get; set; } = new WBita();
        protected override async Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            LasOrganizaciones = await organizacionIServ.GetOrganizaciones();
            await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false,
                "Consulto listado de Organizaciones");
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

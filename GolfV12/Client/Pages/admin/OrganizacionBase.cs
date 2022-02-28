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
        public IEnumerable<G110Organizacion> LasOrganizaciones { get; set; } = Enumerable.Empty<G110Organizacion>();
        //protected WBita WB { get; set; } = new WBita();
        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            LasOrganizaciones = await organizacionIServ.GetOrganizaciones();
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "Consulto listado de Organizaciones");
        }
        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; }
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

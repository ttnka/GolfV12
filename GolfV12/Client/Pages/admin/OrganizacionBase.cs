using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.admin
{
    public class OrganizacionBase : ComponentBase
    {
        [Inject]
        public NavigationManager MN { get; set; }
        
        [Inject]
        public IG110OrganizacionServ organizacionIServ { get; set; }
        public IEnumerable<G110Organizacion> LasOrganizaciones { get; set; }
        protected WBita WB { get; set; } = new WBita();
        protected override async Task OnInitializedAsync()
        {
            LasOrganizaciones = await organizacionIServ.GetOrganizaciones();
            await EscribirBitacoraUno(2, BitaAcciones.Consultar, false,
                "Consulto listado de Organizaciones");
        }

        [Inject]
        public IG190BitacoraServ bitacoraServ { get; set; }
        private G190Bitacora writeBitacora { get; set; } = new G190Bitacora();
        public async Task EscribirBitacoraUno(int usuario, BitaAcciones accion, bool Sistema, string desc)
        {
            writeBitacora.Fecha = DateTime.Now;
            writeBitacora.Accion = accion;
            writeBitacora.Sistema = Sistema;
            writeBitacora.UsuarioId = usuario;
            writeBitacora.Desc = desc;
            await bitacoraServ.AddBitacora(writeBitacora);

        }

    }
}

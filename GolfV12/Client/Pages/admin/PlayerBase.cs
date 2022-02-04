using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.admin
{
    public class PlayerBase : ComponentBase
    {
        [Inject]
        public IG120PlayerServ iG120PlayerServ { get; set; }
        [Inject]
        public IG110OrganizacionServ  iG110OrgServ { get; set; }
        public IEnumerable<G120Player> LosJugadores { get; set; }
        public Dictionary<int, string> LasOrg { get; set; } = new Dictionary<int, string>();
        public NavigationManager MN { get; set; }
        //protected WBita WB { get; set; } = new WBita(); 
        public int renglonNumber  { get; set; }
        protected override async Task OnInitializedAsync()
        {
            LosJugadores = await iG120PlayerServ.GetPlayers();
            await OrganizacionesLeer();
            await EscribirBitacoraUno(2, BitaAcciones.Consultar, false,
                "Consulto el listado de jugadores");
        }

        public async Task OrganizacionesLeer()
        {
            LasOrg.Add(0, "No hay datos");
            var OrgAll = await iG110OrgServ.GetOrganizaciones();
            foreach(var org in OrgAll)
            {
                if (!LasOrg.ContainsKey(org.Id)) LasOrg.Add(org.Id, $"{org.Clave} {org.Nombre}");
            }
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

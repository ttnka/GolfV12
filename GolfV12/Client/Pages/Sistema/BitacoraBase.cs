using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.Sistema
{
    public class BitacoraBase : ComponentBase
    {
        [Inject]
        public IG190BitacoraServ bitacoraServ { get; set; }
        [Inject]
        public IG120PlayerServ playerServ { get; set; }
        private G120Player elUsuario { get; set; } = new G120Player();

        public IEnumerable<G190Bitacora> BitacoraAll { get; set; }
        private G190Bitacora WriteBitacora { get; set; } = new G190Bitacora();
        
        [Parameter]
        public string Id { get; set; }
        protected async override Task OnInitializedAsync()
        {
            BitacoraAll = (await bitacoraServ.GetBitacoraAll()).ToList();
            elUsuario = await playerServ.GetPlayer(2);
            await EscribirBitacora(BitaAcciones.Consultar, false,
                "Consulto el listado de la bitacora.");
                
        }

        protected async Task EscribirBitacora(BitaAcciones accion, bool Sistema, string desc )
        {
            WriteBitacora.Fecha = DateTime.Now;
            WriteBitacora.Accion = accion;
            WriteBitacora.Sistema = Sistema;
            WriteBitacora.Usuario = elUsuario;
            WriteBitacora.Desc = desc;
            await bitacoraServ.AddBitacora(WriteBitacora);
            
        }
    }
}

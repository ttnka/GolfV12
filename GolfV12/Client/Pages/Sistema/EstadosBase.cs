using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.Sistema
{
    public class EstadosBase : ComponentBase
    {
        [Inject]
        public IG180EstadoServ EdosIServ { get; set; }
    
        public IEnumerable<G180Estado> LosEstados { get; set; }
        
        protected async override Task OnInitializedAsync()
        {
            LosEstados = await EdosIServ.GetEstados();
            await EscribirBitacoraUno(2, BitaAcciones.Consultar, false, 
                "Consulto Listado de Estados de registros");
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

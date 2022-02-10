using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.Sistema
{
    public class EstadosBase : ComponentBase
    {
        [Inject]
        public IG180EstadoServ EdosIServ { get; set; }
    
        public IEnumerable<G180Estado> LosEstados { get; set; }
        
        protected async override Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            LosEstados = await EdosIServ.GetEstados();
            await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false, 
                "Consulto Listado de Estados de registros");
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

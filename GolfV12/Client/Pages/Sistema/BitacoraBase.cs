using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace GolfV12.Client.Pages.Sistema
{
    public class BitacoraBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> authStateTask { get; set; }
        public string userIdLog { get; set; }

        [Inject]
        public IG190BitacoraServ bitacoraServ { get; set; }
        
        [Inject]
        public IG120PlayerServ playerServ { get; set; }
        [Inject]
        public IG121ElPlayerServ elPlayerServ { get; set; }

        public G120Player elUsuario { get; set; } = new G120Player();

        public Dictionary<string, string> todosPlayer { get; set; } = new Dictionary<string, string>();

        public IEnumerable<G190Bitacora> bitacoraAll { get; set; }
        //private G190Bitacora writeBitacora { get; set; } = new G190Bitacora();
        //public WBita WB  = new WBita();
        [Parameter]
        public string Id { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            elUsuario = await playerServ.GetPlayer(userIdLog);
            await NombresEscritore();
            await EscribirBitacoraUno(elUsuario.UserId, BitaAcciones.Consultar, false,
                "Consulto el listado de la bitacora."); 
            bitacoraAll = (await bitacoraServ.GetBitacoraAll()).ToList(); 
        }

        public async Task NombresEscritore()
        {
            todosPlayer.Add("", "No hay nombre");
            var AllNames =  await playerServ.GetPlayers();
            foreach( var nombres in AllNames )
            {
                if (!todosPlayer.ContainsKey(nombres.UserId))
                { todosPlayer.Add(nombres.UserId, $"{nombres.Nombre} {nombres.Apodo} {nombres.Paterno}"); }
            }
        }

        //[Inject]
        //public IG190BitacoraServ bitacoraServ { get; set; }
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

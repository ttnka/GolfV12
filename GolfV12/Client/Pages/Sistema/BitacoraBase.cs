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
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; }

        [Inject]
        public IG190BitacoraServ BitacoraServ { get; set; }
        
        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }
        [Inject]
        public IG121ElPlayerServ ElPlayerServ { get; set; }

        public G120Player ElUsuario { get; set; } = new G120Player();

        public Dictionary<string, string> TodosPlayer { get; set; } = new Dictionary<string, string>();

        public IEnumerable<G190Bitacora> BitacoraAll { get; set; }
        
        [Parameter]
        public string Id { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            ElUsuario = (await PlayerIServ.Filtro($"play1id_-_userid_-_{UserIdLog}")).FirstOrDefault() ;
            await NombresEscritore();
            await EscribirBitacoraUno(ElUsuario.UserId, BitaAcciones.Consultar, false,
                "Consulto el listado de la bitacora."); 
            BitacoraAll = (await BitacoraServ.GetBitacoraAll()).ToList(); 
        }

        protected async Task NombresEscritore()
        {
            TodosPlayer.Add("Vacio", "No hay nombre");
            var AllNames =  await PlayerIServ.Filtro("All");
            foreach( var nombres in AllNames )
            {
                if (!TodosPlayer.ContainsKey(nombres.UserId))
                { TodosPlayer.Add(nombres.UserId, $"{nombres.Nombre} {nombres.Apodo} {nombres.Paterno}"); }
            }
        }

        //[Inject]
        //public IG190BitacoraServ bitacoraServ { get; set; }
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

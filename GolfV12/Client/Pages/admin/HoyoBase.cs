using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.admin
{
    public class HoyoBase : ComponentBase
    {
        [Parameter]
        public int CampoId { get; set; }
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        [Inject]
        public IG176HoyoServ HoyoIServ { get; set; } 
        public G170Campo ElCampo { get; set; } = new G170Campo();   
        public IEnumerable<G176Hoyo> LosHoyos { get; set; } = Enumerable.Empty<G176Hoyo>();
        public NavigationManager NM { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            if (CampoId == 0) NM.NavigateTo("/admin/campo/");
            LosHoyos = await HoyoIServ.Buscar(CampoId, "", 0);
            ElCampo = await CampoIServ.GetCampo(CampoId);
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false, 
                    $"El Usuario consulto listado de hoyos de campo {ElCampo.Corto}");
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

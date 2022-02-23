using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.admin
{
    public class DistanciaBase : ComponentBase 
    {
        [Parameter]
        public int BanderaId { get; set; }
        [Parameter]
        public int CampoId { get; set; }

        [Inject]
        public IG178DistanciaServ DistIServ { get; set; }
        [Inject]
        public IG172BanderaServ BanderaIServ { get; set; }
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        public IEnumerable<G178Distancia> LasDistancias { get; set; } = Enumerable.Empty<G178Distancia>();
        public IEnumerable<G172Bandera> LasBanderas { get; set; } = Enumerable.Empty<G172Bandera>();
        public G170Campo ElCampo { get; set; } = new G170Campo();
        public G172Bandera LaBandera { get; set; }
        public NavigationManager NM { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            if (CampoId == 0) NM.NavigateTo("/admin/campo/");
            if (BanderaId == 0) NM.NavigateTo("/admin/bandera/");
           
            LasDistancias = await DistIServ.Buscar(BanderaId, 0);
            LaBandera = await BanderaIServ.GetBandera(BanderaId);
            var campoId = await BanderaIServ.GetBandera(BanderaId);
            ElCampo = await CampoIServ.GetCampo(campoId.Id);
            
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El Usuario consulto las distancias de las banderas {LaBandera.Color} del campo {ElCampo.Corto} {ElCampo.Ciudad} ");
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

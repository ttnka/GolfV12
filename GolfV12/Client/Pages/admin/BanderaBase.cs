using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.admin
{
    public class BanderaBase : ComponentBase 
    {
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        [Inject]
        public IG172BanderaServ BanderaIServ { get; set; }
        [Inject]
        public IG176HoyoServ HoyoIServ { get; set; }
        [Parameter]
        public int CampoId { get; set; }
        public IEnumerable<G172Bandera> LasBanderas { get; set;} = Enumerable.Empty<G172Bandera>();
        public IEnumerable<G176Hoyo> LosHoyos { get; set; } = Enumerable.Empty<G176Hoyo>();
        public Dictionary<int, string> LosCampos { get; set; } = new Dictionary<int, string>();
        
        public string CampoNombre { get; set; } = string.Empty;
        public NavigationManager NM { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            if (CampoId != 0)             
                { LasBanderas = await BanderaIServ.Buscar(CampoId, ""); } 
            else 
                { NM.NavigateTo("/admin/campo"); }

            await LeerCampos();
            
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El Usuario consulto listado de banderas del campo {CampoNombre}");
        }
        public async Task LeerCampos()
        {
            LosCampos.Add(0, "No se encontro el nombre del campo");
            if (CampoId == 0)
            {
                var AllCampos = await CampoIServ.GetCampos();
                foreach (var campo in AllCampos)
                {
                    if (!LosCampos.ContainsKey(CampoId)) { LosCampos.Add(CampoId, campo.Corto + " " + campo.Ciudad); }
                }
            }
            else
            {
                var UnCampo = await CampoIServ.GetCampo(CampoId);
                if (!LosCampos.ContainsKey(CampoId)) { LosCampos.Add(CampoId, UnCampo.Corto + " " +  UnCampo.Ciudad); }
            }
            CampoNombre = LosCampos[CampoId];
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

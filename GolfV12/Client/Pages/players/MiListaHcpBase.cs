using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class MiListaHcpBase : ComponentBase 
    {
        [Parameter]
        public Dictionary<string, string> DicHijo { get; set; } = new Dictionary<string, string>();
        [Inject]
        public IG128HcpServ HcpIServ { get; set; }
        public IEnumerable<G128Hcp> LosHcps { get; set; } = new List<G128Hcp>();
        
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        
        [Parameter]
        public IEnumerable<KeyValuePair<int, string>> LosCampos { get; set; } =
            new List<KeyValuePair<int, string>>();
        protected List<KeyValuePair<int, string>> CamposTemp { get; set; } =
                new List<KeyValuePair<int, string>>();
        public int ElCampo { get; set; } = 0;
        [Inject]
        public IG172BanderaServ BanderaIServ { get; set; }
        public IEnumerable<KeyValuePair<int, string>> LasBanderas { get; set; } =
            new List<KeyValuePair<int, string>>();
        protected List<KeyValuePair<int, string>> BanderaTemp { get; set; } =
                new List<KeyValuePair<int, string>>();

        public RadzenDataGrid<G128Hcp> HcpGrid { get; set; } = new();
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();

        public G128Hcp ElHcp { get; set; } = new G128Hcp();

        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
        
            LeerDatosIniciales();
            await LeerHcp();
            await LeerBanderas();
        }

        protected void LeerDatosIniciales()
        {
            ElHcp.PlayerId = UserIdLog;
            ElHcp.Fecha = DateTime.Now.Date;
            ElCampo = 1;
            ElHcp.BanderaId = 0;
            ElHcp.Hcp = 0;
            ElHcp.Estado = 1;
            ElHcp.Status = true;
        }
        
        protected async Task LeerHcp()
        {
            LosHcps = await HcpIServ.Buscar(UserIdLog);
            int i =1;
            foreach (var hcpT in LosHcps)
            {
                if(!DatosDic.ContainsKey($"Renglon_{hcpT.Id}"))
                {
                    DatosDic.Add($"Renglon_{hcpT.Id}", i.ToString() );
                    i++;
                }    
            }
        }
        protected async Task LeerBanderas()
        {
            var Banderas = await BanderaIServ.GetBanderas();
            foreach (var band in Banderas)
            {
                if (!DatosDic.ContainsKey($"Campo_{band.CampoId}_Bandera_{band.Id}"))
                {
                    DatosDic.Add($"Campo_{band.CampoId}_BanderaId_{band.Id}", band.Id.ToString());
                    DatosDic.Add($"Campo_{band.CampoId}_BanderaId_{band.Id}_Color", band.Color);
                    DatosDic.Add($"BanderaIdCampo_{band.Id}", band.CampoId.ToString());

                }
                if (!DatosDic.ContainsKey($"CampoBanderas_{band.CampoId}"))
                {
                    DatosDic.Add($"CampoBanderas_{band.CampoId}", band.Id.ToString());
                }
                else
                {
                    var Elvalor = DatosDic[$"CampoBanderas_{band.CampoId}"];
                    DatosDic[$"CampoBanderas_{band.CampoId}"] = $"{Elvalor.ToString()},{band.Id.ToString()}";
                }
            }
            FiltrarBanderas();
        }
        protected void FiltrarBanderas()
        {

            //LasBanderas
            BanderaTemp.Clear();

            if (DatosDic.ContainsKey($"CampoBanderas_{ElCampo}"))
            {
                var BandId = DatosDic[$"CampoBanderas_{ElCampo}"].Split(",");
                foreach (var Bid in BandId)
                {
                    BanderaTemp.Add(new KeyValuePair<int, string>(int.Parse(Bid),
                        DatosDic[$"Campo_{ElCampo}_BanderaId_{Bid}_Color"]));
                }
            }
            else
            {
                BanderaTemp.Add(new KeyValuePair<int, string>(0, "No hay banderas registradas"));
            }
            LasBanderas = BanderaTemp.AsEnumerable();
        }
        
        public async Task SaveHcp()
        {
            G128Hcp resultado = new G128Hcp();

            resultado = await HcpIServ.AddHcp(ElHcp);
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                $"El usuario agrego un nuevo registro Hcp {resultado.Id} {resultado.PlayerId}");

            //if (resultado != null) NM.NavigateTo($"/admin/hcp/{PlayerId}");
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

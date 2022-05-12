using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.players
{
    public class MiHcpBase : ComponentBase
    {
        [Inject]
        public IG128HcpServ HcpIServ { get; set; }
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
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

        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string,string>();

        [Inject]
        public NavigationManager NM { get; set; }
        public G128Hcp ElHcp { get; set; } = new G128Hcp();
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            //if (string.IsNullOrEmpty(UserIdLog)) NM.NavigateTo("/");

            LeerDatosIniciales();
            await LeerCampos();
            await LeerBanderas();

        }
        protected void LeerDatosIniciales()
        {
            ElHcp.PlayerId = UserIdLog;
            ElHcp.Fecha = DateTime.Now;
            ElCampo = 1;
            ElHcp.BanderaId = 0;
            ElHcp.Hcp = 0;
            ElHcp.Estado = 1;
            ElHcp.Status = true;

        }
        protected async Task LeerBanderas()
        {
            var Banderas = await BanderaIServ.GetBanderas();
            foreach(var band in Banderas)
            {
                if (!DatosDic.ContainsKey($"Campo_{band.CampoId}_Bandera_{band.Id}"))
                {
                    DatosDic.Add($"Campo_{band.CampoId}_BanderaId_{band.Id}", band.Id.ToString());
                    DatosDic.Add($"Campo_{band.CampoId}_Bandera_{band.Id}_Color", band.Color);
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
                        DatosDic[$"Campo_{ElCampo}_BanderaId_{Bid}"]));
                }
            } 
            else
            {
                BanderaTemp.Add(new KeyValuePair<int, string>(0, "No hay banderas registradas"));
            }
            LasBanderas = BanderaTemp.AsEnumerable();
        }
        protected async Task LeerCampos()
        {
            var Campos = await CampoIServ.GetCampos();
            foreach (var item in Campos)
            {
                if (!DatosDic.ContainsKey($"Campo_{item.Id}"))
                {
                    DatosDic.Add($"Campo_{item.Id}", $"{item.Corto} {item.Ciudad}");
                    CamposTemp.Add(new KeyValuePair<int, string>(item.Id, $"{item.Corto} {item.Ciudad}"));
                }
            }
            LosCampos = CamposTemp.AsEnumerable();
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

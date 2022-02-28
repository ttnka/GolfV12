using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.torneo
{
    public class CategoriaBase : ComponentBase
    {
        [Parameter]
        public int TorneoId { get; set; }

        [Inject]
        public IG208CategoriaTServ CatIServ { get; set; }
        public IEnumerable<G208CategoriaT> LasCategorias { get; set; } = Enumerable.Empty<G208CategoriaT>();
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public string ElTorneo { get; set; } = string.Empty;
        [Inject]
        public IG172BanderaServ BanderaIServ { get; set; }
        public IEnumerable<G172Bandera> LasBanderas { get; set; } = Enumerable.Empty<G172Bandera>();
        public Dictionary<int, string> LasBanderasDic { get; set; } = new Dictionary<int, string>();
        [Inject]
        public NavigationManager NM { get; set; }
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerDatos();
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El Usuario consulto las categorias de un torneo {ElTorneo}");
        }
        protected async Task LeerDatos()
        {
            var t = await TorneoIServ.GetTorneo(TorneoId);
            ElTorneo = $"{t.Titulo} ({t.Id})";
           
            LasCategorias = await CatIServ.Buscar(TorneoId, "");
            LasBanderas = await BanderaIServ.Buscar(t.Campo, "");
            if (LasBanderas.Any())
            {
                foreach (var l in LasBanderas)
                {
                    if (!LasBanderasDic.ContainsKey(l.Id)) LasBanderasDic.Add(l.Id, $"{l.Color}" );
                }
                LasBanderasDic.Add(0, "No hay informacion del color de la bandera!");
            }
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

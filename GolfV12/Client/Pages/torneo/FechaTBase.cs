using Microsoft.AspNetCore.Components;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using GolfV12.Client.Servicios.IFaceServ;

namespace GolfV12.Client.Pages.torneo
{
    public class FechaTBase : ComponentBase 
    {
        [Parameter]
        public int TorneoId { get; set; }
        [Inject]
        public IG204FechaTServ FechaTIServ { get; set; }  
        public IEnumerable<G204FechaT> LasFechas { get; set; } 
        public Dictionary<string, int> FechaDic { get; set; } = new Dictionary<string, int>();
        public string ElTorneo { get; set; } = string.Empty;
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            LasFechas = await FechaTIServ.Buscar(TorneoId, DateTime.MinValue);
            await LeerDatos();
        }

        protected async Task LeerDatos() 
        {
            var t = await TorneoIServ.GetTorneo(TorneoId);
            ElTorneo = $"{t.Titulo} ({t.Id})";

            if (LasFechas != null)
            {
                int rondaMax = 0;
                foreach (var l in LasFechas)
                {
                    if (rondaMax < l.Ronda) rondaMax = l.Ronda;
                }
                FechaDic.Add("RondaMaxFecha", rondaMax);
                FechaDic.Add("RondasTorneo", t.Rondas);
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

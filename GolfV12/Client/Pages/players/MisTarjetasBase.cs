using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class MisTarjetasBase : ComponentBase 
    {
        [Parameter]
        public int ElEstado { get; set; } = 0;
        [Parameter]
        public Dictionary<string, string> DicHijo { get; set; } = new Dictionary<string, string>();
        
        [Parameter]
        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } =
                    new List<KeyValuePair<string, string>>();
        [Parameter]
        public IEnumerable<KeyValuePair<int, string>> LosCampos { get; set; } = 
                        new List<KeyValuePair<int, string>>();
        [Parameter]
        public EventCallback<G500Tarjeta> OnInsertJugador { get; set; }
        [Inject]
        public IG500TarjetaServ TarjetaIServ { get; set; }
        public G500Tarjeta LaTarjeta { get; set; } = new();
        public IEnumerable<G500Tarjeta> LasTarjetas { get; set; } 
        [Inject]
        public IG510JugadorServ JugadorIServ { get; set; }

        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<G500Tarjeta> TarjetaGrid { get; set; } = new();

        public IEnumerable<Torneo2Edit> LasCapturas { get; set; } = Enum.GetValues(typeof(Torneo2Edit)).Cast<Torneo2Edit>().ToList();
        public IEnumerable<TorneoView> LasConsultas { get; set; } = Enum.GetValues(typeof(TorneoView)).Cast<TorneoView>().ToList();

        

        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
 
            await LeerTarjetas();
            
           
        }

        protected async Task LeerTarjetas()
        {
            string LaClave = $"tar4creador_-_creador_-_{UserIdLog}_-_estado_-_3";
            if (ElEstado == 3) LaClave = $"tar3creador_-_creador_-_{UserIdLog}_-_estado_-_3";
                
            var TarjTemp = await TarjetaIServ.Filtro(LaClave);
            

            LasTarjetas = TarjTemp.AsEnumerable();
        }
        
        public async Task SaveTarjeta()
        {
            G500Tarjeta resultado = new G500Tarjeta();

            resultado = await TarjetaIServ.AddTarjeta(LaTarjeta);
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                $"El usuario agrego un nuevo tarjeta de juego {resultado.Id} {resultado.Titulo} ");

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

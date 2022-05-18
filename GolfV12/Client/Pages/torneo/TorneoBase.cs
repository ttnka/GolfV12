using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.torneo
{
    public class TorneoBase : ComponentBase 
    {
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public IEnumerable<G200Torneo> LosTorneos { get; set; }  = Enumerable.Empty<G200Torneo>();
        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }       
        public Dictionary<string, string> AllPlayers { get; set; } = new Dictionary<string, string>();
        [Inject]
        public IG280FormatoTServ FormatoIServ { get; set; }
        public Dictionary<int, string> LosFormatos { get; set; } = new Dictionary<int, string>();
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        public Dictionary<int, string> LosCampos { get; set; } = new Dictionary<int, string>();
        [Inject]
        public IG204FechaTServ FechaIServ { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            LosTorneos = await TorneoIServ.GetTorneos();
            await LeerPlayers();
            await LeerCampos();
            await LeerFormatos();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario Consulto el listado de Torneos");
        }

        protected async Task LeerPlayers()
        {
            var Players = await PlayerIServ.Filtro("All");
            foreach (var player in Players)
            {
                if (!AllPlayers.ContainsKey(player.UserId)) AllPlayers.Add(player.UserId,
                    $"{player.Nombre} {player.Apodo} {player.Paterno}");
            }
            AllPlayers.Add("Vacio", "No se encontro Jugador!");
        }
        protected async Task LeerFormatos()
        {
            var Formatos = await FormatoIServ.GetFormatos();
            foreach (var formato in Formatos)
            {
                if (!LosFormatos.ContainsKey(formato.Id)) LosFormatos.Add(formato.Id,
                    $"{formato.Clave} {formato.Titulo}");
            }
            LosFormatos.Add(0, "No hay informacion del Formato!");
        }
        protected async Task LeerCampos() 
        {
            var Campos = await CampoIServ.GetCampos();
            foreach (var campo in Campos)
            {
                if (!LosCampos.ContainsKey(campo.Id)) LosCampos.Add(campo.Id,
                    $"{campo.Corto} {campo.Ciudad}");
            }
            LosCampos.Add(0, "No se encontro informacion del campo!");
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

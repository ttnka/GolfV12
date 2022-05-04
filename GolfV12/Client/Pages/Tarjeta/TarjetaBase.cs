using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.Tarjeta
{
    public class TarjetaBase : ComponentBase 
    {
        [Parameter]
        public string PlayerId { get; set; }
        [Inject]
        public IG120PlayerServ PlayersServ { get; set; }
        public IEnumerable<G120Player> AllPlayers { get; set; } = new List<G120Player>();

        [Inject]
        public IG500TarjetaServ TarjetaServ { get; set; } 
        public IEnumerable<G500Tarjeta> LasTarjetas { get; set; } = new List<G500Tarjeta>();

        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        public Dictionary<int, string> LosCampos { get; set; } = new Dictionary<int, string>();
        [Inject]
        public IG510JugadorServ ParticipantesIServ { get; set; }
        public IEnumerable<G510Jugador> LosParticipantes { get; set; } = new List<G510Jugador>();
        public Dictionary<string, string> LosNombres { get; set; } = new Dictionary<string, string>();

        [Inject]
        public NavigationManager NM { get; set; }
        public void InsertTarjeta()
        {
            NM.NavigateTo("/tarjeta/tarjetaedit/");
        }
        protected async override Task OnInitializedAsync()
        {
            //if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            

            await LeerDatos();
            await LeerCampos();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario consulto el listado de tarjetas");
        }

        protected async Task LeerDatos()
        {
            string claveP = "clave=";
            if (!string.IsNullOrEmpty(PlayerId))
            {
                claveP = $"tar1_-_creador_-_{PlayerId}";
            }
            AllPlayers = await PlayersServ.GetPlayers();
            if (AllPlayers != null)
            {
                foreach (var player in AllPlayers)
                {
                    if (!LosNombres.ContainsKey(player.UserId))
                        LosNombres.Add(player.UserId, $"{player.Nombre} {player.Apodo} {player.Paterno}");
                }
            }

            LasTarjetas = await TarjetaServ.Filtro(claveP);

            LosParticipantes = await ParticipantesIServ.Filtro("");
            
            if (LosParticipantes.Count() > 0)
            {
                foreach (var tarj in LasTarjetas)
                {
                    if (!LosNombres.ContainsKey($"Jugadores_{tarj.Id}"))
                    {
                        LosNombres.Add($"Jugadores_{tarj.Id}", LosParticipantes.Count(e => e.Tarjeta.Contains(tarj.Id)).ToString());
                    }
                    foreach (var participante in LosParticipantes.Where(e => e.Tarjeta.Contains(tarj.Id)))
                    {

                        if(!LosNombres.ContainsKey($"Nombres_{tarj.Id}")) 
                        {
                            LosNombres.Add($"Nombres_{tarj.Id}", LosNombres[participante.Player]);
                        }
                        else
                        {
                            LosNombres[$"Nombres_{tarj.Id}"] = $"{LosNombres[$"Nombres_{tarj.Id}"]} {LosNombres[participante.Player]}" ;
                        }
                    }   
                }

            }
            
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

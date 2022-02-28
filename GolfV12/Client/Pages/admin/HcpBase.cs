using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.admin
{
    public class HcpBase : ComponentBase 
    {
        [Inject]
        public IG128HcpServ HcpIServ { get; set; } 
        [Inject]
        public IG120PlayerServ PlayerServ { get; set; }

        public IEnumerable<G128Hcp> LosHcps { get; set; } = Enumerable.Empty<G128Hcp>();
        public Dictionary<string, string> LosPlayers { get; set; } = new Dictionary<string, string>();

        [Parameter]
        public string PlayerId { get; set; } 

        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerLosPlayers();
            var textoTemp1 = string.Empty;

            if (string.IsNullOrEmpty(PlayerId)) 
            { 
                LosHcps = await HcpIServ.GetHcps();
                textoTemp1 = "Consulto listado de Hcp de los jugadores";
            }
            else
            {
                //LosHcps = await HcpIServ.GetHcps();
                LosHcps = await HcpIServ.Buscar(PlayerId);

                textoTemp1 = $"Consulto el Hcp de {LosPlayers[PlayerId]}";
            }
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                    textoTemp1);
        }

        public async Task LeerLosPlayers()
        {
            LosPlayers.Add("Vacio", "No se encontro nombre de registro");
            if (string.IsNullOrEmpty(PlayerId)) 
            { 
                var Allplayers = await PlayerServ.GetPlayers();
                foreach (var player in Allplayers)
                {
                    if (!LosPlayers.ContainsKey(player.UserId)) LosPlayers.Add(player.UserId, 
                        player.Nombre + player.Paterno + player.Materno);
                }  
            } 
            else
            { 
                var Oneplayer = await PlayerServ.GetPlayer(PlayerId); 
                var textTemp = string.Empty;
                if(Oneplayer != null) 
                    { 
                        textTemp = Oneplayer.Nombre + " " + Oneplayer.Paterno + " " + Oneplayer.Materno;
                    } 
                else
                    {
                        textTemp = "No se encontro nombre";
                    }
                LosPlayers.Add(PlayerId, textTemp);
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

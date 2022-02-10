using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GolfV12.Client.Pages.admin
{
    public class HcpBase : ComponentBase 
    {
        [Inject]
        public IG128HcpServ hcpIServ { get; set; }
        [Inject]
        public IG120PlayerServ playerServ { get; set; }

        public IEnumerable<G128Hcp> LosHcps { get; set; }
        public Dictionary<string, string> LosPlayers { get; set; } = new Dictionary<string, string>();

        [Parameter]
        public string playerid { get; set; } 

        protected async override Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await EscribirBitacoraUno(userIdLog, BitaAcciones.Borrar, false, $"elplayerid es{playerid} ");

            await LeerLosPlayers();
            var textoTemp1 = string.Empty;

            if (string.IsNullOrEmpty(playerid)) 
            { 
                LosHcps = await hcpIServ.GetHcps();
                textoTemp1 = "Consulto listado de Hcp de los jugadores";
            }
            else
            {
                LosHcps = await hcpIServ.GetHcps();
                //LosHcps = await hcpIServ.Buscar(playerid);

                textoTemp1 = $"Consulto el Hcp de {LosPlayers[playerid]}";
            }
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false,
                    textoTemp1);
        }

        public async Task LeerLosPlayers()
        {
            if (string.IsNullOrEmpty(playerid)) 
            { 
                var Allplayers = await playerServ.GetPlayers();
                foreach (var player in Allplayers)
                {
                    if (!LosPlayers.ContainsKey(player.UserId)) LosPlayers.Add(player.UserId, player.Nombre + player.Paterno + player.Materno);
                }
            } 
            else
            { 
                var Oneplayer = await playerServ.GetPlayer(playerid); 
                var textTemp = string.Empty;
                if(Oneplayer != null) 
                    { 
                        textTemp = Oneplayer.Nombre + Oneplayer.Paterno + Oneplayer.Materno;
                    } 
                else
                    {
                textTemp = "No se encontro nombre";
                    }
                LosPlayers.Add(playerid, textTemp);
            }
            
        }

        [CascadingParameter]
        public Task<AuthenticationState> authStateTask { get; set; }
        public string userIdLog { get; set; }
        [Inject]
        public IG190BitacoraServ bitacoraServ { get; set; }
        private G190Bitacora writeBitacora { get; set; } = new G190Bitacora();
        public async Task EscribirBitacoraUno(string userId, BitaAcciones accion, bool Sistema, string desc)
        {
            writeBitacora.Fecha = DateTime.Now;
            writeBitacora.Accion = accion;
            writeBitacora.Sistema = Sistema;
            writeBitacora.UsuarioId = userId;
            writeBitacora.Desc = desc;
            await bitacoraServ.AddBitacora(writeBitacora);
        }
    }
}

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.admin
{
    public class HcpEditBase : ComponentBase
    {
        [Parameter]
        public string playerid { get; set; }
        [Parameter]
        public int hcpid { get; set; } = 0;
        [Inject]
        public IG128HcpServ hcpIServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        public G128Hcp ElHcp { get; set; } = new G128Hcp();
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            
            if (string.IsNullOrEmpty(playerid)) NM.NavigateTo("/admin/player");

            if (hcpid > 0)
            {
                await hcpIServ.GetHcp(hcpid);
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false,
                "El usuario intento modificar el Hcp");
            } else
            {
                ElHcp.PlayerId = playerid;
                ElHcp.Fecha = DateTime.Now;
                ElHcp.BanderaId = 0;
                ElHcp.Hcp = 0;
                ElHcp.Estado = 1;
                ElHcp.Status = true;
                ButtonTexto = "Agregar";
            }
        }
        public async Task saveHcp()
        {
            G128Hcp resultado = new G128Hcp();
            if (hcpid == 0)
            {
                resultado = await hcpIServ.AddHcp(ElHcp);
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo registro Hcp {resultado.Id} {resultado.PlayerId}");
                elMesage.Summary = "Registro AGREGADO!";
                elMesage.Detail = "Exitosamente";

            }
            else
            {
               resultado = await hcpIServ.UpdateHcp(ElHcp);
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo el Hcp del registro{resultado.Id} {resultado.PlayerId}");
                elMesage.Summary = "Registro ACTUALIZADO!";
                elMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo("/admin/player/");
        }

        public NotificationMessage elMesage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Cuerpo",
            Detail = "Detalles ",
            Duration = 3000
        };

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

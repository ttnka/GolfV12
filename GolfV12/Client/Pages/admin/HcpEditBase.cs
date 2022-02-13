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
        public string PlayerId { get; set; }
        [Parameter]
        public int HcpId { get; set; }
        [Inject]
        public IG128HcpServ HcpIServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        public G128Hcp ElHcp { get; set; } = new G128Hcp();
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            
            if (string.IsNullOrEmpty(PlayerId)) NM.NavigateTo("/admin/player");

            if (HcpId > 0)
            {
                ElHcp = await HcpIServ.GetHcp(HcpId);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El usuario intento modificar el Hcp");
            } else
            {
                ElHcp.PlayerId = PlayerId;
                ElHcp.Fecha = DateTime.Now;
                ElHcp.BanderaId = 0;
                ElHcp.Hcp = 0;
                ElHcp.Estado = 1;
                ElHcp.Status = true;
                ButtonTexto = "Agregar";
            }
        }
        public async Task SaveHcp()
        {
            G128Hcp resultado = new G128Hcp();
            if (HcpId == 0)
            {
                resultado = await HcpIServ.AddHcp(ElHcp);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo registro Hcp {resultado.Id} {resultado.PlayerId}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";

            }
            else
            {
               resultado = await HcpIServ.UpdateHcp(ElHcp);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo el Hcp del registro{resultado.Id} {resultado.PlayerId}");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/admin/hcp/{PlayerId}");
        }

        public NotificationMessage ElMesage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Cuerpo",
            Detail = "Detalles ",
            Duration = 3000
        };

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

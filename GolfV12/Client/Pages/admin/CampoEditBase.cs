using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.admin
{
    public class CampoEditBase : ComponentBase 
    {
        [Parameter]
        public int CampoId { get; set; }
        public G170Campo ElCampo { get; set; } = new G170Campo();

        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";

        protected async override Task OnInitializedAsync()
        {
            if (CampoId > 0)
            {
                ElCampo = await CampoIServ.GetCampo(CampoId);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El usuario intento modificar el Hcp");
            } else
            {
                ElCampo.Corto = "Corto";
                ElCampo.Nombre = "Nombre";
                ElCampo.Desc = "Des";
                ElCampo.Ciudad = "Ciudad";
                ElCampo.Pais = "Pais";
                ButtonTexto = "Agregar";
            }
        }

        public async Task SaveCampo()
        {
            G170Campo resultado = null;
            if (CampoId == 0)
            {
                resultado = await CampoIServ.AddCampo(ElCampo);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo campo {resultado.Id} {resultado.Corto}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";

            }
            else
            {
                resultado = await CampoIServ.UpdateCampo(ElCampo);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo la info del Campo {resultado.Id} {resultado.Corto}");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/admin/campo/");
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

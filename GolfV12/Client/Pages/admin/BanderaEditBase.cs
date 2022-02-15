using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.admin
{
    public class BanderaEditBase : ComponentBase
    {
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        [Inject]
        public IG172BanderaServ BanderaIServ { get; set;}
        [Parameter]
        public int CampoId { get; set; } 
        [Parameter]
        public int BanderaId { get; set; } 
        public G170Campo ElCampo { get; set; } = new G170Campo();
        public G172Bandera LaBandera { get; set; } = new G172Bandera();
        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;


            if (CampoId == 0) { NM.NavigateTo("/admin/campo"); } 
            else { ElCampo = await CampoIServ.GetCampo(CampoId); } 
               
            if (BanderaId == 0) { LaBandera.Color = "Color de la bandera"; } 
            else { LaBandera = await BanderaIServ.GetBandera(BanderaId); }
                
            LaBandera.CampoId = CampoId;
        }
        public async Task SaveBandera()
        {
            G172Bandera resultado = new G172Bandera();
            if (BanderaId == 0)
            {
                resultado = await BanderaIServ.AddBandera(LaBandera);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo registro Banderas {resultado.Id} {ElCampo.Corto} {resultado.Color}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";

            }
            else
            {
                resultado = await BanderaIServ.UpdateBandera(LaBandera);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo la bandera {resultado.Id} del campo " +
                    $"{ElCampo.Corto} {resultado.Color}");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/admin/bandera/{CampoId}");
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

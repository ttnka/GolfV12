using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.admin
{
    public class HoyoEditBase : ComponentBase
    {
        [Parameter]
        public int CampoId { get; set; }
        [Parameter]
        public int HoyoId { get; set; }
        [Inject]
        public IG176HoyoServ HoyoIServ { get; set; }
        public int HoyoNext { get; set; } = 1;
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        public G170Campo ElCampo { get; set; } = new G170Campo();
        public G176Hoyo ElHoyo { get; set; } = new G176Hoyo();
        public string ButtonTexto { get; set; } = "Actualizar";

        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            if (CampoId == 0) NM.NavigateTo("/admin/campo");
            await LeerDatos();
        }

        public async Task LeerDatos()
        {
            if (HoyoId == 0)
            {
                ElHoyo.CampoId = CampoId;
                ElHoyo.Ruta = "Unica";
                await BuscarNextHoyo(ElHoyo.Ruta);
            }
            else
            {
                ElHoyo = (await HoyoIServ.Filtro($"hoy1id_-_id_-_{HoyoId}")).FirstOrDefault() ;
            }
            ElCampo = await CampoIServ.GetCampo(CampoId);
        }
        public async Task BuscarNextHoyo(string ruta)
        {
            var LosHoyos = await HoyoIServ.Filtro($"hoy1campo_-_campo_-_{CampoId}");
            foreach (var l in LosHoyos)
            {
                if (HoyoNext <= l.Hoyo) HoyoNext = l.Hoyo + 1;
            }
            ElHoyo.Hoyo = HoyoNext;
        }

        public async Task SaveHoyo()
        {
            G176Hoyo resultado = new G176Hoyo();
            if (HoyoId == 0)
            {
                resultado = await HoyoIServ.AddHoyo(ElHoyo);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo registro {resultado.Id} Hoyos del campo {resultado.CampoId}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";
            }
            else
            {
                resultado = await HoyoIServ.UpdateHoyo(ElHoyo);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo registro{resultado.Id} del hoyo {resultado.Hoyo} del campo {resultado.CampoId}");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/admin/hoyo/{CampoId}");
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

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;


namespace GolfV12.Client.Pages.players
{
    public class MisdatosBase : ComponentBase
    {
        [Inject]
        protected IG121ElPlayerServ elPlayerServ { get; set; }
        [Inject]
        protected IG120PlayerServ playerServ { get; set; }
        public NavigationManager NM { get; set; }
        protected G120Player Midata { get; set; }
        //protected WBita WB { get; set; } = new WBita();
        protected override async Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            Midata = await elPlayerServ.GetPlayer(userIdLog);
            await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false,
                "Consulto sus datos");
        }

        public async Task MisDatosUpdate()
        {
            var resultado = await playerServ.UpdatePlayer(Midata);
            if (resultado != null)
            {
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Editar, false,
                    $"Actualizo sus datos Nombre {Midata.Nombre} Apellido {Midata.Paterno} {Midata.Materno} " +
                    $"Apodo {Midata.Apodo} {Midata.Estado}");
                elMesage.Summary = "Registro Actualizado ";
                elMesage.Detail = "Exitosamente!!!";
            }
        }

        // Mensaje de Actualizacion
        public NotificationMessage elMesage { get; set; } = new NotificationMessage() { 
                Severity = NotificationSeverity.Success, Summary = "Cuerpo", Detail = "Detalles ", Duration = 3000 };
    
        [CascadingParameter]
        public Task<AuthenticationState> authStateTask { get; set; }
        public string userIdLog { get; set; }
        // Bitacora

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

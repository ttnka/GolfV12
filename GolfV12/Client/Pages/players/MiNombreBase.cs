using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.players
{
    public class MiNombreBase : ComponentBase 
    {
        [Inject]
        protected IG120PlayerServ PlayerIServ { get; set; }
        protected G120Player Midata { get; set; } = new G120Player();
        public bool EditarMisDatos { get; set; } = false;
        [Inject]
        public NavigationManager NM { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;


            Midata = (await PlayerIServ.Filtro($"play1id_-_userid_-_{UserIdLog}")).FirstOrDefault();
            if (Midata == null || Midata.UserId != UserIdLog) NM.NavigateTo("/"); 

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario Consulto sus datos");
        }
        public async Task MisDatosUpdate()
        {
            var resultado = await PlayerIServ.UpdatePlayer(Midata);
            if (resultado != null)
            {
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"Actualizo sus datos Nombre {Midata.Nombre} Apellido {Midata.Paterno} {Midata.Materno} " +
                    $"Apodo {Midata.Apodo} {Midata.Estado}");
                
                elMesage.Summary = "Registro Actualizado ";
                elMesage.Detail = "Exitosamente al 100!!!";
                EditarMisDatos = false;
               
            }
        }



        // Mensaje de Actualizacion
        public NotificationMessage elMesage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Cuerpo",
            Detail = "Detalles ",
            Duration = 3000
        };

        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; } = String.Empty;
        // Bitacora

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

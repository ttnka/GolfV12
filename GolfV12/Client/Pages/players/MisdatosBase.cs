using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;


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
            Midata = await elPlayerServ.GetPlayer("2");
            await EscribirBitacoraUno(2, BitaAcciones.Consultar, false,
                "Consulto sus datos");
        }

        public async Task MisDatosUpdate()
        {
            var resultado = await playerServ.UpdatePlayer(Midata);
            if (resultado != null)
            {
                await EscribirBitacoraUno(2, BitaAcciones.Editar, false,
                    $"Actualizo sus datos Nombre {Midata.Nombre} Apellido {Midata.Paterno} {Midata.Materno} " +
                    $"Apodo {Midata.Apodo} {Midata.Estado}");
                elMesage.Summary = "Registro Actualizado ";
                elMesage.Detail = "Exitosamente!!!";
            }
        }

    // Mensaje de Actualizacion
     public NotificationMessage elMesage { get; set; } = new NotificationMessage() { 
                Severity = NotificationSeverity.Success, Summary = "Cuerpo", Detail = "Detalles ", Duration = 3000 };
    
    // Bitacora

    [Inject]
        public IG190BitacoraServ bitacoraServ { get; set; }
        private G190Bitacora writeBitacora { get; set; } = new G190Bitacora();
        public async Task EscribirBitacoraUno(int usuario, BitaAcciones accion, bool Sistema, string desc)
        {
            writeBitacora.Fecha = DateTime.Now;
            writeBitacora.Accion = accion;
            writeBitacora.Sistema = Sistema;
            writeBitacora.UsuarioId = usuario;
            writeBitacora.Desc = desc;
            await bitacoraServ.AddBitacora(writeBitacora);
        }
    }
}

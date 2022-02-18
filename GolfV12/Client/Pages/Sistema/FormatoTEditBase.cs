using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.Sistema
{
    public class FormatoTEditBase : ComponentBase 
    {
        [Parameter]
        public int FormatoId { get; set; }
        [Inject]
        public IG280FormatoTServ FormatoIServ { get; set; }
        public G280FormatoT ElFormato { get; set; } = new G280FormatoT();
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            if (FormatoId == 0) 
            {
                ElFormato.Clave = "Corto";
                ElFormato.Titulo = "Titulo";
                ElFormato.Individual = true;
                ButtonTexto = "Agregar";
            }
            else 
            {
                ElFormato = await FormatoIServ.GetFormato(FormatoId);
            }
        }
        public async Task SaveFormato()
        {
            G280FormatoT resultado = null;
            if (FormatoId == 0)
            {
                resultado = await FormatoIServ.AddFormato(ElFormato);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo Formato de Torneo {resultado.Titulo} {resultado.Desc}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";
            }
            else
            {
                resultado = await FormatoIServ.UpdateFormato(ElFormato);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo la info de un Formato de Torneo {resultado.Titulo} {resultado.Desc}");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) 
                NM.NavigateTo("/sistema/formato/");
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

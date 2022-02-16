using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.admin
{
    public class DistanciaEditBase : ComponentBase 
    {
        [Parameter]
        public int BanderaId { get; set; }
        [Parameter]
        public int DistanciaId { get; set; }
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        [Inject]
        public IG172BanderaServ BanderaIServ { get; set; }
        [Inject]
        public IG178DistanciaServ DistIServ { get; set; }
        public G170Campo ElCampo { get; set; } = new G170Campo();
        public G172Bandera SoloBandera { get; set; } = new G172Bandera();
        public G178Distancia LaDistancia { get; set; } = new G178Distancia();
        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            
            if (BanderaId == 0) 
                { NM.NavigateTo("/admin/campo/"); } 
            else 
                { 
                    SoloBandera = await BanderaIServ.GetBandera(BanderaId);
                    ElCampo = await CampoIServ.GetCampo(SoloBandera.CampoId);
                }
            if (DistanciaId == 0) 
                { 
                    LaDistancia.BanderaId = BanderaId;
                    LaDistancia.Fecha = DateTime.Now.Date;
                } 
            else 
                { 
                    LaDistancia = await DistIServ.GetDistancia(BanderaId);
                ButtonTexto = "Agregar";
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                    $"El Usuario consulto la distancia del hoyo {LaDistancia.Hoyo} de la mandera {SoloBandera.Color}");
                }
        }
        public async Task SaveDistancia()
        {
            G178Distancia resultado = new G178Distancia();
            if (DistanciaId == 0)
            {
                resultado = await DistIServ.AddDistancia(LaDistancia);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego un nuevo distancia Banderas");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";
            }
            else
            {
                resultado = await DistIServ.UpdateDistancia(LaDistancia);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo la distancia del hoyo  {resultado.Hoyo} del campo ");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/admin/distancia/{BanderaId}");
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

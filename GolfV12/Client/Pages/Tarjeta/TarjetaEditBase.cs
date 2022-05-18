using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.Tarjeta
{
    public class TarjetaEditBase : ComponentBase 
    {
        [Parameter]
        public string TarjetaId { get; set; }
        [Inject]
        public IG500TarjetaServ TarjServicio { get; set; }
        public G500Tarjeta LaTarjeta { get; set; } = new G500Tarjeta();
        [Inject]
        public IG170CampoServ CampoIServ { get; set; }
        public IEnumerable<G170Campo> LosCampos { get; set; } = new List<G170Campo>();

        public IEnumerable<TorneoView> TarjetaViews { get; set; } = Enum.GetValues(typeof(TorneoView)).Cast<TorneoView>().ToList();
        public IEnumerable<Torneo2Edit> TarjetaEdits { get; set; } = Enum.GetValues(typeof(Torneo2Edit)).Cast<Torneo2Edit>().ToList();
        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            ElUser = (await PlayerIServ.Filtro($"play1id_-_{UserIdLog}")).FirstOrDefault();

            LosCampos = await CampoIServ.GetCampos();

            if (string.IsNullOrEmpty(TarjetaId))
            {
                LaTarjeta.Id = Guid.NewGuid().ToString();
                LaTarjeta.Creador = UserIdLog;
                LaTarjeta.Fecha = DateTime.Today;
                LaTarjeta.Titulo = $"Nuevo juego de {DateTime.Today.Day}";
                LaTarjeta.Campo = 1;
                LaTarjeta.Captura = Torneo2Edit.Creador;
                LaTarjeta.Consulta = TorneoView.Todos;
                ButtonTexto = "Agregar nuevo ";
            }
            else
            {
                var latar = await TarjServicio.Filtro($"tar1_-_id_-_{TarjetaId}");
                LaTarjeta = latar.FirstOrDefault() ?? new G500Tarjeta();
            }
        }
        public async Task SaveTarjeta()
        {
            G500Tarjeta resultado = null;
            if (string.IsNullOrEmpty(LaTarjeta.Titulo)) LaTarjeta.Titulo = " ";
            if (LaTarjeta.Fecha.Date < DateTime.Now.Date) LaTarjeta.Fecha = DateTime.Now.Date;
            if (LaTarjeta.Fecha.Date > DateTime.Now.AddDays(370)) LaTarjeta.Fecha = DateTime.Now.Date;
            if (string.IsNullOrEmpty(TarjetaId))
            {
                resultado = await TarjServicio.AddTarjeta(LaTarjeta);
                
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego una nueva tarjeta de juego {resultado.Titulo} {resultado.Fecha}");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";
            }
            else
            {
                resultado = await TarjServicio.UpdateTarjeta(LaTarjeta);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo la info de una tarjeta de juego {resultado.Titulo} {resultado.Fecha}");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }

            if (resultado != null) NM.NavigateTo("/tarjeta/tarjeta");
        }

        public NotificationMessage ElMesage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Cuerpo",
            Detail = "Detalles ",
            Duration = 3000
        };

        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }
        public G120Player ElUser { get; set; } = new G120Player();

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

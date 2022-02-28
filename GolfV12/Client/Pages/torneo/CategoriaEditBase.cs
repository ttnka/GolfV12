using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace GolfV12.Client.Pages.torneo
{
    public class CategoriaEditBase : ComponentBase 
    {
        [Parameter]
        public int TorneoId { get; set; }
        [Parameter]
        public int CategoriaId { get; set; }
        [Inject]
        public IG200TorneoServ TorneoIServ { get; set; }
        public G200Torneo ElTorneo { get; set; } = new G200Torneo();
        [Inject]
        public IG208CategoriaTServ CatIServ { get; set; }
        public G208CategoriaT LaCategoria { get; set; } = new G208CategoriaT();
        [Inject]
        public IG172BanderaServ BanderaIServ { get; set; } 
        public IEnumerable<G172Bandera> LasBanderas { get; set; } = Enumerable.Empty<G172Bandera>();

        [Inject]
        public NavigationManager NM { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        protected async override Task OnInitializedAsync()
        {
            if (TorneoId == 0) NM.NavigateTo("/torneo/torneo/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;
            
            ElTorneo = await TorneoIServ.GetTorneo(TorneoId);
            LasBanderas = await BanderaIServ.Buscar(ElTorneo.Campo, "");

            
            if (CategoriaId == 0)
            {
                LaCategoria.Torneo = TorneoId;
                LaCategoria.Titulo = "Nueva Cat";
                LaCategoria.Desc = "";
                LaCategoria.Bandera = 0;
                LaCategoria.NumJugadores = 2;

                ButtonTexto = "Agregar"; 
            }
            else
            {
                LaCategoria = await CatIServ.GetCategoria(CategoriaId);
            }
        }
        protected async Task LeerDatos()
        {

        }

        public async Task SaveCat()
        {
            G208CategoriaT resultado = new G208CategoriaT();
            if (string.IsNullOrEmpty(LaCategoria.Desc)) LaCategoria.Desc = " ";
            if (CategoriaId == 0)
            {
                resultado = await CatIServ.AddCategoria(LaCategoria);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"El usuario agrego una nueva categoria {resultado.Titulo} al torneo  {ElTorneo.Titulo} ");
                ElMesage.Summary = "Registro AGREGADO!";
                ElMesage.Detail = "Exitosamente";
            }
            else
            {
                resultado = await CatIServ.UpdateCategoria(LaCategoria);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actualizo la categoria {resultado.Titulo} del torneo {ElTorneo.Titulo} ");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }
            if (resultado != null) NM.NavigateTo($"/torneo/categoria/{TorneoId}");
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

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;


namespace GolfV12.Client.Pages.Sistema
{
    public class EstadoEditBase : ComponentBase
    {
       
        [Inject]
        public NavigationManager NM { get; set; }
        [Inject]
        public IG180EstadoServ iG180EstadoServ { get; set; }
        [Parameter]
        public int Id { get; set; } 
        public G180Estado ElEstado { get; set; }
        public IEnumerable<G180Estado> LosEstados { get; set;}
        public int UnEstado { get; set; }
        private G120Player elUsuario { get; set; } = new G120Player();
        public string ButtonTexto { get; set; } = "Actualizar";
        //protected WBita WB { get; set; } = new WBita();
        protected async override Task OnInitializedAsync()
        {
            var autState = await authStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) userIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            if (Id == 0) 
            {
                ButtonTexto = "Agregar";
                ElEstado = new G180Estado
                {
                    Indice = 1,
                    Titulo = "Nuevo",
                    Grupo = "Nuevo",
                    Estado = 2,
                    Status = true
                };
            } else 
            {
                ElEstado = await iG180EstadoServ.GetEstado(Id);
            }
            LosEstados = await iG180EstadoServ.Buscar("Vacio", "Organizacion");
            await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false,
                "Consulto listado de Estados de registros");
        }
        public async Task OnSubmit(G180Estado elEdo)
        {
            G180Estado res = null;
            if (elEdo.Id != 0)
            {
                res = await iG180EstadoServ.UpdateEstado(elEdo);
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Agregar, false,
                    $"Agrego registro {elEdo.Titulo} del grupo {elEdo.Grupo}");
            }
            else
            {
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Consultar, false,
                    $"Consulto registro {elEdo.Id} de {elEdo.Titulo} del grupo {elEdo.Grupo}");
                res = await iG180EstadoServ.AddEstado(elEdo);
                await EscribirBitacoraUno(userIdLog, BitaAcciones.Editar, false,
                    $"Actualizo registro {elEdo.Id} de {elEdo.Titulo} del grupo {elEdo.Grupo}");
            }

            if (res != null) NM.NavigateTo("/sistema/estado");
        }
        [CascadingParameter]
        public Task<AuthenticationState> authStateTask { get; set; }
        public string userIdLog { get; set; }

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

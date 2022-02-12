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
        public IG180EstadoServ EstadoIServ { get; set; }
        [Parameter]
        public int Id { get; set; } 
        public G180Estado ElEstado { get; set; }
        public IEnumerable<G180Estado> LosEstados { get; set;}
        public int UnEstado { get; set; }
        private G120Player ElUsuario { get; set; } = new G120Player();
        public string ButtonTexto { get; set; } = "Actualizar";
        //protected WBita WB { get; set; } = new WBita();
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

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
                ElEstado = await EstadoIServ.GetEstado(Id);
            }
            LosEstados = await EstadoIServ.Buscar("Vacio", "Organizacion");
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "Consulto listado de Estados de registros");
        }
        public async Task OnSubmit(G180Estado elEdo)
        {
            G180Estado res = null;
            if (elEdo.Id != 0)
            {
                res = await EstadoIServ.UpdateEstado(elEdo);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"Agrego registro {elEdo.Titulo} del grupo {elEdo.Grupo}");
            }
            else
            {
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                    $"Consulto registro {elEdo.Id} de {elEdo.Titulo} del grupo {elEdo.Grupo}");
                res = await EstadoIServ.AddEstado(elEdo);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"Actualizo registro {elEdo.Id} de {elEdo.Titulo} del grupo {elEdo.Grupo}");
            }

            if (res != null) NM.NavigateTo("/sistema/estado");
        }
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

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;


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
            await EscribirBitacoraUno(2, BitaAcciones.Consultar, false,
                "Consulto listado de Estados de registros");
        }
        public async Task OnSubmit(G180Estado elEdo)
        {
            G180Estado res = null;
            if (elEdo.Id != 0)
            {
                res = await iG180EstadoServ.UpdateEstado(elEdo);
                await EscribirBitacoraUno(2, BitaAcciones.Agregar, false,
                    $"Agrego registro {elEdo.Titulo} del grupo {elEdo.Grupo}");
            }
            else
            {
                await EscribirBitacoraUno(2, BitaAcciones.Consultar, false,
                    $"Consulto registro {elEdo.Id} de {elEdo.Titulo} del grupo {elEdo.Grupo}");
                res = await iG180EstadoServ.AddEstado(elEdo);
                await EscribirBitacoraUno(2, BitaAcciones.Editar, false,
                    $"Actualizo registro {elEdo.Id} de {elEdo.Titulo} del grupo {elEdo.Grupo}");
            }

            if (res != null) NM.NavigateTo("/sistema/estado");
        }
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

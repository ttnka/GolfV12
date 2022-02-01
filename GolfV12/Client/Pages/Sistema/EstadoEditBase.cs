using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
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
        public string ButtonTexto { get; set; } = "Actualizar";
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
                    Estado = 1,
                    Status = true
                };
            } else 
            {
                ElEstado = await iG180EstadoServ.GetEstado(Id);
            } 
        }
        public async Task OnSubmit(G180Estado elEdo)
        {

            G180Estado res = null;
            if (elEdo.Id != 0)
            {
                res = await iG180EstadoServ.UpdateEstado(elEdo);
            }
            else
            {
                res = await iG180EstadoServ.AddEstado(elEdo);
                
            }

            if (res != null) NM.NavigateTo("/sistema/estado");
        }
       
    }
}

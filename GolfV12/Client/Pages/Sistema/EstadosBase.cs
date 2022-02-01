using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.Sistema
{
    public class EstadosBase : ComponentBase
    {
        [Inject]
        public IG180EstadoServ EdosIServ { get; set; }
    
        public IEnumerable<G180Estado> LosEstados { get; set; }
        protected async override Task OnInitializedAsync()
        {
            LosEstados = await EdosIServ.GetEstados();           
        }
    
    }
}

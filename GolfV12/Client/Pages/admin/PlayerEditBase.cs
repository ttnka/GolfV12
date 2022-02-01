using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace GolfV12.Client.Pages.admin
{
    public class PlayerEditBase : ComponentBase
    {
        [Inject]
        public IG110OrganizacionServ iOrgServ { get; set; }
        [Inject]
        public IG120PlayerServ iPlayerServ { get; set; }
        [Inject]
        public IG180EstadoServ iEstadoServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        public IEnumerable<G110Organizacion> LasOrg { get; set; }
        public int UnaOrg { get; set; }  
        public IEnumerable<G180Estado> LosEstados { get; set; }  
         
        [Parameter]
        public int Id { get; set; }
        public int ElEstado { get; set; }
        public G120Player ElPlayer { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";

        protected async override Task OnInitializedAsync()
        {
            LasOrg = await iOrgServ.GetOrganizaciones();
            if (Id == 0)
            {
                ElPlayer = new G120Player
                {
                    User = 3,
                    Nombre = "Nuevo", Paterno = "Apellido", Materno ="",Apodo = "",
                    Bday = DateTime.Parse("1974-09-12 00:00:00"),
                    //Date.("1974-09-12 00:00:00"),
                    Nivel = (Niveles)3,
                    Organizacion = await iOrgServ.GetOrganizacion(2),
                    Estado = 2,
                    Status = true,
                    Temporal = false
                };
                ButtonTexto = "Agregar";
            } else
            {
                ElPlayer = await iPlayerServ.GetPlayer(Id);
            }
            LosEstados = await iEstadoServ.GetEstados();
            ElEstado = ElPlayer.Estado;
        }
        
        protected async Task OnSubmit(G120Player updatePlayer)
        {
            G120Player resultado = null;
            ElPlayer.Organizacion = await iOrgServ.GetOrganizacion(UnaOrg);
            if (Id == 0)
            {
                resultado = await iPlayerServ.AddPlayer(updatePlayer);
            } else
            {
                resultado = await iPlayerServ.UpdatePlayer(updatePlayer);
            }
            if( resultado != null) NM.NavigateTo("/admin/player");
        }
        public IEnumerable<Niveles> LosNiveles = (IEnumerable<Niveles>)
                Enum.GetValues(typeof(Niveles));

        IEnumerable<DateTime> dates = new DateTime[] {DateTime.Today};    
        protected void DateRender(DateRenderEventArgs args)
        {
            for (int i = 1; i < 30; i++)
                { dates.Append<DateTime>(DateTime.Today.AddDays(-i)); }
            
            args.Disabled = dates.Contains(args.Date);
        }
        

    }
}

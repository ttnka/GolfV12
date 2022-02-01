using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.admin
{
    public class OrganizacionBase : ComponentBase
    {
        [Inject]
        public NavigationManager MN { get; set; }
        
        [Inject]
        public IG110OrganizacionServ organizacionIServ { get; set; }
        public IEnumerable<G110Organizacion> LasOrganizaciones { get; set; }
        protected override async Task OnInitializedAsync()
        {
            LasOrganizaciones = await organizacionIServ.GetOrganizaciones();
        }
        
    }
}

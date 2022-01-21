using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.admin
{
    public class OrganizacionEditBase : ComponentBase
    {
        [Inject]
        public IG110OrganizacionServ iOrgServ { get; set; }
        [Inject]
        public G180EstadoServ iEstadoServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        [Parameter]
        public int Id { get; set; }
        public G110Organizacion LaOrganizacion { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        public IEnumerable<G180Estado> LosEstados { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
            if (Id == 0)
            {
                LaOrganizacion = new G110Organizacion
                {
                    Clave = "Nueva organizacion",
                    Nombre = "Nueva",
                    Desc = " ",
                    Domicilio = "Conocido",
                    Estado = 1,
                    Status = true
                };
            } else
            {
                LaOrganizacion = await iOrgServ.GetOrganizacion(Id);
            }
            LosEstados = await iEstadoServ.get
        }
        public void ActualizarTexto()
        {
            
            if (Id == 0) ButtonTexto = "Agregar";
        }
        public async Task OnSubmit(G110Organizacion LaOrg)
        {
            G110Organizacion res = null;
            if (LaOrg.Id !=0 )
            {
                res = await iOrgServ.UpdateOrganizacion(LaOrg);
            } else
            {
                res = await iOrgServ.AddOrganizacion(LaOrg);
            }
            
            if (res != null) NM.NavigateTo("/admin/organizacion");
        }

    }
}

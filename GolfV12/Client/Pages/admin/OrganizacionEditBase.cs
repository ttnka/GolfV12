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
        public IG180EstadoServ iEstadoServ { get; set; }
        [Inject]
        public NavigationManager NM { get; set; }
        [Parameter]
        public int Id { get; set; }
        public G110Organizacion LaOrganizacion { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";
        public IEnumerable<G180Estado> LosEstados { get; set; }
        public int ElEstado { get; set; }
        protected async override Task OnInitializedAsync()
        {
            
            if (Id == 0)
            {   
                ButtonTexto = "Agregar Nuevo";
                LaOrganizacion = new G110Organizacion
                {
                    Clave = "Nueva organizacion",
                    Nombre = "Nueva",
                    Desc = " ",
                    Domicilio = "Conocido",
                    Estado = 2,
                    Status = true
                };
            } else
            {
                LaOrganizacion = await iOrgServ.GetOrganizacion(Id);   
            }
            LosEstados = await iEstadoServ.Buscar(titulo:"Vacio", grupo: "Organizacion");
            ElEstado = LaOrganizacion.Estado;
        }
        
        public async Task OnSubmit(G110Organizacion LaOrg)
        {
            G110Organizacion res = null;
            LaOrg.Estado = ElEstado;
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

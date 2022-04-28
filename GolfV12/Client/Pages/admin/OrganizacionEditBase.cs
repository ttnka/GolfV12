using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using GolfV12.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;


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
        //protected WBita WB { get; set; } = new WBita();
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

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
                LaOrganizacion = (await iOrgServ.Filtro($"org1_-_id_-_{Id}")).FirstOrDefault();
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                    $"Consulto el registro {LaOrganizacion.Id} de la organizacion {LaOrganizacion.Clave}");
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
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"Actualizo el registro {LaOrg.Id} con clave {LaOrg.Clave} titulo {LaOrg.Nombre}");
            } else
            {
                res = await iOrgServ.AddOrganizacion(LaOrg);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                    $"Agrego un registro {LaOrg.Clave} titulo {LaOrg.Nombre}");
            }
            
            if (res != null) NM.NavigateTo("/admin/organizacion");
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

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.admin
{
    public class PlayerEditBase : ComponentBase
    {
        [Inject]
        public IG110OrganizacionServ iOrgServ { get; set; }
        public IEnumerable<G110Organizacion> LasOrg { get; set; }
        [Inject]
        public IG120PlayerServ iPlayerServ { get; set; }
        [Parameter]
        public int Id { get; set; }
        public G120Player ElPlayer { get; set; }
        public string ButtonTexto { get; set; } = "Actualizar";

        protected override async Task OnInitializedAsync()
        {
            LasOrg = await iOrgServ.GetOrganizaciones();
            if (Id == 0)
            {
                ElPlayer = new G120Player();
            }
            ElPlayer = await iPlayerServ.GetPlayer(Id);
        }
        void ActualizarTexto()
        {
            if (Id == 0) ButtonTexto = "Agregar";
        }
        async void OnSubmit(G120Player updatePlayer)
        {
            await iPlayerServ.UpdatePlayer(updatePlayer);
        }

    }
}

using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;

namespace GolfV12.Client.Pages.admin
{
    public class PlayerBase : ComponentBase
    {
        [Inject]
        public IG120PlayerServ iG120PlayerServ { get; set; }
        public IEnumerable<G120Player> LosJugadores { get; set; }
        public NavigationManager MN { get; set; }    

        protected override async Task OnInitializedAsync()
        {
            LosJugadores = await iG120PlayerServ.GetPlayers();
        }
        protected void OnClick(int playerId)
        {
            if (playerId != 0) MN.NavigateTo($"/admin/playerEdit/{playerId}");
            MN.NavigateTo("/admin/playerEdit");
        }

    }
}

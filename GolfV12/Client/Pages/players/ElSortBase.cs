using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class ElSortBase : ComponentBase 
    {
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        public bool ElHcpB9 { get; set; } = false;
        [Parameter]
        public IEnumerable<TarjetaMolde> LosScores { get; set; } = new List<TarjetaMolde>();
        [Parameter]
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();

        public RadzenDataGrid<TarjetaMolde> F9Grid { get; set; } = new();
        public RadzenDataGrid<TarjetaMolde> B9Grid { get; set; } = new();
        public RadzenDataGrid<TarjetaMolde> TotalGrid { get; set; } = new();
        public bool isLoading { get; set; } = false;
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            CalLugares();
            //await LeerDatos();

        }

        protected void CalcularFB9()
        {
            isLoading = true;
            if (LosScores != null)
            {
                List<TarjetaMolde> Recalculo = new();
                foreach (var item in LosScores)
                {
                    item.HcpB9 = !item.HcpB9;
                    Recalculo.Add(item);
                }
                LosScores = Recalculo.AsEnumerable();
                CalLugares();
                
                F9Grid.Reload();
                B9Grid.Reload();
                TotalGrid.Reload();
                
                isLoading = false;
            }
        }
        protected void CalLugares()
        {
            List<int> LhitsF9 = new List<int>();
            List<int> LhitsB9 = new List<int>();
            List<int> LhitsT = new List<int>();
            
            foreach (var reg in LosScores)
            {
                LhitsF9.Add(reg.F9Hcp);
                LhitsB9.Add(reg.B9Hcp);
                LhitsT.Add(reg.TotalHcp);
                /*
                if (!LhitsF9.Contains(reg.F9Hcp))
                    LhitsF9.Add(reg.F9Hcp);
             
                if (!LhitsB9.Contains(reg.B9Hcp))
                    LhitsB9.Add(reg.B9Hcp);

                if (!LhitsT.Contains(reg.TotalHcp)) 
                    LhitsT.Add(reg.TotalHcp);
                */
            }
            

            int[] hitsF9 = LhitsF9.ToArray();
            int[] hitsB9 = LhitsB9.ToArray();
            int[] hitsT = LhitsT.ToArray();

            Array.Sort(hitsF9);
            Array.Sort(hitsB9);
            Array.Sort(hitsT);

            foreach (var reg in LosScores)
            {
                LugaresAdd("F9", reg.UserId, (Array.IndexOf(hitsF9, reg.F9Hcp)+1).ToString());
                LugaresAdd("B9", reg.UserId, (Array.IndexOf(hitsB9, reg.B9Hcp)+1).ToString());
                LugaresAdd("Total", reg.UserId, (Array.IndexOf(hitsT, reg.TotalHcp)+1).ToString());
            }
        }

        protected void LugaresAdd(string tabla, string jugador, string lugar)
        {
            if(DatosDic.ContainsKey($"TablaLugar{tabla}_jugador_{jugador}"))
                {
                DatosDic[$"TablaLugar{tabla}_jugador_{jugador}"] = lugar;
            }
                else
            {
                DatosDic.Add($"TablaLugar{tabla}_jugador_{jugador}", lugar);
            }
        }
        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; } = string.Empty;
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

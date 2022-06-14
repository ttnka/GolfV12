using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using GolfV12.Client.Shared;

namespace GolfV12.Client.Pages.players
{
    public class MisBolitasBase : ComponentBase 
    {
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        public bool ElHcpB9 { get; set; } = false;
        [Parameter]
        public string ElPadreId {get; set; } = string.Empty;
        [Parameter]
        public IEnumerable<TarjetaMolde> LosScores { get; set; } = new List<TarjetaMolde>();
        [Parameter]
        public IEnumerable<TarjetaMolde> LasBolitas { get; set; } = new List<TarjetaMolde>();

        [Parameter]
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        [Parameter]
        public Dictionary<string, int> LosExtrasDic { get; set; } = new Dictionary<string, int>();
        public RadzenDataGrid<TarjetaMolde> BolitasGrid { get; set; } = new(); 
        public CalcularBolitas ElCalculo { get; set; } = new CalcularBolitas();
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            CalcularLasBolitas();
            //if (LasBolitas != null) CalcularLosImportes();
        }
        protected void CalcularLasBolitas()
        { 
            if(LosScores != null )
            {
                int[] Dif = new int[18];
                for (int i = 0; i < Dif.Length; i++)
                {
                    Dif[i] = DatosDic.ContainsKey($"HoyoH_{i + 1}") ? int.Parse(DatosDic[$"HoyoH_{i + 1}"]) : 0; 
                    
                }
                LasBolitas = ElCalculo.CalculoGeneral(LosScores, TarjetaId, ElPadreId, Dif);
                if (LasBolitas != null)
                {
                    foreach (var item in LasBolitas)
                    {
                        int ExtH1 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_1"))
                            ExtH1 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_1"];
                        item.H1 += ExtH1;
                        
                        int ExtH2 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_2"))
                            ExtH2 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_2"];
                        item.H2 += ExtH2;
                        
                        int ExtH3 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_3"))
                            ExtH3 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_3"];
                        item.H3 += ExtH3;
                        
                        int ExtH4 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_4"))
                            ExtH4 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_4"];
                        item.H4 += ExtH4;
                        
                        int ExtH5 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_5"))
                            ExtH5 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_5"];
                        item.H5 += ExtH5;
                        
                        int ExtH6 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_6"))
                            ExtH6 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_6"];
                        item.H6 += ExtH6;
                        
                        int ExtH7 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_7"))
                            ExtH7 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_7"];
                        item.H7 += ExtH7;
                        
                        int ExtH8 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_8"))
                            ExtH8 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_8"];
                        item.H8 += ExtH8;
                        
                        int ExtH9 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_9"))
                            ExtH9 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_9"];
                        item.H9 += ExtH9;
                        
                        int ExtH10 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_10"))
                            ExtH10 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_10"];
                        item.H10 += ExtH10;
                        
                        int ExtH11 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_11"))
                            ExtH11 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_11"];
                        item.H11 += ExtH11;
                        
                        int ExtH12 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_12"))
                            ExtH12 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_12"];
                        item.H12 += ExtH12;
                        
                        int ExtH13 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_13"))
                            ExtH13 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_13"];
                        item.H13 += ExtH13;
                        
                        int ExtH14 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_14"))
                            ExtH14 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_14"];
                        item.H14 += ExtH14;
                        
                        int ExtH15 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_15"))
                            ExtH15 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_15"];
                        item.H15 += ExtH15;
                        
                        int ExtH16 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_16"))
                            ExtH16 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_16"];
                        item.H16 += ExtH16;
                        
                        int ExtH17 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_17"))
                            ExtH17 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_17"];
                        item.H17 += ExtH17;
                        
                        int ExtH18 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{item.UserId}_Hoyo_18"))
                            ExtH18 = LosExtrasDic[$"Extras_{item.UserId}_Hoyo_18"];
                        item.H18 += ExtH18;
                    }
                }
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

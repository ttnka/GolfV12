using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players.retos
{
    public class LasBolitasResBase : ComponentBase 
    {
        [Inject]
        public IG320BolitasServ BolitasIServ { get; set; }
        [Parameter]
        public string ElJugadorId { get; set; } = string.Empty;
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        [Parameter]
        public string AzarId { get; set; } = string.Empty;
        [Parameter]
        public Dictionary<string, G120Player> JugadoresDic { get; set; } = new Dictionary<string, G120Player>();
        
        public IEnumerable<G320Bolitas> LasBolitasRes { get; set; } = new List<G320Bolitas>();
        public IEnumerable<KeyValuePair<string, string>> TiposAzar { get; set; } =
            new List<KeyValuePair<string, string>>();
        [Inject]
        public IG120PlayerServ PlayerIServ { get; set; }
        public IEnumerable<KeyValuePair<string, string>> LosNombres { get; set; } =
            new List<KeyValuePair<string, string>>();
        [Inject]
        public IG300AzarServ AzarIServ { get; set; }
        [Parameter]
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public bool Acumulado { get; set; } = true;
        [Parameter]
        public Dictionary<string, G320Bolitas> LasBolitasDic { get; set; } = new Dictionary<string, G320Bolitas>();
        [Parameter]
        public Dictionary<string, int> LosExtrasDic { get; set; } = new Dictionary<string, int>();
        public RadzenDataGrid<G320Bolitas> BolitasResGrid { get; set; } = new();
        [Inject]
        public NavigationManager NM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerDatos();
            //await LeerNombres();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                "El Usuario Consulto los retos");
        }
        protected async Task LeerDatos()
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            List<G320Bolitas> keyValues = new List<G320Bolitas>();
            
            foreach (var item in JugadoresDic)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(item.Key, $"{item.Value.Nombre} {item.Value.Apodo} {item.Value.Paterno}"));
                
            }
            LosNombres = keyValuePairs.AsEnumerable();

            var LasBolitasValores = await BolitasIServ.Filtro($"bol2azar_-_azar_-_{AzarId}");
            if (LasBolitasValores != null)
            {
                int renglon = 1;
                foreach (var LBV in LasBolitasValores)
                {
                    if (!DatosDic.ContainsKey($"Renglon_{LBV.J1}"))
                    {
                        DatosDic.Add($"Renglon_{LBV.J1}", (renglon).ToString());
                        renglon++;
                    }
                    
                    if (LasBolitasDic.ContainsKey(LBV.J1))
                    {
                        G320Bolitas g320Bolitas = new G320Bolitas();
                        int acu = 0;
                        int P = LBV.Precio;
                        
                        g320Bolitas.J1 = LBV.J1;
                        g320Bolitas.J2 = LBV.J2;
                        if (ElJugadorId != LBV.J1)
                        { 
                            g320Bolitas.J1 = LBV.J2;
                            g320Bolitas.J2 = LBV.J1;
                            P = -P;    
                        }
                      
                        g320Bolitas.Tarjeta = TarjetaId;
                        g320Bolitas.Azar = LBV.Azar;
                        g320Bolitas.Precio = Math.Abs(P);
                        int ExtH1 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_1")) 
                            ExtH1 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_1"];
                        g320Bolitas.H1V = (P * LasBolitasDic[LBV.J1].H1V * LBV.H1V) + (P * ExtH1);
                        acu += Acumulado ? (P * LasBolitasDic[LBV.J1].H1V * LBV.H1V + P * ExtH1 ): 0;
                        
                        int ExtH2 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_2"))
                            ExtH2 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_2"];
                        g320Bolitas.H2V = (P * LasBolitasDic[LBV.J1].H2V * LBV.H2V) + (P * ExtH2) + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H2V * LBV.H2V + P * ExtH2 : 0;
                        
                        int ExtH3 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_3"))
                            ExtH3 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_3"];
                        g320Bolitas.H3V = (P * LasBolitasDic[LBV.J1].H3V * LBV.H3V) + (P * ExtH3) + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H3V * LBV.H3V + P * ExtH3 : 0;

                        int ExtH4 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_4"))
                            ExtH4 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_4"];
                        g320Bolitas.H4V = (P * LasBolitasDic[LBV.J1].H4V * LBV.H4V) + (P * ExtH4) + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H4V * LBV.H4V + P * ExtH4 : 0;

                        int ExtH5 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_5"))
                            ExtH5 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_5"];
                        g320Bolitas.H5V = (P * LasBolitasDic[LBV.J1].H5V * LBV.H5V) + P * ExtH5 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H5V * LBV.H5V + P * ExtH5 : 0;

                        int ExtH6 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_6"))
                            ExtH6 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_6"];
                        g320Bolitas.H6V = (P * LasBolitasDic[LBV.J1].H6V * LBV.H6V) + P * ExtH6 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H6V * LBV.H6V + P * ExtH6 : 0;

                        int ExtH7 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_7"))
                            ExtH7 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_7"];
                        g320Bolitas.H7V = (P * LasBolitasDic[LBV.J1].H7V * LBV.H7V) + P * ExtH7 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H7V * LBV.H7V + P * ExtH7 : 0;

                        int ExtH8 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_8"))
                            ExtH8 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_8"];
                        g320Bolitas.H8V = (P * LasBolitasDic[LBV.J1].H8V * LBV.H8V) + P * ExtH8 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H8V * LBV.H8V + P * ExtH8 : 0;

                        int ExtH9 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_9"))
                            ExtH9 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_9"];
                        g320Bolitas.H9V = (P * LasBolitasDic[LBV.J1].H9V * LBV.H9V) + P * ExtH9 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H9V * LBV.H9V + P * ExtH9 : 0;

                        int ExtH10 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_10"))
                            ExtH10 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_10"];
                        g320Bolitas.H10V = (P * LasBolitasDic[LBV.J1].H10V * LBV.H10V) + P * ExtH10 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H10V * LBV.H10V + P * ExtH10 : 0;

                        int ExtH11 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_11"))
                            ExtH11 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_11"];
                        g320Bolitas.H11V = (P * LasBolitasDic[LBV.J1].H11V * LBV.H11V) + P * ExtH11 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H11V * LBV.H11V + P * ExtH11 : 0;

                        int ExtH12 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_12"))
                            ExtH12 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_12"];
                        g320Bolitas.H12V = (P * LasBolitasDic[LBV.J1].H12V * LBV.H12V) + P * ExtH12 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H12V * LBV.H12V + P * ExtH12 : 0;

                        int ExtH13 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_13"))
                            ExtH13 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_13"];
                        g320Bolitas.H13V = (P * LasBolitasDic[LBV.J1].H13V * LBV.H13V) + P * ExtH13 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H13V * LBV.H13V + P * ExtH13 : 0;

                        int ExtH14 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_14"))
                            ExtH14 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_14"];
                        g320Bolitas.H14V = (P * LasBolitasDic[LBV.J1].H14V * LBV.H14V) + P*ExtH14 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H14V * LBV.H14V + P*ExtH14 : 0;

                        int ExtH15 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_15"))
                            ExtH15 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_15"];
                        g320Bolitas.H15V = (P * LasBolitasDic[LBV.J1].H15V * LBV.H15V) + P*ExtH15 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H15V * LBV.H15V + P*ExtH15 : 0;

                        int ExtH16 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_16"))
                            ExtH16 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_16"];
                        g320Bolitas.H16V = (P * LasBolitasDic[LBV.J1].H16V * LBV.H16V)+ P*ExtH16 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H16V * LBV.H16V + P*ExtH16 : 0;

                        int ExtH17 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_17"))
                            ExtH17 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_17"];
                        g320Bolitas.H17V = (P * LasBolitasDic[LBV.J1].H17V * LBV.H17V) + P*ExtH17 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H17V * LBV.H17V + P * ExtH17 : 0;

                        int ExtH18 = 0;
                        if (LosExtrasDic.ContainsKey($"Extras_{LBV.J1}_Hoyo_18"))
                            ExtH18 = LosExtrasDic[$"Extras_{LBV.J1}_Hoyo_18"];
                        g320Bolitas.H18V = (P * LasBolitasDic[LBV.J1].H18V * LBV.H18V) + P*ExtH18 + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].H18V * LBV.H18V + P * ExtH18 : 0;

                        g320Bolitas.F9V = (P * LasBolitasDic[LBV.J1].F9V * LBV.F9V) + acu;
                        acu += Acumulado ? P * LasBolitasDic[LBV.J1].F9V * LBV.F9V : 0;

                        g320Bolitas.B9V = (P * LasBolitasDic[LBV.J1].B9V * LBV.B9V) + acu;
                        g320Bolitas.TotalV = P * LasBolitasDic[LBV.J1].TotalV * LBV.TotalV; 
                        
                        keyValues.Add(g320Bolitas);
                    }
                            
                }
            }
            LasBolitasRes = keyValues.AsEnumerable();
        }
        public NotificationMessage elMessage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Cuerpo",
            Detail = "Detalles ",
            Duration = 3000
        };
        [Inject]
        public NotificationService NS { get; set; } = new();

        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; } = String.Empty;
        // Bitacora

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

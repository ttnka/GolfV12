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
        [Parameter]
        public Dictionary<string, G320Bolitas> LasBolitasDic { get; set; } = new Dictionary<string, G320Bolitas>();
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
                foreach (var tipo in LasBolitasValores)
                {
                    if (!DatosDic.ContainsKey($"Renglon_{tipo.Id}"))
                    {
                        DatosDic.Add($"Renglon_{tipo.Id}", (renglon).ToString());
                        renglon++;
                    }
                    
                    if (LasBolitasDic.ContainsKey(tipo.J1))
                    {
                        G320Bolitas g320Bolitas = new G320Bolitas();
                        int P = tipo.Precio;
                        
                        g320Bolitas.J1 = tipo.J1;
                        g320Bolitas.J2 = tipo.J2;
                        if (ElJugadorId != tipo.J1)
                        { 
                            g320Bolitas.J1 = tipo.J2;
                            g320Bolitas.J2 = tipo.J1;
                            P = -P;
                        } 

                        g320Bolitas.Tarjeta = TarjetaId;
                        g320Bolitas.Azar = tipo.Azar;
                        g320Bolitas.Precio = Math.Abs(P);
                        g320Bolitas.H1V = P * LasBolitasDic[tipo.J1].H1V * tipo.H1V;
                        g320Bolitas.H2V = P * LasBolitasDic[tipo.J1].H2V * tipo.H2V;
                        g320Bolitas.H3V = P * LasBolitasDic[tipo.J1].H3V * tipo.H3V;
                        g320Bolitas.H4V = P * LasBolitasDic[tipo.J1].H4V * tipo.H4V;
                        g320Bolitas.H5V = P * LasBolitasDic[tipo.J1].H5V * tipo.H5V;
                        g320Bolitas.H6V = P * LasBolitasDic[tipo.J1].H6V * tipo.H6V;
                        g320Bolitas.H7V = P * LasBolitasDic[tipo.J1].H7V * tipo.H7V;
                        g320Bolitas.H8V = P * LasBolitasDic[tipo.J1].H8V * tipo.H8V;
                        g320Bolitas.H9V = P * LasBolitasDic[tipo.J1].H9V * tipo.H9V;
                        g320Bolitas.H10V = P * LasBolitasDic[tipo.J1].H10V * tipo.H10V;
                        g320Bolitas.H11V = P * LasBolitasDic[tipo.J1].H11V * tipo.H11V;
                        g320Bolitas.H12V = P * LasBolitasDic[tipo.J1].H12V * tipo.H12V;
                        g320Bolitas.H13V = P * LasBolitasDic[tipo.J1].H13V * tipo.H13V;
                        g320Bolitas.H14V = P * LasBolitasDic[tipo.J1].H14V * tipo.H14V;
                        g320Bolitas.H15V = P * LasBolitasDic[tipo.J1].H15V * tipo.H15V;
                        g320Bolitas.H16V = P * LasBolitasDic[tipo.J1].H16V * tipo.H16V;
                        g320Bolitas.H17V = P * LasBolitasDic[tipo.J1].H17V * tipo.H17V;
                        g320Bolitas.H18V = P * LasBolitasDic[tipo.J1].H18V * tipo.H18V;
                        g320Bolitas.F9V = P * LasBolitasDic[tipo.J1].F9V * tipo.F9V;
                        g320Bolitas.B9V = P * LasBolitasDic[tipo.J1].B9V * tipo.B9V; 
                        g320Bolitas.TotalV = P * LasBolitasDic[tipo.J1].TotalV * tipo.TotalV;

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

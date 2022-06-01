using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

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

        public RadzenDataGrid<TarjetaMolde> BolitasGrid { get; set; } = new();
        
        public bool isLoading { get; set; } = false;
        protected async override Task OnInitializedAsync()
        {
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            Calcular();
            
            //await LeerDatos();

        }

        protected void Calcular()
        {
            List<TarjetaMolde> BList = new List<TarjetaMolde>();
            if (LosScores != null )
            {
                foreach (var padre in LosScores)
                {
                    if (padre.UserId == ElPadreId)
                    {
                        int[] ElPadreScore = new int[18];
                        int[] PadreScore = new int[] {padre.H1,padre.H2,padre.H3,padre.H4,padre.H5,padre.H6,padre.H7,
                                            padre.H8,padre.H9,padre.H10,padre.H11,padre.H12,padre.H13,padre.H14,padre.H15,
                                            padre.H16,padre.H17,padre.H18 };
                        
                        foreach (var hijo in LosScores)
                        {
                            if (padre.UserId != hijo.UserId)
                            {
                                TarjetaMolde ElPadre = new TarjetaMolde();
                                ElPadre.UserId = padre.UserId;
                                ElPadre.Hijo = hijo.UserId;
                                ElPadre.Tarjeta = TarjetaId;

                                int[] HijoScore = new int[18] {hijo.H1,hijo.H2,hijo.H3,hijo.H4,hijo.H5,hijo.H6,hijo.H7,
                                            hijo.H8,hijo.H9,hijo.H10,hijo.H11,hijo.H12,hijo.H13,hijo.H14,hijo.H15,
                                            hijo.H16,hijo.H17,hijo.H18};

                                for (int h = 0; h < 18; h++)
                                {
                                    if (PadreScore[h] > 0 && HijoScore[h] > 0)
                                    {
                                        int signo = padre.Hcp < hijo.Hcp ? -1 : 1;
                                        int bolita = Math.Abs(padre.Hcp - hijo.Hcp) / 18;
                                        bolita += Math.Abs(padre.Hcp - hijo.Hcp) % 18 > 
                                                int.Parse(DatosDic[$"HoyoH_{h + 1}"]) ? 1 : 0;
                                        bolita = PadreScore[h] - bolita * signo - HijoScore[h];
                                        ElPadreScore[h] = bolita < 0 ? 1 : -1;
                                        if (bolita == 0) ElPadreScore[h] = 0;       
                                    }
                                }
// Alimentar ElPadre
                                { 
                                    ElPadre.H1 = ElPadreScore[0];
                                    ElPadre.H2 = ElPadreScore[1];
                                    ElPadre.H3 = ElPadreScore[2];
                                    ElPadre.H4 = ElPadreScore[3];
                                    ElPadre.H5 = ElPadreScore[4];
                                    ElPadre.H6 = ElPadreScore[5];
                                    ElPadre.H7 = ElPadreScore[6];
                                    ElPadre.H8 = ElPadreScore[7];
                                    ElPadre.H9 = ElPadreScore[8];
                                    ElPadre.H10 = ElPadreScore[9];
                                    ElPadre.H11 = ElPadreScore[10];
                                    ElPadre.H12 = ElPadreScore[11];
                                    ElPadre.H13 = ElPadreScore[12];
                                    ElPadre.H14 = ElPadreScore[13];
                                    ElPadre.H15 = ElPadreScore[14];
                                    ElPadre.H16 = ElPadreScore[15];
                                    ElPadre.H17 = ElPadreScore[16];
                                    ElPadre.H18 = ElPadreScore[17];
                                }        
                                BList.Add(ElPadre);
                            }
                        }
                    }
                }
            }
            LasBolitas = BList.AsEnumerable();
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

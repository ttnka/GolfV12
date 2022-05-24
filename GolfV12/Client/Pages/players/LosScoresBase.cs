using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class LosScoresBase : ComponentBase 
    {
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;

        [Inject]
        public IG120PlayerServ NombresIServ { get; set; }

        [Inject]
        public IG510JugadorServ JugadorIServ { get; set; }
        public IEnumerable<G510Jugador> LosJugadores { get; set; } = new List<G510Jugador>();
        public G510Jugador ElJugador { get; set; } = new G510Jugador();

        [Inject]
        public IG520ScoreServ ScoreIServ { get; set; }
        public IEnumerable<TarjetaMolde> LosScores { get; set; } = new List<TarjetaMolde>();
        public G520Score ElScore { get; set; } = new G520Score();

        public int ElHoyo { get; set; } = 1;
        public IEnumerable<int> LosHoyos { get; set; } = new List<int>();
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<TarjetaMolde> ScoreGrid { get; set; } = new();
        [Inject]
        public NavigationManager NM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(TarjetaId))
                NM.NavigateTo("/players/misdatos");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerNombres();
            
            await LeerJugadores();
            await LeerScores();
            LeerLosHoyos();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El usuario consulto el listado de jugadores de la tarjeta {TarjetaId}");
        }
        protected async Task LeerNombres()
        {
            var nombTemp = await NombresIServ.Filtro("all");
            foreach (var nomb in nombTemp)
            {
                if (!DatosDic.ContainsKey($"Nombre_{nomb.UserId}")) DatosDic.Add($"Nombre_{nomb.UserId}", $"{nomb.Apodo} {nomb.Nombre} {nomb.Paterno}");
            }
        }
        protected async Task LeerJugadores()
        {
            var JugadoresTemp = await JugadorIServ.Filtro($"jug2tarjeta_-_tarjeta_-_{TarjetaId}");
            LosJugadores = JugadoresTemp;
            foreach (var nomb in JugadoresTemp)
            {
                TotalesCal(nomb.Player, 0, 0, "");
            }
        }
        
        protected void TotalesCal(string jugador, int hoyo, int newScore, string scoreId)
        {
            string acumula = $"Jugador_{jugador}_";
            acumula += hoyo < 10 ? "F9" : "B9";
            
            if ( hoyo == 0)
            {
                if (!DatosDic.ContainsKey(acumula))
                    DatosDic.Add($"Jugador_{ jugador}_Total", "0");
                    DatosDic.Add($"Jugador_{ jugador}_F9", "0");
                    DatosDic.Add($"Jugador_{ jugador}_B9", "0");
            } 
            else
            {
                if (!DatosDic.ContainsKey($"Jugador_{jugador}_Hoyo_{hoyo}"))
                {
                    DatosDic.Add($"Jugador_{jugador}_Hoyo_{hoyo}", newScore.ToString());
                    DatosDic.Add($"Jugador_{jugador}_HoyoId_{hoyo}", scoreId);
                    DatosDic[$"Jugador_{jugador}_Total"] = 
                        (int.Parse(DatosDic[$"Jugador_{jugador}_Total"]) + newScore).ToString();

                    DatosDic[acumula] = (int.Parse(DatosDic[acumula]) + newScore).ToString();
                }
                else
                {
                    int OldScore = int.Parse(DatosDic[$"Jugador_{jugador}_Hoyo_{hoyo}"]);
                    DatosDic[$"Jugador_{jugador}_Hoyo_{hoyo}"] = newScore.ToString();

                    DatosDic[$"Jugador_{jugador}_Total"] = 
                        (int.Parse(DatosDic[$"Jugador_{jugador}_Total"]) + newScore - OldScore).ToString();
                    DatosDic[acumula] = 
                        (int.Parse(DatosDic[acumula]) + newScore - OldScore).ToString();
                }
            }
        }

        protected async Task UpdateHcpPlayer(G510Jugador jugTemp)
        {
            var res = await JugadorIServ.UpdateJugador(jugTemp);
            if (res.Id == jugTemp.Id) await LeerScores();
               
        }
        protected async Task LeerScores()
        {
            var LosDatosTemp = await ScoreIServ.Filtro($"sco2tarjeta_-_tarjeta_-_{TarjetaId}");
            List<TarjetaMolde> ListaTM = new List<TarjetaMolde>();
            
            if (LosDatosTemp != null)
            {
                foreach (var jugTemp in LosDatosTemp)
                {
                    TotalesCal(jugTemp.Player, jugTemp.Hoyo, jugTemp.Score, jugTemp.Id);     
                }
            }
            if (LosJugadores != null)
            {
                int renglon = 0;
                foreach (var TM in LosJugadores)
                {

                    renglon++;
                    TarjetaMolde tarjetaMolde = new();               

                    tarjetaMolde.Renglon = renglon;
                    tarjetaMolde.UserId = TM.Player.ToString();
                    tarjetaMolde.hcpId = TM.Id;
                    tarjetaMolde.hcp = Decimal.ToInt32(TM.Hcp) < 1 ? 0  : Decimal.ToInt32(TM.Hcp);
                    {
                        tarjetaMolde.H1 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{1}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{1}"]) : 0;
                        tarjetaMolde.H2 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{2}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{2}"]) : 0;
                        tarjetaMolde.H3 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{3}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{3}"]) : 0;
                        tarjetaMolde.H4 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{4}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{4}"]) : 0;
                        tarjetaMolde.H5 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{5}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{5}"]) : 0;
                        tarjetaMolde.H6 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{6}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{6}"]) : 0;
                        tarjetaMolde.H7 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{7}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{7}"]) : 0;
                        tarjetaMolde.H8 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{8}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{8}"]) : 0;
                        tarjetaMolde.H9 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{9}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{9}"]) : 0;
                        tarjetaMolde.H10 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{10}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{10}"]) : 0;
                        tarjetaMolde.H11 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{11}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{11}"]) : 0;
                        tarjetaMolde.H12 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{12}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{12}"]) : 0;
                        tarjetaMolde.H13 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{13}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{13}"]) : 0;
                        tarjetaMolde.H14 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{14}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{14}"]) : 0;
                        tarjetaMolde.H15 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{15}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{15}"]) : 0;
                        tarjetaMolde.H16 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{16}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{16}"]) : 0;
                        tarjetaMolde.H17 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{17}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{17}"]) : 0;
                        tarjetaMolde.H18 = DatosDic.ContainsKey($"Jugador_{TM.Player}_Hoyo_{18}") ?
                                int.Parse(DatosDic[$"Jugador_{TM.Player}_Hoyo_{18}"]) : 0;
                    }

                    {
                        tarjetaMolde.H1Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{1}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{1}"]) : " ";
                        tarjetaMolde.H2Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{2}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{2}"]) : " ";
                        tarjetaMolde.H3Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{3}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{3}"]) : " ";
                        tarjetaMolde.H4Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{4}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{4}"]) : " ";
                        tarjetaMolde.H5Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{5}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{5}"]) : " ";
                        tarjetaMolde.H6Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{6}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{6}"]) : " ";
                        tarjetaMolde.H7Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{7}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{7}"]) : " ";
                        tarjetaMolde.H8Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{8}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{8}"]) : " ";
                        tarjetaMolde.H9Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{9}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{9}"]) : " ";
                        tarjetaMolde.H10Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{10}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{10}"]) : " ";
                        tarjetaMolde.H11Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{11}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{11}"]) : " ";
                        tarjetaMolde.H12Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{12}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{12}"]) : " ";
                        tarjetaMolde.H13Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{13}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{13}"]) : " ";
                        tarjetaMolde.H14Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{14}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{14}"]) : " ";
                        tarjetaMolde.H15Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{15}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{15}"]) : " ";
                        tarjetaMolde.H16Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{16}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{16}"]) : " ";
                        tarjetaMolde.H17Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{17}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{17}"]) : " ";
                        tarjetaMolde.H18Id = DatosDic.ContainsKey($"Jugador_{TM.Player}_HoyoId_{18}") ?
                                (DatosDic[$"Jugador_{TM.Player}_HoyoId_{18}"]) : " ";
                    }

                    ListaTM.Add(tarjetaMolde);
                }
            }          
            LosScores = ListaTM.AsEnumerable();
        }
        protected void LeerLosHoyos()
        {
            IList<int> ListaTemp = new List<int>();
            for (int i = 1; i < 19; i++)
            {
                ListaTemp.Add(i);
            }
            LosHoyos = ListaTemp.AsEnumerable();    
        }
        protected async Task AddUpdateScore(G520Score Datos)
        {
            var res = DatosDic.ContainsKey($"Jugador_{Datos.Player}_Hoyo_{Datos.Hoyo}") ?
                await ScoreIServ.UpdateScore(Datos) : await ScoreIServ.AddScore(Datos);

            TotalesCal(Datos.Player, Datos.Hoyo, Datos.Score, Datos.Id);
            
        }

        

        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; }
        public string UserIdLog { get; set; } = String.Empty;

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

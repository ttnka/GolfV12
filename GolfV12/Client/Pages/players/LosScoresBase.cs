using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.players
{
    public class LosScoresBase : ComponentBase 
    {
        [Parameter]
        public string TarjetaId { get; set; } = string.Empty;
        public G500Tarjeta LaTarjeta { get; set; } = new G500Tarjeta();
        [Inject]
        public IG500TarjetaServ TarjetasIServ { get; set; }

        [Inject]
        public IG120PlayerServ NombresIServ { get; set; }
        [Inject]
        public IG176HoyoServ HoyoIServ { get; set; }
        [Inject]
        public IG510JugadorServ JugadorIServ { get; set; }
        public IEnumerable<G510Jugador> LosJugadores { get; set; } = new List<G510Jugador>();
        public G510Jugador ElJugador { get; set; } = new G510Jugador();

        [Inject]
        public IG520ScoreServ ScoreIServ { get; set; }
        public IEnumerable<TarjetaMolde> LosScores { get; set; } = new List<TarjetaMolde>();
        public G520Score ElScore { get; set; } = new G520Score();

        public IEnumerable<int> LosHoyos { get; set; } = new List<int>();
        public Dictionary<string, string> DatosDic { get; set; } = new Dictionary<string, string>();
        public RadzenDataGrid<TarjetaMolde> ScoreGrid { get; set; } = new();
        [Inject]
        public NavigationManager NM { get; set; }
        public bool Calculando { get; set; } = false;
        public List<KeyValuePair<string, G510Jugador>> ListaHcp { get; set; } = new List<KeyValuePair<string, G510Jugador>>();
        public List<KeyValuePair<string, G520Score>> ListaScores { get; set; } = new List<KeyValuePair<string, G520Score>>();

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(TarjetaId))
                NM.NavigateTo("/players/misdatos");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            
            await LeerJugadores();
            await LeerScores();
            LeerLosHoyos();
            await LeerPermisos();

            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El usuario consulto el listado de jugadores de la tarjeta {TarjetaId}");
            
        }
        protected void ListaInvolucrados(string useridInt)
        {
            if (DatosDic.ContainsKey($"Involucrados_{TarjetaId}"))
            {
                DatosDic[$"Involucrados_{TarjetaId}"] += $",{useridInt}";
            }
            else
            {
                DatosDic.Add ($"Involucrados_{TarjetaId}", useridInt);
            }
        }
        protected async Task LeerPermisos()
        {
            LaTarjeta = (await TarjetasIServ.Filtro($"tar1id_-_id_-_{TarjetaId}")).FirstOrDefault();
            
            if (!DatosDic.ContainsKey($"PermisoLeer_{TarjetaId}_Usuario_{UserIdLog}"))
            {
                DatosDic.Add($"PermisoLeer_{TarjetaId}_Usuario_{UserIdLog}", "0");
                DatosDic.Add($"PermisoEscribir_{TarjetaId}_Usuario_{UserIdLog}", "0");
            }
            if (LaTarjeta != null)
            {
                string[] involucrados = DatosDic[$"Involucrados_{TarjetaId}"].Split(",");
                // LEER creador captura, jugador todos
                if (LaTarjeta.Consulta == TorneoView.Capturista)
                {
                    // PENDIENTE
                }
                else
                {   
                    if (involucrados.Any(UserIdLog.Contains))
                        DatosDic[$"PermisoLeer_{TarjetaId}_Usuario_{UserIdLog}"] = "1";
                }
                // Escribir Creador Captura dif jugador

                if (LaTarjeta.Estado != 3)
                {                
                    if ((LaTarjeta.Captura == Torneo2Edit.Jugadores &&
                                involucrados.Any(UserIdLog.Contains)))
                                            DatosDic[$"PermisoEscribir_{TarjetaId}_Usuario_{UserIdLog}"] = "1";

                    /* Esta caso es solo cada jugador sin creador 
                        if ((LaTarjeta.Captura == Torneo2Edit.Creador &&
                                    LaTarjeta.Creador == UserIdLog))
                                                DatosDic[$"PermisoEscribir_{TarjetaId}_Usuario_{UserIdLog}"] = "2";
                    */

                    if (LaTarjeta.Creador == UserIdLog)
                        DatosDic[$"PermisoEscribir_{TarjetaId}_Usuario_{UserIdLog}"] = "2";
                }

                // Leer Par y Dificultad

                var ParTemp = await HoyoIServ.Filtro($"hoy1campo_-_campo_-_{LaTarjeta.Campo}");
                if (ParTemp != null)
                {
                    foreach (var item in ParTemp)
                    {
                        if (!DatosDic.ContainsKey($"HoyoPar_{item.Hoyo}"))
                        {
                            DatosDic.Add($"HoyoPar_{item.Hoyo}", item.Par.ToString());
                            DatosDic.Add($"HoyoH_{item.Hoyo}", item.HcpHombres.ToString());
                            DatosDic.Add($"HoyoW_{item.Hoyo}", item.HcpMujeres.ToString());
                        }
                    }
                }
            }
        }
        protected async Task LeerJugadores()
        {
            var JugadoresTemp = await JugadorIServ.Filtro($"jug2tarjeta_-_tarjeta_-_{TarjetaId}");
            LosJugadores = JugadoresTemp.AsEnumerable();
            if (JugadoresTemp != null )
            {
                foreach (var nomb in JugadoresTemp)
                {
                    if (!DatosDic.ContainsKey($"Nombre_{nomb.Player}"))
                    {
                        var PlayerName = (await NombresIServ.Filtro($"play1id_-_userid_-_{nomb.Player}")).FirstOrDefault();
                        if (PlayerName != null) 
                            DatosDic.Add($"Nombre_{PlayerName.UserId}", 
                                $"{PlayerName.Apodo} {PlayerName.Nombre} {PlayerName.Paterno}");
                    }
                    ListaInvolucrados(nomb.Player);
                    //TotalesCal(nomb.Player, 0, 0, "");
                }
            }   
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

    protected async Task UpDateWrite(string tipo) 
    {
            Console.WriteLine($"Mios Entro UPDateWrite listaScore tiene {ListaScores.Count()} y valor de calculado {Calculando}");
            Calculando = true;
            List<string> Intentados = new List<string>();
            List<string> Logrados = new List<string>();
            string indexNuevo = string.Empty;
            string indexLogrado = string.Empty;
            int freno = 0;
            if (tipo != "Hcp")
            {
                do
                {
                    Console.WriteLine($"Mios Entro Do listaScore tiene {ListaScores.Count()} y valor de calculado {Calculando}");
                    if (freno == 5) break;
                    freno++;
                    if (ListaScores.Any()) {
                        Console.WriteLine($"Mios Entro IF listaScore tiene {ListaScores.Count()}");
                    foreach (var nuevo in ListaScores)
                    {
                            Console.WriteLine($"Mios Entro Each listaScore tiene {ListaScores.Count()}");
                            //{TarjetaId}_{jugadorIdd}_{hoyoT _{UserIdLog}}
                            indexNuevo = nuevo.Key;
                        Intentados.Add(indexNuevo);
                        G520Score res = indexNuevo.Substring(indexNuevo.Length - 3, 3) == "Add" ?
                            await ScoreIServ.AddScore(nuevo.Value) :
                            await ScoreIServ.UpdateScore(nuevo.Value);

                        if (res != null)
                        {
                                Console.WriteLine($"Mios agrego o actualizo listaScore tiene {ListaScores.Count()}");
                                indexLogrado = $"{res.Tarjeta}_{res.Player}_{res.Hoyo}_{UserIdLog}";
                            Logrados.Add(indexLogrado);
                            if (indexNuevo == indexLogrado)
                            {
                                    Console.WriteLine($"Mios antes de borrar listaScore tiene {ListaScores.Count()}");
                                    TotalesCal(res.Player, res.Hoyo, res.Score, res.Id);
                                ListaScores.Remove(new KeyValuePair<string, G520Score>(indexLogrado, res));
                                    Console.WriteLine($"despues de borrar listaScore tiene {ListaScores.Count()}");
                                }
                        }
                    }
                }
                } while (ListaScores.Count() > 0);
                Console.WriteLine($"Mios salio Do listaScore tiene {ListaScores.Count()} y valor de calculado {Calculando}");
            }
            else
            {
                foreach (var nuevo in ListaHcp)
                {
                    indexNuevo = nuevo.Key;
                    Intentados.Add(indexNuevo);
                    G510Jugador res = await JugadorIServ.UpdateJugador(nuevo.Value);
                    if (res != null )
                    {
                        indexLogrado = $"{res.Tarjeta}_{res.Player}";
                        Logrados.Add(indexLogrado);
                        if (indexNuevo == indexLogrado)
                        {
                            ListaHcp.Remove(new KeyValuePair<string, G510Jugador>(indexLogrado, res));
                        }
                    }
                }
            }

            Calculando = false;
            Console.WriteLine($"Mios antes leerScore listaScore tiene {ListaScores.Count()}");
            await LeerScores();
            Console.WriteLine($"Mios despues leerscore listaScore tiene {ListaScores.Count()}");
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
        protected async void ActualizarEstado(int edo)
        {
            if (edo == 3)
                LaTarjeta.Estado = 3;
            if (LaTarjeta.Estado == 1)
                LaTarjeta.Estado = 2;
            await TarjetasIServ.UpdateTarjeta(LaTarjeta);
            if (edo == 3) NM.NavigateTo("/players/misdatos/");
        }
        
        protected void TotalesCal(string jugador, int hoyo, int newScore, string scoreId)
        {
            if (!DatosDic.ContainsKey($"Jugador_{jugador}_Hoyo_{hoyo}"))
            {
                DatosDic.Add($"Jugador_{jugador}_Hoyo_{hoyo}", newScore.ToString());
                DatosDic.Add($"Jugador_{jugador}_HoyoId_{hoyo}", scoreId);
            }
            else
            {
                DatosDic[$"Jugador_{jugador}_Hoyo_{hoyo}"] = newScore.ToString();
            }
        }


        [Inject]
        public NotificationService NS { get; set; } = new();
        public NotificationMessage ElMesage { get; set; } =
            new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = "Cuerpo",
                Detail = "Detalles ",
                Duration = 3000
            };

        public void ShowNotification(NotificationMessage message)
        {
            NS.Notify(message);
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

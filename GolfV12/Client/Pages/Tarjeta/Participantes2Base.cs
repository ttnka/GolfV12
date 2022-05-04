using GolfV12.Client.Servicios.IFaceServ;
using GolfV12.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace GolfV12.Client.Pages.Tarjeta
{
    public class Participantes2Base : ComponentBase
    {
        [Parameter]
        public string TarjetaId { get; set; }

        public G510Jugador ElJugador { get; set; } = new G510Jugador();

        [Inject]
        public IG510JugadorServ ParticipantesIServ { get; set; }
        public IEnumerable<G510Jugador> LosParticipantes { get; set; } = Enumerable.Empty<G510Jugador>();
        
        [Inject]
        public IG120PlayerServ PlayersIServ { get; set; }
        public IEnumerable<KeyValuePair<string, string>> LosJugadores { get; set; } = new List<KeyValuePair<string, string>>();
        
        public Dictionary<string, string> LosNombres { get; set; } = new Dictionary<string, string>();

        [Inject]
        public NavigationManager NM { get; set; }
        public Dictionary<string, int> Datos { get; set; } = new Dictionary<string, int>();
        
        protected async override Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(TarjetaId)) NM.NavigateTo("/tarjeta/tarjeta/");
            var autState = await AuthStateTask;
            var user = autState.User;
            if (user.Identity.IsAuthenticated) UserIdLog = user.FindFirst(c => c.Type == "sub")?.Value;

            await LeerNombres();
            await LeerParticipantes();
            
            await EscribirBitacoraUno(UserIdLog, BitaAcciones.Consultar, false,
                $"El Usuario consulto el listado de jugadores de una tarjeta");
        }

        protected List<KeyValuePair<string, string>> NamesTemp { get; set; } =
                new List<KeyValuePair<string, string>>();
        public string ElJugadorRenglon { get; set; } = string.Empty;
        protected async Task LeerNombres()
        {
            //List<KeyValuePair<string, string>> NamesTemp;
            var AllNames = await PlayersIServ.GetPlayers();
            int renglon = 1;
            foreach (var name in AllNames)
            {
                NamesTemp.Add(new KeyValuePair<string, string>(name.UserId,
                    $"{name.Nombre} {name.Apodo} {name.Paterno}"));
                if (!LosNombres.ContainsKey(name.UserId))
                {
                    LosNombres.Add(name.UserId, $"{name.Nombre} {name.Apodo} {name.Paterno}");
                    LosNombres.Add($"Renglon_{name.UserId}", renglon.ToString());
                }
                renglon++;
            }
            
            LosNombres.Add("Vacio", "No se encontro Jugador!");
            LosJugadores = NamesTemp.AsEnumerable();
        }

        public async Task LeerParticipantes()
        {
            
            LosParticipantes = await ParticipantesIServ.Filtro($"jug1_-_tarjeta_-_{TarjetaId}");
            foreach (var participante in LosParticipantes)
            {
                if (!LosNombres.ContainsKey($"Participante_{participante.Player}"))
                    LosNombres.Add($"Participante_{participante.Player}", participante.Player);
            }
            
        }
        public async Task CambiarStatus(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                G510Jugador resultado = new G510Jugador();
                resultado = (await ParticipantesIServ.Filtro($"jug1_-_id_-_{Id}")).FirstOrDefault();
                if (resultado != null)
                {
                    resultado.Status = !resultado.Status;
                    resultado = await ParticipantesIServ.UpdateJugador(resultado);
                    await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                        $"El usuario actulizo {LosNombres[resultado.Player]} de la tarjeta de juego");
                    ElMesage.Summary = "Registro ACTUALIZADO!";
                    ElMesage.Detail = "Exitosamente";
                }
                else
                {
                    ElMesage.Summary = "Registro NO Actualizado";
                    ElMesage.Detail = "No se encontro el registro";
                }
            }
            else
            {
                ElMesage.Summary = "Registro NO Actualizado";
                ElMesage.Detail = "No hay ID";
            }
        }  
        public async Task SaveJugador()
        {
            G510Jugador resultado = new G510Jugador();
            if (ElJugador.Player.Length < 10)
            {
                if (!LosNombres.ContainsKey($"Participante_{ElJugadorRenglon}"))
                {
                    ElJugador.Tarjeta = TarjetaId;
                    ElJugador.Player = ElJugadorRenglon;
                    ElJugador.Estado = 1;
                    ElJugador.Status = true;
                    
                    resultado = await ParticipantesIServ.AddJugador(ElJugador);
                    await EscribirBitacoraUno(UserIdLog, BitaAcciones.Agregar, false,
                        $"El usuario agrego un nuevo Jugador {LosNombres[resultado.Player]} a la tarjeta de juego");
                    ElMesage.Summary = "Registro AGREGADO!";
                    ElMesage.Detail = "Exitosamente";
                }
                else
                {
                    ElMesage.Summary = "Registro NO Agregado!";
                    ElMesage.Detail = "El Jugador YA Existe";
                }
            }
            else
            {
                ElJugador.Status = !ElJugador.Status;
                resultado = await ParticipantesIServ.UpdateJugador(ElJugador);
                await EscribirBitacoraUno(UserIdLog, BitaAcciones.Editar, false,
                    $"El usuario actulizo {LosNombres[resultado.Player]} de la tarjeta de juego");
                ElMesage.Summary = "Registro ACTUALIZADO!";
                ElMesage.Detail = "Exitosamente";
            }

            if (resultado != null)
            {
               await LeerParticipantes(); 
               

            //  NM.NavigateTo($"/tarjeta/participantes/{TarjetaId}");
            }
        }
        
        public NotificationMessage ElMesage { get; set; } = 
            new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Cuerpo",
                Detail = "Detalles ",
                Duration = 3000
            };

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

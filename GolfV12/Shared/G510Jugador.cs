using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G510Jugador
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Tarjeta { get; set; } = string.Empty;
        public string Player { get; set; } = string.Empty;
        public decimal Hcp { get; set; } = 0;
        // public TeamsTemp Team { get; set; } = TeamsTemp.Hole_in_One;
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;
    }
}

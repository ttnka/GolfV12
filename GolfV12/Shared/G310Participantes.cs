using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G310Participantes
    {
        public string Id { get; set; } = string.Empty;
        public string Tarjeta { get; set; } = string.Empty;
        public string Azar { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty ;
        public string J1 { get; set; } = string.Empty;
        public string J2 { get; set; } = string.Empty;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

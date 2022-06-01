using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G300Azar
    {
        public string Id { get; set; } = string.Empty;
        public string Tarjeta { get; set; } = string.Empty ;
        public string Creador { get; set; } = string.Empty;
        public string TipoAzar { get; set; } = string.Empty;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

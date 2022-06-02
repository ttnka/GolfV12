using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G390TiposAzar
    {
        public string Id { get; set; } = string.Empty;
        public string Creador { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public bool Indivual { get; set; } = true;
        public bool Publico { get; set; } = true;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

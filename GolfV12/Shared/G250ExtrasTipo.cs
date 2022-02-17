using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G250ExtrasTipo
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; } = "Titulo";
        public string Desc { get; set; }
        public int Valor { get; set; } = 1;
        public int Creador { get; set; } = 0;
        public string Grupo { get; set; } = "General";
        public bool Publico { get; set; } = true;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;

    }
}

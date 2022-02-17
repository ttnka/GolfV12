using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G280FormatoT
    {
        [Key]
        public int Id { get; set; }
        public string Clave { get; set; } = "Clave";
        public string Titulo { get; set; } = "Titulo";
        public string Desc { get; set; }
        public int Ronda { get; set; } = 1;
        public bool Individual { get; set; } = true;
        public int Integrantes { get; set; } = 2;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G180Estado
    {
        [Key]
        public int Id { get; set; }
        public int Indice { get; set; }
        public string Titulo { get; set; }
        public string Grupo { get; set; }
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

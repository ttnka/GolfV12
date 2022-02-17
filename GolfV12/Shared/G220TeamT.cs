using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G220TeamT
    {
        [Key]
        public int Id { get; set; }
        public int TeamNum { get; set; } = 1;
        public string Nombre { get; set; } = "Nombre";
        public int NumJugadores { get; set; } = 1;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G204FechaT
    {
        [Key]
        public int Id { get; set; }
        public int Torneo { get; set; } = 0;
        public int Ronda { get; set; } = 1;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;    
    }
}

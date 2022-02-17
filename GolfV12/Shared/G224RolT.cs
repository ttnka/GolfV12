using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G224RolT
    {
        [Key]
        public int Id { get; set; }
        public int Toreno { get; set; } = 1;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int Ronda { get; set; } = 1;
        public int Indice { get; set; } = 1;
        public int Team { get; set; } = 0;
        public int PlayerT { get; set; } = 1;
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;
    }
}

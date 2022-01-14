using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G194Cita
    {
        [Key]
        public int Id { get; set; }
        public DateTime FIni { get; set; }
        public DateTime FFin { get; set; }
        public string Desc { get; set; }
        public G120Player Creador { get; set; }
        public int MasterId { get; set; } = 0;
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;
    }
}

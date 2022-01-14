using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G120Player
    {
        [Key]
        public int Id { get; set; }
        public int User { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Apodo { get; set; }
        public DateTime Bday { get; set; }
        public Niveles Nivel { get; set; }
        public G110Organizacion Organizacion {get; set;}
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;
        public bool Temporal { get; set; } = false;
    }
}

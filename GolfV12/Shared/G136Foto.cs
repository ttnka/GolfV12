using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G136Foto
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; }
        public int PlayerId { get; set; }
        public string Grupo { get; set; }
        public string Foto { get; set; }
        public bool Privada { get; set; } = false;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

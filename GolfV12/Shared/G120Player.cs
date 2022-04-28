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
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Nombre { get; set; } = string.Empty;
        public string Paterno { get; set; } = string.Empty;
        public string Materno { get; set; } = string.Empty;
        public string Apodo { get; set; } = string.Empty;
        public DateTime Bday { get; set; } = DateTime.Now;
        public Niveles Nivel { get; set; }
        public int OrganizacionId {get; set;}
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;
        public bool Temporal { get; set; } = false;
    }
}

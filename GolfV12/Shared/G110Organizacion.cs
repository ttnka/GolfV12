using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G110Organizacion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Desc { get; set; }
        public string Domicilio { get; set; }
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G190Bitacora
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public BitaAcciones Accion { get; set; }
        public bool Sistema { get; set; } = false;
        public int UsuarioId { get; set; } 
        public string Desc { get; set; }
    }
}

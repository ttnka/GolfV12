using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G170Campo
    {
        [Key]
        public int Id { get; set; }
        public string Corto { get; set; }
        public string Nombre { get; set; }
        public string Desc { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;
    }
}

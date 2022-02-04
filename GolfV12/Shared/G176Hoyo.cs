using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G176Hoyo
    {
        [Key]
        public int Id { get; set; }
        public int CampoId { get; set; }
        public string Ruta { get; set; }
        public int Hoyo { get; set; }
        public int Par { get; set; }
        public int HcpHombres { get; set; }
        public int HcpMujeres { get; set; }
        public int HcpMenores { get; set; }
        public int HcpOtros { get; set; }
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

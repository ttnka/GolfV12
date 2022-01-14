using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G178Distancia
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public G172Bandera Bandera { get; set; }
        public G176Hoyo Hoyo { get; set; }
        public int Distancia { get; set; }
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

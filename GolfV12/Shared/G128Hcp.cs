using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G128Hcp
    {
        [Key]
        public int Id { get; set; }
        public string PlayerId { get; set; }
        public DateTime Fecha { get; set; }
        public int BanderaId { get; set; }
        public Decimal Hcp { get; set; }
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G240Score
    {
        [Key]
        public int Id { get; set; }
        public int Rol { get; set; } = 0;
        public string Player { get; set; }
        public int Hoyo { get; set; } = 0;
        public int Score { get; set; } = 0;
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;



    }
}

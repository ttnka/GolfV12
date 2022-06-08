using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G332LobaDet
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Loba { get; set; } = string.Empty;
        public int Hoyo { get; set; } = 0;
        public string J1 { get; set; } = string.Empty;
        public string J2 { get; set; } = string.Empty;
        public string J3 { get; set; } = string.Empty;
        public string J4 { get; set; } = string.Empty;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
    }
}

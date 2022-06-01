using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G530Bolitas
    {
        public int Renglon { get; set; } = 0;
        public string Tarjeta { get; set; } = string.Empty;
        public string Padre { get; set; } = string.Empty;
        public string Hijo { get; set; } = string.Empty;
        public int HcpPadre { get; set; } = 0;
        public int HcpHijo { get; set; } = 0;
        public int H1 { get; set; } = 0;
        public int H2 { get; set; } = 0;
        public int H3 { get; set; } = 0;
        public int H4 { get; set; } = 0;
        public int H5 { get; set; } = 0;
        public int H6 { get; set; } = 0;
        public int H7 { get; set; } = 0;
        public int H8 { get; set; } = 0;
        public int H9 { get; set; } = 0;
        public int H10 { get; set; } = 0;
        public int H11 { get; set; } = 0;
        public int H12 { get; set; } = 0;
        public int H13 { get; set; } = 0;
        public int H14 { get; set; } = 0;
        public int H15 { get; set; } = 0;
        public int H16 { get; set; } = 0;
        public int H17 { get; set; } = 0;
        public int H18 { get; set; } = 0;
        
        public int F9 
        { get
            {
                return H1 + H2 + H3 + H4 + H5 + H6 + H7 + H8 + H9 ;
            }
        } 
        
        public int B9
        {
            get
            {
                return H10 + H11 + H12 + H13 + H14 + H15 + H16 + H17 + H18;
            }
        }
        
        public int Total
        {
            get
            {
                return F9 + B9;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class TarjetaMolde
    {
        public int Renglon { get; set; } = 0;
        public string UserId { get; set; } = string.Empty;
        public int Hcp { get; set; } = 0;
        public string HcpId { get; set; } = string.Empty;
        public int H1 { get; set; } = 0;
        public string H1Id { get; set; } = string.Empty;
        public int H2 { get; set; } = 0;
        public string H2Id { get; set; } = string.Empty;
        public int H3 { get; set; } = 0;
        public string H3Id { get; set; } = string.Empty;
        public int H4 { get; set; } = 0;
        public string H4Id { get; set; } = string.Empty;
        public int H5 { get; set; } = 0;
        public string H5Id { get; set; } = string.Empty;
        public int H6 { get; set; } = 0;
        public string H6Id { get; set; } = string.Empty;
        public int H7 { get; set; } = 0;
        public string H7Id { get; set; } = string.Empty;
        public int H8 { get; set; } = 0;
        public string H8Id { get; set; } = string.Empty;
        public int H9 { get; set; } = 0;
        public string H9Id { get; set; } = string.Empty;
        public int H10 { get; set; } = 0;
        public string H10Id { get; set; } = string.Empty;
        public int H11 { get; set; } = 0;
        public string H11Id { get; set; } = string.Empty;
        public int H12 { get; set; } = 0;
        public string H12Id { get; set; } = string.Empty;
        public int H13 { get; set; } = 0;
        public string H13Id { get; set; } = string.Empty;
        public int H14 { get; set; } = 0;
        public string H14Id { get; set; } = string.Empty;
        public int H15 { get; set; } = 0;
        public string H15Id { get; set; } = string.Empty;
        public int H16 { get; set; } = 0;
        public string H16Id { get; set; } = string.Empty;
        public int H17 { get; set; } = 0;
        public string H17Id { get; set; } = string.Empty;
        public int H18 { get; set; } = 0;
        public string H18Id { get; set; } = string.Empty;
        public bool HcpB9 { get; set; } = false;
        public int F9 
        { get
            {
                return H1 + H2 + H3 + H4 + H5 + H6 + H7 + H8 + H9 ;
            }
        } 
        public int F9Hcp 
        {
            get
            {
                int sumando = -1;
                
                if (HcpB9 == false)
                    sumando = 1;
                if (Hcp % 2 == 0)
                    sumando = 0;

                return F9 - (Hcp + sumando) / 2;
            }
        }
        public int B9
        {
            get
            {
                return H10 + H11 + H12 + H13 + H14 + H15 + H16 + H17 + H18;
            }
        }
        public int B9Hcp
        {
            get
            {
                int sumando = -1;
                
                if (HcpB9 == true)
                    sumando = 1;
                if (Hcp % 2 == 0)
                    sumando = 0;

                return B9 - (Hcp + sumando) / 2;
            }
        }

        public int Total
        {
            get
            {
                return F9 + B9;
            }
        }
        public int TotalHcp
        {
            get
            {
                return F9Hcp + B9Hcp ;
            }
        }
    }
}

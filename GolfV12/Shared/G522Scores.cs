using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G522Scores
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Tarjeta { get; set; } = string.Empty;
        public CatTemp Categoria { get; set; } = new CatTemp();
        public TeamsTemp Teams { get; set; } = TeamsTemp.Zopilote;
        public string Player { get; set; } = string.Empty;
        public int Hcp { get; set; } = 0;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
        public int Renglon { get; set; } = 0;
        public bool HcpB9 { get; set; } = false;
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
        {
            get
            {
                return H1 + H2 + H3 + H4 + H5 + H6 + H7 + H8 + H9;
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
                return F9Hcp + B9Hcp;
            }
        }


    }
}

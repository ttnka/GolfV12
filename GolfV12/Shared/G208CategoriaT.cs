using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G208CategoriaT
    {
        [Key]
        public int Id { get; set; }
        public int Torneo { get; set; } = 0;
        public string Titulo { get; set; } = "Categoria";
        public string Desc { get; set; }
        public int Bandera { get; set; } = 0;
        public int NumJugadores { get; set; } = 4;
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;



    }
}

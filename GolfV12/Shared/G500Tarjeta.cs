using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G500Tarjeta
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int Campo { get; set; } = 0;
        public string Titulo { get; set; } = string.Empty;
        public Torneo2Edit Captura  { get; set; }
        public TorneoView Consulta { get; set; }
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;


    }
}

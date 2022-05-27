using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G502Tarjetas
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Creador { get; set; } = Guid.NewGuid().ToString();
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int Campo { get; set; } = 0;
        public string Titulo { get; set; } = string.Empty;
        public Torneo2Edit Captura { get; set; } = Torneo2Edit.Jugadores;
        public TorneoView Consulta { get; set; } = TorneoView.Todos;
        public int Estado { get; set; } = 1;
        public bool Status { get; set; } = true;
        
        //public virtual ICollection<G522Scores> Scores { get; set; } 
        //public virtual G522Scores UnScore { get; set; } 
        
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G200Torneo
    {
        [Key]
        public int Id { get; set; }
        public int Ejercicio { get; set; }
        public string Titulo { get; set; } = "Nuevo";
        public string Desc { get; set; }
        public int Creador { get; set; } = 0;
        public int Formato { get; set; } = 0; 
        public int Rondas { get; set; } = 1;
        public int Campo { get; set; } = 0;
        public TorneoView TorneoV { get; set; } = TorneoView.Todos;
        public TorneoEdit TorneoE { get; set; } = TorneoEdit.Jugadores;
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;



    }
}

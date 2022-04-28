﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfV12.Shared
{
    public class G520Score
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Campo { get; set; } = 0;
        public string Player { get; set; } = string.Empty;
        public decimal Hcp { get; set; } = 0;
        public bool Publico { get; set; } = true;
        public int Hoyo { get; set; } = 0;
        public int Score { get; set; } = 1;

    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Raspisanie.Models
{
    public class Levels
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

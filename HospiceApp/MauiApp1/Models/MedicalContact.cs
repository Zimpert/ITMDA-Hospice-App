﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class MedicalContact : Contact
    {
        public string Speciality { get; set; }
        public string Hospital { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace JiCpu.Modelos
{
    public class Mainboard
    {
        public required string Marca { get; set; }
        public string? Modelo { get; set; }
        public string? BiosModel {get; set;}
        public int Port { get; set; }
        public string? Socket { get; set; }
        public string? Chipset { get; set; }
        public string? Bus { get; set; }
        public int POD_slots {get; set;}

    }
}

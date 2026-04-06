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
        public int Port { get; set; }
        public string? Socket { get; set; }
        public string? Chipset { get; set; }
        public string? Bus { get; set; }

    }
}

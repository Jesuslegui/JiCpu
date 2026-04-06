using System;
using System.Collections.Generic;
using System.Text;

namespace JiCpu.Modelos
{
    public class Ram
    {
        public string? Marca { get; set; }
        public double CapacidadGB { get; set; }
        public int FrecuenciaMHz { get; set; }
        public string? Slot { get; set; }
        public string? TipoMemoria { get; set; }
        public string? Fabricante { get; set; }
        public string? NumeroParte { get; set; }
    }

    public class RamInfo
    {
        public string? Tipo { get; set; }
        public string? TotalGB { get; set; }
        public double CapacidadTotal { get; set; }
        public string? Canal { get; set; }
        public string? FrecuenciaControlador { get; set; }
        public string? FrecuenciaDRAM { get; set; }
        public List<Ram>? Modulos { get; set; }

        // Timings
        public string? CL { get; set; }
        public string? TRCD { get; set; }
        public string? TRP { get; set; }
        public string? TRAS { get; set; }
        public string? TRC { get; set; }
        public string? CommandRate { get; set; }
        public string? FSBRAM { get; set; }
    }
}
using JiCpu.Modelos;
using System.Management;
using System.Linq;
using System.Collections.Generic;

namespace Services
{
    public class RamService
    {
        public List<Ram> ObtenerModulos()
        {
            var lista = new List<Ram>();
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

            foreach (var obj in searcher.Get())
            {
                lista.Add(new Ram
                {
                    Marca = obj["Manufacturer"]?.ToString(),
                    CapacidadGB = Convert.ToDouble(obj["Capacity"]) / (1024 * 1024 * 1024),
                    FrecuenciaMHz = Convert.ToInt32(obj["Speed"]),
                    Slot = obj["DeviceLocator"]?.ToString(),
                    TipoMemoria = ObtenerTipoMemoria(obj),
                    Fabricante = obj["Manufacturer"]?.ToString(),
                    NumeroParte = obj["PartNumber"]?.ToString()
                });
            }

            return lista;
        }

        // Cambiado a ManagementBaseObject para evitar casteos y errores de conversión
        private string ObtenerTipoMemoria(ManagementBaseObject obj)
        {
            if (obj["SMBIOSMemoryType"] != null)
            {
                return obj["SMBIOSMemoryType"].ToString() switch
                {
                    "26" => "DDR4",
                    "27" => "DDR5",
                    "24" => "DDR3",
                    "22" => "DDR2",
                    _ => "Desconocido"
                };
            }
            return "No disponible";
        }

        public RamInfo ObtenerInfoCompleta()
        {
            var modulos = ObtenerModulos();
            if (!modulos.Any()) return null;

            var info = new RamInfo
            {
                Tipo = modulos.First().TipoMemoria,
                TotalGB = modulos.Sum(x => x.CapacidadGB).ToString("0") + " GB",
                CapacidadTotal = modulos.Sum(x => x.CapacidadGB),
                Canal = ObtenerCanal(modulos),
                FrecuenciaControlador = ObtenerFrecuenciaControlador(modulos),
                FrecuenciaDRAM = ObtenerFrecuenciaExacta(modulos),
                Modulos = modulos,
                CL = "16 clocks",
                TRCD = "20 clocks",
                TRP = "20 clocks",
                TRAS = "39 clocks",
                TRC = "74 clocks",
                CommandRate = "1T",
                FSBRAM = "1:16"
            };

            return info;
        }

        private string ObtenerFrecuenciaExacta(List<Ram> modulos)
        {
            return modulos.Any()
                ? modulos.First().FrecuenciaMHz.ToString("0.0") + " MHz"
                : "0 MHz";
        }

        private string ObtenerFrecuenciaControlador(List<Ram> modulos)
        {
            return modulos.Any()
                ? (modulos.First().FrecuenciaMHz / 2.0).ToString("0.0") + " MHz"
                : "0 MHz";
        }

        private string ObtenerCanal(List<Ram> modulos)
        {
            if (modulos.Count == 1) return "Single Channel";

            var slots = modulos.Select(m => m.Slot).ToList();
            bool tieneCanalA = slots.Any(s => s.Contains("A") || s.Contains("1"));
            bool tieneCanalB = slots.Any(s => s.Contains("B") || s.Contains("2"));

            return (tieneCanalA && tieneCanalB)
                ? $"{modulos.Count} x 64-bit (Dual Channel)"
                : $"{modulos.Count} x 64-bit";
        }
    }
}
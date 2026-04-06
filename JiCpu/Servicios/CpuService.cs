using System.Diagnostics.CodeAnalysis;
using System.Management;

namespace JiCpu.Servicios
{
    public class CpuService
    {
        public (string name, string cores, string threads, string speed) ObtenerCPU()
        {
            var searcher = new ManagementObjectSearcher("select * from Win32_Processor");

            foreach (var obj in searcher.Get())
            {
                string? name = obj["Name"]?.ToString();
                string? cores = obj["NumberOfCores"]?.ToString();
                string? threads = obj["NumberOfLogicalProcessors"]?.ToString();

                double speedMHz = Convert.ToDouble(obj["MaxClockSpeed"]);
                string speed = (speedMHz / 1000).ToString("0.00") + " GHz";

                return (name, cores, threads, speed);
            }

            return ("N/A", "0", "0", "0");
        }
    }
}
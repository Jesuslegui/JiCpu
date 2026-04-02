using System.Management;

namespace JiCpu.Servicios
{
    public class RamService
    {
        public string ObtenerRamTotal()
        {
            double total = 0;

            var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");

            foreach (var obj in searcher.Get())
            {
                total += Convert.ToDouble(obj["Capacity"]);
            }

            double gb = total / (1024 * 1024 * 1024);

            return gb.ToString("0.00") + " GB";
        }
    }
}
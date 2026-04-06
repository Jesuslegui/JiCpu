using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Text;
using JiCpu.Modelos;

namespace JiCpu.Servicios
{
    public class GraphicsService
    {
        public List<Modelos.Graphics> ObtenerGPU()
        {
            var lista = new List<Modelos.Graphics>();
            var searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (var obj in searcher.Get())
            {
                // Seguridad al leer AdapterRAM y VideoMemoryType
                double memoryGb = 0;
                try
                {
                    var adapterRamObj = obj["AdapterRAM"];
                    if (adapterRamObj != null)
                    {
                        // Leer AdapterRAM de forma robusta para evitar overflow de 32 bits
                        ulong memoryBytes = 0;

                        if (adapterRamObj is ulong u)
                            memoryBytes = u;
                        else if (adapterRamObj is long l)
                            memoryBytes = (ulong)l;
                        else if (adapterRamObj is uint ui)
                            memoryBytes = ui;
                        else if (adapterRamObj is int i)
                            memoryBytes = (ulong)i;
                        else
                        {
                            // Intentar parsear desde string si es necesario
                            if (!ulong.TryParse(adapterRamObj.ToString(), out memoryBytes))
                            {
                                if (double.TryParse(adapterRamObj.ToString(), out var db))
                                    memoryBytes = (ulong)db;
                            }
                        }

                        memoryGb = Math.Round(memoryBytes / (1024.0 * 1024.0 * 1024.0));
                    }
                }
                catch
                {
                    memoryGb = 0;
                }

                int memoryType = -1;
                try
                {
                    if (obj["VideoMemoryType"] != null)
                        memoryType = Convert.ToInt32(obj["VideoMemoryType"]);
                }
                catch
                {
                    memoryType = -1;
                }

                lista.Add(new Modelos.Graphics
                {
                    Name = obj["Name"]?.ToString(),
                    model = obj["VideoProcessor"]?.ToString(),
                    Memory = memoryGb,             // si es 0, UI mostrará 8 GB por defecto
                    MemoryBus = memoryType,
                    temperature = ObtenerTemperatura()
                });
            }
            return lista;
        }

        private string ObtenerTemperatura()
        {
            Computer computer = new Computer
            {
                IsGpuEnabled = true
            };
            computer.Open();

            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd)
                {
                    hardware.Update();
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU"))
                        {
                            if (sensor.Value.HasValue)
                                return Convert.ToInt32(Math.Round(sensor.Value.Value)).ToString() + " °C";
                        }
                    }
                }
            }

            return "No disponible";
        }
    }
}
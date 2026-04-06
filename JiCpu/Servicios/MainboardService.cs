using JiCpu.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace JiCpu.Servicios
{
    public class MainboardService
    {
        // Obtiene información de la placa base usando WMI
        public List<Mainboard> ObtenerInfo()
        {
            var lista = new List<Mainboard>();

            // Contar slots disponibles (puertos/ranuras)
            int slotCount = 0;
            try
            {
                var slotSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_SystemSlot");
                foreach (var s in slotSearcher.Get()) slotCount++;
            }
            catch
            {
                slotCount = 0;
            }

            // Obtener socket del/los procesadores (si existe)
            string socket = null;
            try
            {
                var procSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject p in procSearcher.Get())
                {
                    socket = p["SocketDesignation"]?.ToString();
                    if (!string.IsNullOrEmpty(socket)) break;
                }
            }
            catch
            {
                socket = null;
            }

            // Intentar obtener información adicional de Win32_MotherboardDevice
            string chipset = null;
            string bus = null;
            try
            {
                var mbDevSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_MotherboardDevice");
                foreach (ManagementObject mb in mbDevSearcher.Get())
                {
                    // Description o Manufacturer pueden contener datos útiles
                    chipset = mb["Description"]?.ToString() ?? mb["Manufacturer"]?.ToString();

                    if (mb["PrimaryBusType"] != null && string.IsNullOrEmpty(bus))
                    {
                        try
                        {
                            int pb = Convert.ToInt32(mb["PrimaryBusType"]);
                            bus = pb switch
                            {
                                5 => "PCI",
                                6 => "PCI-X",
                                9 => "PCI Express",
                                _ => "BusType-" + pb
                            };
                        }
                        catch { }
                    }

                    if (!string.IsNullOrEmpty(chipset) || !string.IsNullOrEmpty(bus)) break;
                }
            }
            catch
            {
                chipset = null;
                bus = null;
            }

            // Obtener la/s placas base registradas
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject obj in searcher.Get())
            {
                var marca = obj["Manufacturer"]?.ToString() ?? "Desconocido";
                var modelo = obj["Product"]?.ToString() ?? obj["Version"]?.ToString();
                var version = obj["Version"]?.ToString();

                lista.Add(new Mainboard
                {
                    Marca = marca,
                    Modelo = string.IsNullOrEmpty(modelo) ? version : modelo,
                    Port = slotCount,
                    Socket = socket,
                    Chipset = chipset ?? version ?? "Desconocido",
                    Bus = bus ?? "Desconocido"
                });
            }

            return lista;
        }
    }
}

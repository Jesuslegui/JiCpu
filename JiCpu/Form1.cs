using System;
using System.Management;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;
using System.Linq;
using System.Drawing;
using JiCpu.Servicios;

namespace JiCpu
{
    public partial class Form1 : Form
    {
        private Computer computer;

        // 🔥 SERVICIOS
        private readonly Services.RamService _ramService;
        private readonly MainboardService _boardService;
        private readonly GraphicsService _gpuService;

        public Form1()
        {
            InitializeComponent();

            _ramService = new Services.RamService();
            _boardService = new MainboardService();
            _gpuService = new GraphicsService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarCPU();
            CargarRAM();
            CargarMainboard();
            CargarGPU();
           

            computer = new Computer()
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true
            };

            computer.Open();
            timer1.Start();
        }

        // 🎮 GPU
        private void CargarGPU()
        {
            try
            {
                var lista = _gpuService.ObtenerGPU();
                var gpu = lista.FirstOrDefault();
                if (gpu == null) return;

                lblGpuNameValue.Text = gpu.Name ?? "Desconocido";
                // Mostrar VRAM redondeada; si el valor no está disponible, mostrar 8 GB por defecto
                lblGpuMemoryValue.Text = gpu.Memory > 0 ? Math.Round(gpu.Memory).ToString() + " GB" : "8 GB";
                lblGpuDriverValue.Text = gpu.model ?? "Desconocido";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error GPU: " + ex.Message);
            }
        }

        // 🔥 MAINBOARD
        private void CargarMainboard()
        {
            try
            {
                var lista = _boardService.ObtenerInfo();
                var mb = lista.FirstOrDefault();
                if (mb == null) return;

                lblBoardMarcaValue.Text = mb.Marca ?? "Desconocido";
                lblBoardModeloValue.Text = mb.Modelo ?? "Desconocido";
                lblBoardPortsValue.Text = mb.Port > 0 ? mb.Port.ToString() : "Desconocido";
                lblBoardSocketValue.Text = mb.Socket ?? "Desconocido";
                lblBoardChipsetValue.Text = mb.Chipset ?? "Desconocido";
                lblBoardBusValue.Text = mb.Bus ?? "Desconocido";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Mainboard: " + ex.Message);
            }
        }

        // 🔥 CPU (por ahora sigue aquí, luego lo pasas a service)
        private void CargarCPU()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_Processor");

                foreach (var obj in searcher.Get())
                {
                    lblNameValue.Text = obj["Name"]?.ToString();
                    lblCoresValue.Text = obj["NumberOfCores"]?.ToString();
                    lblThreadsValue.Text = obj["NumberOfLogicalProcessors"]?.ToString();

                    double speedMHz = Convert.ToDouble(obj["MaxClockSpeed"]);
                    lblSpeedValue.Text = (speedMHz / 1000).ToString("0.00") + " GHz";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error CPU: " + ex.Message);
            }
        }

        // 🔥 RAM
        private void CargarRAM()
        {
            var info = _ramService.ObtenerInfoCompleta();
            if (info == null) return;

            // Limpiar controles dinámicos previos (excepto lblRamValue)
            var dynamicControls = tabRAM.Controls.Cast<Control>()
                .Where(c => c != lblRamValue)
                .ToList();
            foreach (var c in dynamicControls)
                tabRAM.Controls.Remove(c);

            // Info general arriba
            lblRamValue.Text = $"{info.TotalGB} | {info.Canal} | {info.FrecuenciaDRAM} | {info.Tipo}";
            lblRamValue.Location = new Point(20, 20);
            lblRamValue.ForeColor = Color.DarkBlue;

            int y = 60; // inicio para módulos

            foreach (var ram in info.Modulos)
            {
                // Crear GroupBox por módulo
                GroupBox gb = new GroupBox
                {
                    Text = ram.Slot,
                    Location = new Point(20, y),
                    Size = new Size(730, 60),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.Black
                };

                // Label con info de cada módulo
                Label lbl = new Label
                {
                    Text = $"Marca: {ram.Marca,-12}  Capacidad: {ram.CapacidadGB,4:0} GB  " +
                           $"Frecuencia: {ram.FrecuenciaMHz,4} MHz  Tipo: {ram.TipoMemoria,-4}",
                    Location = new Point(10, 25),
                    AutoSize = true,
                    Font = new Font("Consolas", 9, FontStyle.Regular),
                    ForeColor = Color.DarkBlue
                };

                gb.Controls.Add(lbl);
                tabRAM.Controls.Add(gb);

                y += 70; // espacio entre módulos
            }

            // Info del controlador y timings
            Label lblCtrl = new Label
            {
                Text = $"Frecuencia controlador: {info.FrecuenciaControlador} | CL: {info.CL} | TRCD: {info.TRCD} | TRP: {info.TRP} | TRAS: {info.TRAS} | CommandRate: {info.CommandRate}",
                Location = new Point(20, y + 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkRed
            };
            tabRAM.Controls.Add(lblCtrl);
        }

        // 🔥 TEMPERATURA
        private void timer1_Tick(object sender, EventArgs e)
        {
            float? cpuTemp = null;

            foreach (var hardware in computer.Hardware)
            {
                hardware.Update();

                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Value.HasValue)
                        {
                            float temp = sensor.Value.Value;
                            string name = sensor.Name.ToLower();

                            if (temp > 0 && temp < 120)
                            {
                                if (name.Contains("tdie") || name.Contains("tctl") || name.Contains("package"))
                                {
                                    cpuTemp = temp;
                                    break;
                                }

                                if (!cpuTemp.HasValue)
                                    cpuTemp = temp;
                            }
                        }
                    }
                }
            }

            lblTempValue.Text = cpuTemp.HasValue
                ? cpuTemp.Value.ToString("0") + " °C"
                : "N/A";

            // Actualizar temperatura de GPU en tiempo real
            string gpuTempText = "N/A";
            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.GpuAmd || hardware.HardwareType == HardwareType.GpuNvidia)
                {
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Value.HasValue)
                        {
                            // Preferir sensores que contengan "GPU" en el nombre
                            if (sensor.Name.Contains("GPU") || string.IsNullOrEmpty(gpuTempText) || gpuTempText == "N/A")
                            {
                                gpuTempText = Convert.ToInt32(Math.Round(sensor.Value.Value)).ToString() + " °C";
                                break;
                            }
                        }
                    }
                }
                if (gpuTempText != "N/A") break;
            }

            lblGpuTempValue.Text = gpuTempText;
        }
    }
}
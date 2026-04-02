using System;
using System.Management;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;

namespace JiCpu
{
    public partial class Form1 : Form
    {
        private Computer computer;

        // 🔥 SERVICIOS
        private readonly Services.RamService _ramService;

        public Form1()
        {
            InitializeComponent();

            _ramService = new Services.RamService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarCPU();
            CargarRAM();

            computer = new Computer()
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true
            };

            computer.Open();
            timer1.Start();
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
        }
    }
}
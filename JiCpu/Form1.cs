using System;
using System.Management;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;

namespace JiCpu
{
    public partial class Form1 : Form
    {
        private Computer computer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ForeColor = System.Drawing.Color.White;

            CargarCPU();

            computer = new Computer()
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true
            };

            computer.Open();
            timer1.Start();
        }

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
                MessageBox.Show(ex.Message);
            }
        }

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
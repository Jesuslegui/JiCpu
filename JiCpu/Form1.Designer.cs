using Timer = System.Windows.Forms.Timer;

namespace JiCpu
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private TabControl tabControl;
        private TabPage tabCPU;
        private TabPage tabGPU;
        private TabPage tabRAM;
        private TabPage tabBoard;

        private GroupBox grpCPU;

        private Label lblNameValue;
        private Label lblCoresValue;
        private Label lblThreadsValue;
        private Label lblSpeedValue;
        private Label lblTempValue;

        // 🔥 RAM
        private Label lblRamValue;

        private Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            timer1 = new Timer(components);

            tabControl = new TabControl();
            tabCPU = new TabPage();
            tabGPU = new TabPage();
            tabRAM = new TabPage();
            tabBoard = new TabPage();

            grpCPU = new GroupBox();

            lblNameValue = new Label();
            lblCoresValue = new Label();
            lblThreadsValue = new Label();
            lblSpeedValue = new Label();
            lblTempValue = new Label();
            lblRamValue = new Label();

            SuspendLayout();

            // FORM
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.Font = new Font("Segoe UI", 9);

            // TAB CONTROL
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(780, 430);

            tabCPU.Text = "CPU";
            tabGPU.Text = "Graphics";
            tabRAM.Text = "Memory";
            tabBoard.Text = "Mainboard";

            tabControl.TabPages.Add(tabCPU);
            tabControl.TabPages.Add(tabGPU);
            tabControl.TabPages.Add(tabRAM);
            tabControl.TabPages.Add(tabBoard);

            // GROUP CPU
            grpCPU.Text = "Processor";
            grpCPU.Location = new Point(10, 10);
            grpCPU.Size = new Size(740, 200);

            AddLabel(grpCPU, "Name:", 20, 40);
            AddLabel(grpCPU, "Cores:", 20, 70);
            AddLabel(grpCPU, "Threads:", 20, 100);
            AddLabel(grpCPU, "Speed:", 20, 130);
            AddLabel(grpCPU, "Temperature:", 20, 160);

            SetupValueLabel(lblNameValue, 150, 40);
            SetupValueLabel(lblCoresValue, 150, 70);
            SetupValueLabel(lblThreadsValue, 150, 100);
            SetupValueLabel(lblSpeedValue, 150, 130);
            SetupValueLabel(lblTempValue, 150, 160);

            grpCPU.Controls.Add(lblNameValue);
            grpCPU.Controls.Add(lblCoresValue);
            grpCPU.Controls.Add(lblThreadsValue);
            grpCPU.Controls.Add(lblSpeedValue);
            grpCPU.Controls.Add(lblTempValue);

            tabCPU.Controls.Add(grpCPU);

            // 🔥 RAM TAB REAL
            AddLabel(tabRAM, "Total RAM:", 20, 40);

            lblRamValue.Location = new Point(150, 40);
            lblRamValue.AutoSize = true;
            lblRamValue.ForeColor = Color.DarkBlue;

            tabRAM.Controls.Add(lblRamValue);

            // PLACEHOLDERS
            AddLabel(tabGPU, "GPU info próximamente...", 20, 40);
            AddLabel(tabBoard, "Board info próximamente...", 20, 40);

            // TIMER
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;

            // FORM FINAL
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl);
            Text = "JiCpu";
            Load += Form1_Load;

            ResumeLayout(false);
        }

        private void AddLabel(Control parent, string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.ForeColor = Color.Black;
            parent.Controls.Add(lbl);
        }

        private void SetupValueLabel(Label lbl, int x, int y)
        {
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.ForeColor = Color.DarkBlue;
        }
    }
}
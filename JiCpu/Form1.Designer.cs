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
        private GroupBox grpBoard;

        private Label lblNameValue;
        private Label lblCoresValue;
        private Label lblThreadsValue;
        private Label lblSpeedValue;
        private Label lblTempValue;

        // 🔥 RAM
        private Label lblRamValue;
        private Label lblBoardMarcaValue;
        private Label lblBoardModeloValue;
        private Label lblBoardPortsValue;
        private Label lblBoardSocketValue;
        private Label lblBoardChipsetValue;
        private Label lblBoardBusValue;

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

            // MAINBOARD GROUP
            grpBoard = new GroupBox();
            grpBoard.Text = "Mainboard";
            grpBoard.Location = new Point(10, 10);
            grpBoard.Size = new Size(740, 220);

            AddLabel(grpBoard, "Manufacturer:", 20, 40);
            AddLabel(grpBoard, "Model:", 20, 70);
            AddLabel(grpBoard, "Slots:", 20, 100);
            AddLabel(grpBoard, "Socket:", 20, 130);
            AddLabel(grpBoard, "Chipset:", 20, 160);
            AddLabel(grpBoard, "Bus:", 20, 190);

            lblBoardMarcaValue = new Label();
            lblBoardModeloValue = new Label();
            lblBoardPortsValue = new Label();
            lblBoardSocketValue = new Label();
            lblBoardChipsetValue = new Label();
            lblBoardBusValue = new Label();

            SetupValueLabel(lblBoardMarcaValue, 150, 40);
            SetupValueLabel(lblBoardModeloValue, 150, 70);
            SetupValueLabel(lblBoardPortsValue, 150, 100);
            SetupValueLabel(lblBoardSocketValue, 150, 130);
            SetupValueLabel(lblBoardChipsetValue, 150, 160);
            SetupValueLabel(lblBoardBusValue, 150, 190);

            grpBoard.Controls.Add(lblBoardMarcaValue);
            grpBoard.Controls.Add(lblBoardModeloValue);
            grpBoard.Controls.Add(lblBoardPortsValue);
            grpBoard.Controls.Add(lblBoardSocketValue);
            grpBoard.Controls.Add(lblBoardChipsetValue);
            grpBoard.Controls.Add(lblBoardBusValue);

            tabBoard.Controls.Add(grpBoard);

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
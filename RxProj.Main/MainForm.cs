using Modern.Forms;
using RxProj.Core;
using System.Globalization;

namespace RxProj.Main
{
    public sealed class MainForm : Form
    {
        private readonly Menu m_MenuBar = new Menu();
        private readonly MenuItem m_File = new MenuItem("File");
        private readonly MenuItem m_Tools = new MenuItem("Tools");

        private void InitMenuBar()
        {
            m_MenuBar.Dock = DockStyle.Top;
            m_MenuBar.Items.Add(m_File);
            m_MenuBar.Items.Add(m_Tools);
            Controls.Add(m_MenuBar);
        }

        private TabControl m_Tabs = new TabControl();
        private TabPage m_TabCascade = new TabPage("Cascade");
        private TabPage m_TabDynamicRange = new TabPage("Dynamic Range");

        private void InitTabMenu()
        {
            m_Tabs.AutoSize = true;
            m_Tabs.Dock = DockStyle.Fill;
            m_Tabs.TabPages.Add(m_TabCascade);
            m_Tabs.TabPages.Add(m_TabDynamicRange);
            Controls.Add(m_Tabs);

            m_TabCascade.AutoSize = true;
            m_TabCascade.AutoScroll = true;
            m_TabCascade.Dock = DockStyle.Fill;
        }

        private TableLayoutPanel m_Input = new TableLayoutPanel();
        private TextBox m_InputPower = new TextBox();
        private TextBox m_InputNoiseBand = new TextBox();
        private TextBox m_InputImpedance = new TextBox();
        private TextBox m_NodeCount = new TextBox();

        private void InitCascadeInput()
        {
            m_Input.AutoSize = true;
            m_Input.Dock = DockStyle.Top;
            m_TabCascade.Controls.Add(m_Input);

            m_InputPower.Text = "0";
            m_InputPower.Dock = DockStyle.Top;
            m_InputPower.KeyPress += TextUtil.NumericTextBox_KeyPress;
            m_Input.Controls.Add(new Label { Text = "Power", Dock = DockStyle.Top }, 0, 0);
            m_Input.Controls.Add(new Label { Text = "dBm", Dock = DockStyle.Top }, 2, 0);
            m_Input.Controls.Add(m_InputPower, 1, 0);

            m_InputNoiseBand.Text = "1000";
            m_InputNoiseBand.Dock = DockStyle.Top;
            m_InputNoiseBand.KeyPress += TextUtil.NumericTextBox_KeyPress;
            m_Input.Controls.Add(new Label { Text = "Noise Band", Dock = DockStyle.Top }, 0, 1);
            m_Input.Controls.Add(new Label { Text = "Hz", Dock = DockStyle.Top }, 2, 1);
            m_Input.Controls.Add(m_InputNoiseBand, 1, 1);

            m_InputImpedance.Text = "50";
            m_InputImpedance.Dock = DockStyle.Top;
            m_InputImpedance.KeyPress += TextUtil.NumericTextBox_KeyPress;
            m_Input.Controls.Add(new Label { Text = "Impedance", Dock = DockStyle.Top }, 0, 2);
            m_Input.Controls.Add(new Label { Text = "Ohms", Dock = DockStyle.Top }, 2, 2);
            m_Input.Controls.Add(m_InputImpedance, 1, 2);
            
            m_NodeCount.Text = "1";
            m_NodeCount.Dock = DockStyle.Top;
            m_NodeCount.KeyPress += TextUtil.NumericTextBox_KeyPress;
            m_Input.Controls.Add(new Label { Text = "Nodes", Dock = DockStyle.Top }, 0, 3);
            m_Input.Controls.Add(m_NodeCount, 1, 3);

            Button apply = new Button();
            apply.Dock = DockStyle.Fill;
            apply.Text = "Apply";
            apply.Click += Apply_Click;
            m_Input.Controls.Add(apply, 1, 4);

            m_Input.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.0f));
            m_Input.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80.0f));
            m_Input.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.0f));
        }

        private RxCascade m_Cascade = new RxCascade();
        private Panel m_NodesHack = new Panel();
        private TableLayoutPanel m_Nodes = new TableLayoutPanel();

        private void InitCascadeNodes()
        {
            m_Nodes.AutoSize = true;
            m_Nodes.AutoScroll = false;
            m_Nodes.Dock = DockStyle.Top;
            m_TabCascade.Controls.Add(m_Nodes);
        }

        private void Apply_Click(object? sender, System.EventArgs e)
        {
            if(double.TryParse(m_InputPower.Text, out double power)) {
                m_InputPower.Text = power.ToString(CultureInfo.InvariantCulture);
                m_Cascade.InputPower = power;
            }

            if(double.TryParse(m_InputNoiseBand.Text, out double band)) {
                band = System.Math.Abs(band);
                m_InputNoiseBand.Text = band.ToString(CultureInfo.InvariantCulture);
                m_Cascade.InputNoiseBand = band;
            }

            if(double.TryParse(m_InputImpedance.Text, out double impedance)) {
                impedance = System.Math.Max(impedance, 0.0);
                m_InputImpedance.Text = impedance.ToString(CultureInfo.InvariantCulture);
                m_Cascade.InputImpedance = impedance;
            }

            if(int.TryParse(m_NodeCount.Text, out int count)) {
                count = System.Math.Max(count, 1);
                m_NodeCount.Text = count.ToString(CultureInfo.InvariantCulture);
                m_Nodes.Controls.Clear();
                m_Cascade.Nodes.Clear();

                for(int i = 0; i < count; ++i) {
                    RxNode node = new RxNode();
                    NodeControl control = new NodeControl(node);
                    m_Cascade.Nodes.Add(node);
                    m_Nodes.Controls.Add(control, 0, i);
                }
            }
        }

        public MainForm()
        {
            //MinimumSize = DefaultSize;
            //MaximumSize = DefaultSize;
            UseSystemDecorations = true;

            InitTabMenu();
            InitMenuBar();
            InitCascadeNodes();
            InitCascadeInput();
        }
    }
}

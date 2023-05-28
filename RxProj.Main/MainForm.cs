using Modern.Forms;
using RxProj.Core;

namespace RxProj.Main
{
    public sealed class MainForm : Form
    {
        private RxCascade Cascade = new RxCascade();

        private Menu TabMenu = new Menu();
        private MenuItem MenuFile = new MenuItem("File");
        private MenuItem MenuTools = new MenuItem("Tools");

        private TabControl Tabs = new TabControl();
        private TabPage CascadeTab = new TabPage("Cascade");
        private TabPage RangeTab = new TabPage("Dynamic Range");

        private TableLayoutPanel CascadeInput = new TableLayoutPanel();
        private FieldControl CascadeInputPower = new FieldControl();
        private FieldControl CascadeNoiseBandwidth = new FieldControl();

        public MainForm()
        {
            MinimumSize = DefaultSize;
            MaximumSize = DefaultSize;
            UseSystemDecorations = true;

            Tabs.Dock = DockStyle.Fill;
            Tabs.TabPages.Add(CascadeTab);
            Tabs.TabPages.Add(RangeTab);
            Controls.Add(Tabs);

            TabMenu.Dock = DockStyle.Top;
            TabMenu.Items.Add(MenuFile);
            TabMenu.Items.Add(MenuTools);
            Controls.Add(TabMenu);
        
            MenuFile.Items.Add(new MenuItem("Exit")).Click += Exit_Click;

            CascadeInputPower.Height = 32;
            CascadeInputPower.Dock = DockStyle.Top;
            CascadeInputPower.Prefix = "Input power";
            CascadeInputPower.Postfix = "dBm";
            CascadeInput.Controls.Add(CascadeInputPower);

            CascadeNoiseBandwidth.Height = 32;
            CascadeNoiseBandwidth.Dock = DockStyle.Top;
            CascadeNoiseBandwidth.Prefix = "Noise bandwidth";
            CascadeNoiseBandwidth.Postfix = "Hz";
            CascadeInput.Controls.Add(CascadeNoiseBandwidth);

            CascadeInput.AutoSize = true;
            CascadeInput.Dock = DockStyle.Fill;
            CascadeTab.Controls.Add(CascadeInput);
            System.Console.WriteLine(CascadeInput.Width);
            System.Console.WriteLine(CascadeTab.Width);

            CascadeTab.Controls.Add(new Splitter());

            CascadeTab.Controls.Add(new Button { Text = "Click me!", AutoSize = true, Dock = DockStyle.Left });


        }

        private void Exit_Click(object? sender, MouseEventArgs e)
        {
            Application.Exit();
        }
    }
}

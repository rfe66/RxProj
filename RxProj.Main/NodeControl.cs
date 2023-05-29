using Modern.Forms;
using RxProj.Core;

namespace RxProj.Main
{
    public sealed class NodeControl : TableLayoutPanel
    {
        public readonly RxNode Node;

        public readonly TextBox InputPowerGain = new TextBox();
        public readonly TextBox InputNoiseFigure = new TextBox();
        public readonly TextBox InputVoltage = new TextBox();
        public readonly TextBox InputCurrent = new TextBox();
        public readonly TextBox InputIP1dB = new TextBox();
        public readonly TextBox InputOP1dB = new TextBox();
        public readonly TextBox InputIIP3 = new TextBox();
        public readonly TextBox InputOIP3 = new TextBox();

        public NodeControl(RxNode node)
        {
            Node = node;

            AutoSize = true;
            //AutoScroll = true;
            Dock = DockStyle.Fill;

            InputPowerGain.Text = "0";
            InputPowerGain.Dock = DockStyle.Top;
            InputPowerGain.KeyPress += TextUtil.NumericTextBox_KeyPress;
            Controls.Add(new Label { Text = "Power Gain", Dock = DockStyle.Top }, 0, 0);
            Controls.Add(new Label { Text = "dB", Dock = DockStyle.Top }, 2, 0);
            Controls.Add(InputPowerGain, 1, 0);

            InputNoiseFigure.Text = "0";
            InputNoiseFigure.Dock = DockStyle.Top;
            InputPowerGain.KeyPress += TextUtil.NumericTextBox_KeyPress;
            Controls.Add(new Label { Text = "Noise Figure", Dock = DockStyle.Top }, 0, 1);
            Controls.Add(new Label { Text = "dB", Dock = DockStyle.Top }, 2, 1);
            Controls.Add(InputNoiseFigure, 1, 1);

            InputVoltage.Text = "0.001";
            InputVoltage.Dock = DockStyle.Top;
            InputPowerGain.KeyPress += TextUtil.NumericTextBox_KeyPress;
            Controls.Add(new Label { Text = "Voltage", Dock = DockStyle.Top }, 0, 2);
            Controls.Add(new Label { Text = "V", Dock = DockStyle.Top }, 2, 2);
            Controls.Add(InputVoltage, 1, 2);

            InputCurrent.Text = "0.001";
            InputCurrent.Dock = DockStyle.Top;
            InputPowerGain.KeyPress += TextUtil.NumericTextBox_KeyPress;
            Controls.Add(new Label { Text = "Current", Dock = DockStyle.Top }, 0, 3);
            Controls.Add(new Label { Text = "A", Dock = DockStyle.Top }, 2, 3);
            Controls.Add(InputCurrent, 1, 3);

            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.0f));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80.0f));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.0f));
            RowStyles.Add(new RowStyle());
        }

        public void Apply()
        {

        }
    }
}

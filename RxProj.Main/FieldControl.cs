using Modern.Forms;

namespace RxProj.Main
{
    public class FieldControl : TableLayoutPanel
    {
        private readonly Label m_Prefix = new Label();
        private readonly Label m_Postfix = new Label();
        private readonly TextBox m_TextBox = new TextBox();

        public string Prefix {
            get => m_Prefix.Text;
            set => m_Prefix.Text = value;
        }

        public string Postfix {
            get => m_Postfix.Text;
            set => m_Postfix.Text = value;
        }

        public new string Text {
            get => m_TextBox.Text;
            set => m_TextBox.Text = value;
        }

        public FieldControl() : base()
        {
            Padding margin;

            margin = m_Prefix.Margin;
            margin.Top = 8;
            m_Prefix.Margin = margin;
            m_Prefix.Dock = DockStyle.Fill;
            m_Prefix.AutoSize = true;

            margin = m_TextBox.Margin;
            margin.Top = 8;
            m_TextBox.Margin = margin;
            m_TextBox.Dock = DockStyle.Fill;
            m_TextBox.AutoSize = true;

            margin = m_Postfix.Margin;
            margin.Top = 8;
            m_Postfix.Margin = margin;
            m_Postfix.Dock = DockStyle.Fill;
            m_Postfix.AutoSize = true;

            AutoSize = true;
            ColumnCount = 3;
            RowCount = 1;

            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.0f));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80.0f));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.0f));
            RowStyles.Add(new RowStyle());

            Controls.Add(m_Prefix, 0, 0);
            Controls.Add(m_TextBox, 1, 0);
            Controls.Add(m_Postfix, 2, 0);
        }
    }
}

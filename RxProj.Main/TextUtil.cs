using Modern.Forms;

namespace RxProj.Main
{
    public static class TextUtil
    {
        public static void NumericTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '.') {
                e.Handled = true;
                e.KeyChar = '\0';
            }
        }
    }
}

namespace RxProj.Core
{
    public sealed class RxNode
    {
        public double PowerGain = 0.0;
        public double NoiseFigure = 0.0;
        public double Voltage = 24.0;
        public double Current = 0.01;
        public double OP1dB = double.NaN;
        public double IP1dB = double.NaN;
        public double OIP3 = double.NaN;
        public double IIP3 = double.NaN;

        public double C_PowerGain = 0.0;
        public double C_NoiseFigure = 0.0;
        public double C_InputPower = 0.0;
        public double C_OutputPower = 0.0;
        public double C_SupplyPower = 0.0;
        public double C_OP1dB = 0.0;
        public double C_IP1dB = 0.0;
        public double C_OIP3 = 0.0;
        public double C_IIP3 = 0.0;

        public void SetOP1dB(double value)
        {
            OP1dB = value;
            IP1dB = value - PowerGain + 1.0;
        }

        public void SetIP1dB(double value)
        {
            OP1dB = value + PowerGain - 1.0;
            IP1dB = value;
        }

        public void SetOIP3(double value)
        {
            OIP3 = value;
            IIP3 = value - PowerGain;
        }

        public void SetIIP3(double value)
        {
            OIP3 = value + PowerGain;
            IIP3 = value;
        }
    }
}

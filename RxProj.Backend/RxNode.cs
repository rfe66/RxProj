namespace RxProj.Backend
{
    public sealed class RxNode
    {
        public double Gain { get; set; }
        public double IIP3 { get; set; }
        public double OIP3 { get; set; }
        public double IP1dB { get; set; }
        public double OP1dB { get; set; }
        public double NoiseFigure { get; set; }

        public double C_Gain { get; set; }
        public double C_IIP3 { get; set; }
        public double C_OIP3 { get; set; }
        public double C_IP1dB { get; set; }
        public double C_OP1dB { get; set; }
        public double C_NoiseFigure { get; set; }
        public double C_InputPower { get; set; }
        public double C_OutputPower { get; set; }
        public double C_OutputPowerBackoff { get; set; }
        public double C_OutputPowerBackoffPeak { get; set; }

        public RxNode()
        {
            Gain = Double.NaN;
            IIP3 = Double.NaN;
            OIP3 = Double.NaN;
            IP1dB = Double.NaN;
            OP1dB = Double.NaN;
            NoiseFigure = Double.NaN;

            C_Gain = Double.NaN;
            C_IIP3 = Double.NaN;
            C_OIP3 = Double.NaN;
            C_IP1dB = Double.NaN;
            C_OP1dB = Double.NaN;
            C_NoiseFigure = Double.NaN;
            C_InputPower = Double.NaN;
            C_OutputPower = Double.NaN;
            C_OutputPowerBackoff = Double.NaN;
            C_OutputPowerBackoffPeak = Double.NaN;
        }

        public void SetIIP3(double value)
        {
            IIP3 = value;
            OIP3 = IIP3 + Gain;
        }

        public void SetOIP3(double value)
        {
            OIP3 = value;
            IIP3 = OIP3 - Gain;
        }

        public void SetIP1dB(double value)
        {
            IP1dB = value;
            OP1dB = IP1dB + (Gain - 1.0);
        }

        public void SetOP1dB(double value)
        {
            OP1dB = value;
            IP1dB = OP1dB - (Gain - 1.0);
        }
    }
}

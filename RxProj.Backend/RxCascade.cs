namespace RxProj.Backend
{
    public sealed class RxCascade
    {
        public readonly List<RxNode> Nodes;
        public double InputFrequency { get; set; }
        public double InputPower { get; set; }

        public double Gain { get; set; }
        public double IIP3 { get; set; }
        public double OIP3 { get; set; }
        public double IP1dB { get; set; }
        public double OP1dB { get; set; }
        public double NoiseFigure { get; set; }
        public double OutputPower { get; set; }

        public RxCascade()
        {
            Nodes = new List<RxNode>();
            InputFrequency = Double.NaN;
            InputPower = Double.NaN;

            Gain = Double.NaN;
            IIP3 = Double.NaN;
            OIP3 = Double.NaN;
            IP1dB = Double.NaN;
            OP1dB = Double.NaN;
            NoiseFigure = Double.NaN;
        }

        public void Update()
        {
            double accum;
            bool is_first;

            Gain = 0.0;
            IIP3 = 0.0;
            OIP3 = 0.0;
            IP1dB = 0.0;
            OP1dB = 0.0;
            NoiseFigure = 0.0;

            foreach(RxNode node in Nodes) {
                Gain += node.Gain;
                node.C_Gain = Gain;
            }

            accum = 1.0;
            is_first = true;
            foreach(RxNode node in Nodes) {
                if(is_first) {
                    NoiseFigure += RxMath.GetTimesPower(node.NoiseFigure);
                    is_first = false;
                }
                else {
                    // https://en.wikipedia.org/wiki/Noise_figure#Noise_factor_of_cascaded_devices
                    NoiseFigure += (RxMath.GetTimesPower(node.NoiseFigure) - 1.0) / accum;
                }

                accum *= RxMath.GetTimesPower(node.Gain);
                node.C_NoiseFigure = RxMath.GetDecibelsPower(NoiseFigure);
            }

            NoiseFigure = RxMath.GetDecibelsPower(NoiseFigure);

            is_first = true;
            foreach(RxNode node in Nodes) {
                if(is_first) {
                    OIP3 = RxMath.GetTimesPower(node.OIP3);
                    is_first = false;
                }
                else {
                    double xval = 0.0;
                    xval += 1.0 / OIP3 / RxMath.GetTimesPower(node.Gain);
                    xval += 1.0 / RxMath.GetTimesPower(node.OIP3);
                    OIP3 = 1.0 / xval;
                }

                node.C_OIP3 = RxMath.GetDecibelsPower(OIP3);
                node.C_IIP3 = node.C_OIP3 - node.C_Gain;
            }

            OIP3 = RxMath.GetDecibelsPower(OIP3);
            IIP3 = OIP3 - Gain;

            accum = 0.0;
            is_first = true;
            foreach(RxNode node in Nodes) {
                if(is_first) {
                    node.C_InputPower = InputPower;
                    is_first = false;
                }
                else {
                    node.C_InputPower = accum;
                }

                accum = node.C_InputPower + node.Gain;
                node.C_OutputPower = accum;
            }

            OutputPower = InputPower + Gain;

            is_first = true;
            foreach(RxNode node in Nodes) {
                if(is_first) {
                    OP1dB = RxMath.GetTimesPower(node.OP1dB);
                    is_first = false;
                }
                else {
                    double xval = 0.0;
                    xval += 1.0 / OP1dB / RxMath.GetTimesPower(node.Gain);
                    xval += 1.0 / node.OP1dB;
                    OP1dB = 1.0 / xval;
                }

                node.C_OP1dB = RxMath.GetDecibelsPower(OP1dB);
                node.C_IP1dB = node.C_OP1dB - (node.Gain - 1.0);
                node.C_OutputPowerBackoff = node.OP1dB - node.C_OutputPower;
                node.C_OutputPowerBackoffPeak = node.C_OutputPowerBackoff;
            }

            OP1dB = RxMath.GetDecibelsPower(OP1dB);
            IP1dB = OP1dB - (Gain - 1.0);
        }
    }
}

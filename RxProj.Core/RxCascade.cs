using System.Collections.Generic;

namespace RxProj.Core
{
    public sealed class RxCascade
    {
        public readonly List<RxNode> Nodes = new List<RxNode>();
        public double InputPower = 0.0;
        public double InputImpedance = 50.0;
        public double InputNoiseBand = 1000.0;

        public double PowerGain = 0.0;
        public double NoiseFigure = 0.0;
        public double OutputPower = 0.0;
        public double SupplyPower = 0.0;
        public double OP1dB = double.NaN;
        public double IP1dB = double.NaN;
        public double OIP3 = double.NaN;
        public double IIP3 = double.NaN;

        public void Solve()
        {
            double accum;

            OIP3 = double.NaN;
            IIP3 = double.NaN;

            //
            // CASCADED POWER GAIN
            //

            PowerGain = 0.0;
            for(int i = 0; i < Nodes.Count; ++i) {
                PowerGain += Nodes[i].PowerGain;
                Nodes[i].C_PowerGain = PowerGain;
            }

            //
            // CASCADED NOISE FIGURE
            //

            accum = 1.0;
            NoiseFigure = 0.0;
            for(int i = 0; i < Nodes.Count; ++i) {
                if(i == 0)
                    NoiseFigure += (RxUtil.PowerTimes(Nodes[i].NoiseFigure));
                else
                    NoiseFigure += (RxUtil.PowerTimes(Nodes[i].NoiseFigure) - 1.0) / accum;
                Nodes[i].C_NoiseFigure = RxUtil.PowerDecibels(NoiseFigure);
                accum *= RxUtil.PowerTimes(Nodes[i].PowerGain);
            }

            NoiseFigure = RxUtil.PowerDecibels(NoiseFigure);

            //
            // CASCADED OUTPUT POWER
            //

            accum = 0.0;
            OutputPower = 0.0;
            for(int i = 0; i < Nodes.Count; ++i) {
                if(i == 0)
                    Nodes[i].C_InputPower = InputPower;
                else
                    Nodes[i].C_InputPower = accum;
                accum = Nodes[i].C_InputPower + Nodes[i].PowerGain;
                Nodes[i].C_OutputPower = accum;
            }

            OutputPower = InputPower + PowerGain;

            //
            // CASCADED SUPPLY POWER
            //

            SupplyPower = 0.0;
            for(int i = 0; i < Nodes.Count; ++i) {
                SupplyPower += RxUtil.PowerDecibels(Nodes[i].Voltage * Nodes[i].Current * 1000.0);
                Nodes[i].C_SupplyPower = SupplyPower;
            }

            //
            // CASCADED OP1dB/IP1dB
            //

            OP1dB = double.NaN;
            IP1dB = double.NaN;
            for(int i = 0; i < Nodes.Count; ++i) {
                if(i == 0)
                    OP1dB = RxUtil.PowerTimes(Nodes[i].OP1dB);
                else
                    OP1dB = 1.0 / ((1.0 / OP1dB / RxUtil.PowerTimes(Nodes[i].PowerGain)) + (1.0 / RxUtil.PowerTimes(Nodes[i].OP1dB)));
                Nodes[i].SetOP1dB(RxUtil.PowerDecibels(OP1dB));
            }

            OP1dB = RxUtil.PowerDecibels(OP1dB);
            IP1dB = OP1dB - PowerGain + 1.0;

            //
            // CASCADED OIP3/IIP3
            //

            OIP3 = double.NaN;
            IIP3 = double.NaN;
            for(int i = 0; i < Nodes.Count; ++i) {
                if(i == 0)
                    OIP3 = RxUtil.PowerTimes(Nodes[i].OIP3);
                else
                    OIP3 = 1.0 / ((1.0 / OIP3 / RxUtil.PowerTimes(Nodes[i].PowerGain)) + (1.0 / RxUtil.PowerTimes(Nodes[i].OIP3)));
                Nodes[i].SetIIP3(RxUtil.PowerDecibels(OIP3));
            }

            OIP3 = RxUtil.PowerDecibels(OIP3);
            IIP3 = OIP3 - PowerGain;
        }
    }
}

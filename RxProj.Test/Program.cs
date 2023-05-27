using RxProj.Backend;

namespace RxProj.Test
{
    public static class Program
    {
        public static void Main()
        {
            RxNode node;
            RxCascade cascade = new RxCascade();

            node = new RxNode();
            node.Gain = 15.0;
            node.SetIIP3(85.0);
            node.SetIP1dB(0.0);
            node.NoiseFigure = 2.0;
            cascade.Nodes.Add(node);

            node = new RxNode();
            node.Gain = -8.0;
            node.SetIIP3(108.0);
            node.SetIP1dB(13.0);
            node.NoiseFigure = 8.0;
            cascade.Nodes.Add(node);

            node = new RxNode();
            node.Gain = 15.0;
            node.SetIIP3(85.0);
            node.SetIP1dB(5.0);
            node.NoiseFigure = 5.0;
            cascade.Nodes.Add(node);

            cascade.InputPower = 0.0;

            cascade.Update();

            Console.WriteLine("P = {0}", cascade.OutputPower);
            Console.WriteLine("OP1dB = {0}", cascade.OP1dB);
            Console.WriteLine("IP1dB = {0}", cascade.IP1dB);
            Console.WriteLine("G = {0}", cascade.Gain);
            Console.WriteLine("N = {0}", cascade.NoiseFigure);
            Console.WriteLine("OIP3 = {0}", cascade.OIP3);
            Console.WriteLine("IIP3 = {0}", cascade.IIP3);
        }
    }
}

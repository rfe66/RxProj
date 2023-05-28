namespace RxProj.Core
{
    public static class RxUtil
    {
        public const double Boltzmann = 1.380649e-23;

        public static double PowerDecibels(double times)
        {
            return 10.0 * Math.Log10(times);
        }

        public static double PowerTimes(double decibels)
        {
            return Math.Pow(10.0, decibels / 10.0);
        }

        public static double VoltageDecibels(double times)
        {
            return 20.0 * Math.Log10(times);
        }

        public static double VoltageTimes(double decibels)
        {
            return Math.Pow(10.0, decibels / 20.0);
        }
    }
}

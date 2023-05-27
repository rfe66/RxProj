namespace RxProj.Backend
{
    public static class RxMath
    {
        public static double GetDecibelsPower(double times)
        {
            return 10.0 * Math.Log10(times);
        }

        public static double GetDecibelsVoltage(double times)
        {
            return 20.0 * Math.Log10(times);
        }

        public static double GetTimesPower(double decibels)
        {
            return Math.Pow(10.0, decibels / 10.0);
        }

        public static double GetTimesVoltage(double decibels)
        {
            return Math.Pow(10, decibels / 20.0);
        }
    }
}

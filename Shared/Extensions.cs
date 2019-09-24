namespace Zebble
{
    using OxyPlot;

    public static class Extensions
    {
        public static OxyColor ToOxyColor(this Color color)
        {
            if (color != null)
                return OxyColor.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
            return new OxyColor();
        }
    }
}
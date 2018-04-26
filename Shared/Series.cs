namespace Zebble
{
    using System.Collections.Generic;

    public class Series
    {
        public PlotTypes PlotType { get; set; }

        public PlotConfig Config { get; set; }

        public List<PlotData> Data { get; set; }
    }
}

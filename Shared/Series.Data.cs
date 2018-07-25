namespace Zebble
{
    public class PlotData
    {
        public PlotData() { }

        public PlotData(double x, double y) { X = x; Y = y; }

        public PlotData(string label, double value)
        {
            Label = label;
            Value = value;
        }

        public PlotData(double value, int categoryIndex = -1)
        {
            Value = value;
            CategoryIndex = categoryIndex;
        }

        public PlotData(double x, double lowerWhisker, double boxBottom, double median, double boxTop, double upperWhisker)
        {
            X = x;
            LowerWhisker = LowerWhisker;
            BoxBottom = boxBottom;
            Median = median;
            BoxTop = boxTop;
            UpperWhisker = upperWhisker;
        }

        public PlotData(double x0, double y0, double x1, double y1)
        {
            X0 = x0; Y0 = y0;
            X1 = x1; Y1 = y1;
        }

        public PlotData(double x, double high, double low, double open = 0, double close = 0)
        {
            X = x;
            High = high;
            Low = low;
            Open = open;
            Close = close;
        }

        public PlotData(double start, double end, string title = null)
        {
            Start = start;
            End = end;
            Title = title;
        }

        public PlotData(double x, double y, double size = 0, double value = 0, object tag = null)
        {
            X = x; Y = y;
            Size = size;
            Value = value;
            Tag = tag;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public string Label { get; set; }
        public double Value { get; set; }
        public Color FillColor { get; set; }

        public int CategoryIndex { get; set; } = 1;

        public double UpperWhisker { get; set; }
        public double LowerWhisker { get; set; }
        public double Median { get; set; }
        public double BoxTop { get; set; }
        public double BoxBottom { get; set; }

        public double X0 { get; set; }
        public double X1 { get; set; }
        public double Y0 { get; set; }
        public double Y1 { get; set; }

        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }

        public string Title { get; set; }
        public double Start { get; set; }
        public double End { get; set; }

        public double Size { get; set; }
        public object Tag { get; set; }
    }
}

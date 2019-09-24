namespace Zebble
{
    using OxyPlot.Series;

    public class PlotConfig
    {
        public PlotConfig() { }

        public PlotConfig(Color color, Color color2, double limit)
        {
            Color = color;
            Color2 = color2;
            Limit = limit;
        }

        public PlotConfig(double[,] data, double[] columnCoordinates, double[] rowCoordinates)
        {
            Data = data;
            ColumnCoordinates = columnCoordinates;
            RowCoordinates = rowCoordinates;
        }

        public PlotConfig(double x0, double y0, double x1, double y1, string title, bool interpolate, HeatMapRenderMethod renderMethod)
        {
            X0 = x0; Y0 = y0;
            X1 = x1; Y1 = y1;
            Title = title;
            Interpolate = interpolate;
            RenderMethod = renderMethod;
        }

        public bool IsLabelsEnabled { get; set; } = true;

        public Color Color { get; set; }
        public Color Color2 { get; set; }
        public double Limit { get; set; }

        public double[,] Data { get; set; }
        public double[] ColumnCoordinates { get; set; }
        public double[] RowCoordinates { get; set; }

        public string Title { get; set; }
        public bool Interpolate { get; set; }
        public HeatMapRenderMethod RenderMethod { get; set; }
        public double X0 { get; set; }
        public double X1 { get; set; }
        public double Y0 { get; set; }
        public double Y1 { get; set; }
    }
}
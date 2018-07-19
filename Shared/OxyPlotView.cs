namespace Zebble
{
    using OxyPlot;
    using OxyPlot.Series;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OxyPlotView : View, IRenderedBy<OxyPlotViewRenderer>
    {
        internal PlotModel OxyplotModel { get; set; }

        public string Title { get; set; }

        public List<Series> Series { get; set; }

        public List<OxyPlot.Axes.Axis> Axes { get; set; }

        public async Task InitializePlot()
        {
            try
            {
                OxyplotModel = new PlotModel { Title = Title };

                if (Series == null && Axes == null)
                {
                    Device.Log.Error("Series or Axes of plot is null");
                    return;
                }

                foreach (var plot in Series)
                {
                    switch (plot.PlotType)
                    {
                        case PlotTypes.TwoColorArea:
                            await RenderTwoColorArea(plot);
                            break;
                        case PlotTypes.Area:
                            await RenderArea(plot);
                            break;
                        case PlotTypes.TwoColorLine:
                            await RenderTwoColorLine(plot);
                            break;
                        case PlotTypes.Line:
                            await RenderLine(plot);
                            break;
                        case PlotTypes.Pie:
                            await RenderPie(plot);
                            break;
                        case PlotTypes.Bar:
                            await RenderBar(plot);
                            break;
                        case PlotTypes.ErrorColumn:
                            await RenderErrorColumn(plot);
                            break;
                        case PlotTypes.Column:
                            await RenderColumn(plot);
                            break;
                        case PlotTypes.Box:
                            await RenderBox(plot);
                            break;
                        case PlotTypes.Contour:
                            await RenderContour(plot);
                            break;
                        case PlotTypes.RectangleBar:
                            await RenderRectangleBar(plot);
                            break;
                        case PlotTypes.CandleStick:
                            await RenderCandleStick(plot);
                            break;
                        case PlotTypes.HeatMap:
                            await RenderHeatMap(plot);
                            break;
                        case PlotTypes.HighLow:
                            await RenderHighLow(plot);
                            break;
                        case PlotTypes.IntervalBar:
                            await RenderIntervalBar(plot);
                            break;
                        case PlotTypes.Scatter:
                            await RenderScatter(plot);
                            break;
                        default: break;
                    }

                    if (Axes != null)
                        foreach (var axis in Axes) OxyplotModel.Axes.Add(axis);

                    await Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                Device.Log.Error(ex);
            }
        }

        Task RenderTwoColorArea(Series plot)
        {
            var twoColorAreaSeries = new TwoColorAreaSeries
            {
                Color = plot.Config.Color.ToOxyColor(),
                Color2 = plot.Config.Color2.ToOxyColor(),
                Limit = plot.Config.Limit
            };

            foreach (var point in plot.Data) twoColorAreaSeries.Points.Add(new DataPoint(point.X, point.Y));
            OxyplotModel.Series.Add(twoColorAreaSeries);

            return Task.CompletedTask;
        }

        Task RenderArea(Series plot)
        {
            var areaSeries = new AreaSeries();
            foreach (var point in plot.Data) areaSeries.Points.Add(new DataPoint(point.X, point.Y));
            OxyplotModel.Series.Add(areaSeries);

            return Task.CompletedTask;
        }

        Task RenderTwoColorLine(Series plot)
        {
            var twoColorLineSeries = new TwoColorLineSeries
            {
                Color = plot.Config.Color.ToOxyColor(),
                Color2 = plot.Config.Color2.ToOxyColor(),
                Limit = plot.Config.Limit
            };

            foreach (var point in plot.Data) twoColorLineSeries.Points.Add(new DataPoint(point.X, point.Y));
            OxyplotModel.Series.Add(twoColorLineSeries);

            return Task.CompletedTask;
        }

        Task RenderLine(Series plot)
        {
            var lineSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            foreach (var point in plot.Data) lineSeries.Points.Add(new DataPoint(point.X, point.Y));
            OxyplotModel.Series.Add(lineSeries);

            return Task.CompletedTask;
        }

        Task RenderPie(Series plot)
        {
            var pieSeries = new PieSeries();
            foreach (var slice in plot.Data) pieSeries.Slices.Add(new PieSlice(slice.Label, slice.Value) { Fill = slice.FillColor.ToOxyColor() });
            OxyplotModel.Series.Add(pieSeries);

            return Task.CompletedTask;
        }

        Task RenderBar(Series plot)
        {
            var barSeries = new BarSeries();
            foreach (var item in plot.Data) barSeries.Items.Add(new BarItem(item.Value, item.CategoryIndex));
            OxyplotModel.Series.Add(barSeries);

            return Task.CompletedTask;
        }

        Task RenderErrorColumn(Series plot)
        {
            var errorColumn = new ErrorColumnSeries();
            foreach (var item in plot.Data) errorColumn.Items.Add(new ColumnItem(item.Value, item.CategoryIndex));
            OxyplotModel.Series.Add(errorColumn);

            return Task.CompletedTask;
        }

        Task RenderColumn(Series plot)
        {
            var barSeries = new ColumnSeries();
            foreach (var item in plot.Data) barSeries.Items.Add(new ColumnItem(item.Value, item.CategoryIndex));
            OxyplotModel.Series.Add(barSeries);

            return Task.CompletedTask;
        }

        Task RenderBox(Series plot)
        {
            var boxSeries = new BoxPlotSeries();
            foreach (var item in plot.Data) boxSeries.Items.Add(new BoxPlotItem(item.X, item.LowerWhisker, item.BoxBottom, item.Median, item.BoxTop, item.UpperWhisker));
            OxyplotModel.Series.Add(boxSeries);

            return Task.CompletedTask;
        }

        Task RenderContour(Series plot)
        {
            var contourSeries = new ContourSeries
            {
                Data = plot.Config.Data,
                ColumnCoordinates = plot.Config.ColumnCoordinates,
                RowCoordinates = plot.Config.RowCoordinates
            };
            OxyplotModel.Series.Add(contourSeries);

            return Task.CompletedTask;
        }

        Task RenderRectangleBar(Series plot)
        {
            var rectangleBarSeries = new RectangleBarSeries { Title = plot.Config.Title };

            foreach (var item in plot.Data) rectangleBarSeries.Items.Add(new RectangleBarItem(item.X0, item.Y0, item.X1, item.Y1));
            OxyplotModel.Series.Add(rectangleBarSeries);

            return Task.CompletedTask;
        }

        Task RenderCandleStick(Series plot)
        {
            var candleStickSeries = new CandleStickSeries();
            foreach (var item in plot.Data) candleStickSeries.Items.Add(new HighLowItem(item.X, item.High, item.Low, item.Open, item.Close));
            OxyplotModel.Series.Add(candleStickSeries);

            return Task.CompletedTask;
        }

        Task RenderHeatMap(Series plot)
        {
            var heatMapSeries = new HeatMapSeries
            {
                Data = plot.Config.Data,
                X0 = plot.Config.X0,
                X1 = plot.Config.X1,
                Y0 = plot.Config.Y0,
                Y1 = plot.Config.Y1,
                Interpolate = plot.Config.Interpolate,
                RenderMethod = plot.Config.RenderMethod
            };
            OxyplotModel.Series.Add(heatMapSeries);

            return Task.CompletedTask;
        }

        Task RenderHighLow(Series plot)
        {
            var highLowSeries = new HighLowSeries();
            foreach (var item in plot.Data) highLowSeries.Items.Add(new HighLowItem(item.X, item.High, item.Low, item.Open, item.Close));
            OxyplotModel.Series.Add(highLowSeries);

            return Task.CompletedTask;
        }

        Task RenderIntervalBar(Series plot)
        {
            var intervalBarSeries = new IntervalBarSeries();
            foreach (var item in plot.Data) intervalBarSeries.Items.Add(new IntervalBarItem(item.Start, item.End, item.Title));
            OxyplotModel.Series.Add(intervalBarSeries);

            return Task.CompletedTask;
        }

        Task RenderScatter(Series plot)
        {
            var scatterSeries = new ScatterSeries();
            foreach (var item in plot.Data) scatterSeries.Points.Add(new ScatterPoint(item.X, item.Y, item.Size, item.Value, item.Tag));
            OxyplotModel.Series.Add(scatterSeries);

            return Task.CompletedTask;
        }
    }
}

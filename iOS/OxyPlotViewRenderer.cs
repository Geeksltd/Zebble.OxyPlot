namespace Zebble
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using OxyPlot.Xamarin.iOS;
    using System;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OxyPlotViewRenderer : INativeRenderer
    {
        OxyPlotView View;
        PlotView Result;

        public async Task<UIKit.UIView> Render(Renderer renderer)
        {
            View = (OxyPlotView)renderer.View;
            await View.InitializePlot();
            return Result = new PlotView { Model = View.Model, BackgroundColor = renderer.View.BackgroundColor.Render() };
        }

        public void Dispose()
        {
            try { Result?.Dispose(); }
            catch { }
        }
    }
}
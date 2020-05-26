namespace Zebble
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using OxyPlot.Xamarin.iOS;
    using System;
    using System.ComponentModel.Design.Serialization;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OxyPlotViewRenderer : INativeRenderer
    {
        OxyPlotView View;
        PlotView Result;

        public async Task<UIKit.UIView> Render(Renderer renderer)
        {
            View = (OxyPlotView)renderer.View;
            await View.InitializePlot();
            return Result = new PlotView { 
                Model = View.Model
            };
        }

        public void Dispose()
        {
            try { Result?.Dispose(); }
            catch { }
        }
    }
}
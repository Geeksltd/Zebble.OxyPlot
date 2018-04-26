namespace Zebble
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using OxyPlot.Xamarin.iOS;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OxyPlotViewRenderer : INativeRenderer
    {
        OxyPlotView View;
        PlotView Result;

        public async Task<UIKit.UIView> Render(Renderer renderer)
        {
            View = (OxyPlotView)renderer.View;
            Result = new PlotView
            {
                Model = View.OxyplotModel,
            };

            return Result;
        }

        public void Dispose() { }
    }
}
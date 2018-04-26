namespace Zebble
{
    using OxyPlot.Windows;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OxyPlotViewRenderer : INativeRenderer
    {
        OxyPlotView View;
        PlotView Result;

        public async Task<FrameworkElement> Render(Renderer renderer)
        {
            View = (OxyPlotView)renderer.View;
            Result = new PlotView
            {
                Model = View.OxyplotModel
            };

            return Result;
        }

        public void Dispose() { }
    }
}

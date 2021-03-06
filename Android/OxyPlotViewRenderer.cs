namespace Zebble
{
    using OxyPlot.Xamarin.Android;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Android.Runtime;
    using Olive;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Preserve]
    public class OxyPlotViewRenderer : INativeRenderer
    {
        OxyPlotView View;

        static int NextId;

        [Preserve]
        public OxyPlotViewRenderer() { }

        public async Task<Android.Views.View> Render(Renderer renderer)
        {
            try
            {
                View = (OxyPlotView)renderer.View;
                await View.InitializePlot();
                return new PlotView(Renderer.Context) { Model = View.Model };
            }
            catch (Exception ex)
            {
                Log.For(this).Error(ex);
                return null;
            }
        }

        int FindFreeId()
        {
            NextId++;
            while (UIRuntime.CurrentActivity.FindViewById(NextId) != null) NextId++;
            return NextId;
        }

        public void Dispose() { }
    }
}
namespace Zebble
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using OxyPlot.Xamarin.Android;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OxyPlotViewRenderer : INativeRenderer
    {
        OxyPlotView View;
        PlotView Result;

        static int NextId;

        public async Task<Android.Views.View> Render(Renderer renderer)
        {
            try
            {
                View = (OxyPlotView)renderer.View;
                await View.InitializePlot();
                Result = new PlotView(Renderer.Context)
                {
                    Model = View.OxyplotModel,
                };

                return Result;
            }
            catch (Exception ex)
            {
                Device.Log.Error(ex.Message);
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
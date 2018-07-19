namespace Zebble
{
    using OxyPlot.Windows;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using System;
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Series;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OxyPlotViewRenderer : INativeRenderer
    {
        OxyPlotView View;
        PlotView Result;

        public async Task<FrameworkElement> Render(Renderer renderer)
        {
            try
            {
                View = (OxyPlotView)renderer.View;
                await View.InitializePlot();
                var webView = new WebView { Html = GetHtml(View.OxyplotModel) };
                var native = await webView.Render();
                return native.Native();
            }
            catch (Exception ex)
            {
                Device.Log.Error(ex.Message);
                return null;
            }

        }

        string GetHtml(PlotModel model)
        {
            var data = string.Empty;
            var colors = string.Empty;
            var labels = string.Empty;
            var rng = new Random();
            foreach (var serie in model.Series)
            {
                if (serie is PieSeries)
                {
                    var items = (serie as PieSeries).Slices;
                    var total = items.Sum(barItem => barItem.Value);

                    data = items.Aggregate(data, (current, item) => current + ((int)(item.Value / total * 100) + ","));
                    foreach (var slice in items)
                    {
                        labels += $"'{slice.Label}',";
                        colors += $"'rgba({slice.Fill.R}, {slice.Fill.G}, {slice.Fill.B}, {slice.Fill.A / (float)byte.MaxValue})',";
                    }
                }
                else
                    throw new NotImplementedException("The renderer for other charts has not been implemented yet");
            }
            var html = $@"
<html>
<head>
<title>{model.Title}</title>
<script src='http://www.chartjs.org/dist/2.7.2/Chart.bundle.js'></script>
<script src='http://www.chartjs.org/samples/latest/utils.js'></script>
<link rel='stylesheet' href='https://use.fontawesome.com/releases/v5.0.10/css/all.css' integrity='sha384-+d0P83n9kaQMCwj8F4RJB66tzIwOKmrdb46+porD/OvrJ+37WqIM7UoBtwHO6Nlg' crossorigin='anonymous'>
</head>
<body>
<div id='canvas-holder' style='width:{100}%'>
    <canvas id='chart-area'></canvas>
    </div>
    <script>
        var config =
        {{
            type:'pie',
            data:
            {{
                datasets:[
                {{
                    data:[{data}],
                    backgroundColor:[{colors}],label:'{model.Title}'
                }}],
                labels: [{labels}]
            }},
            options: 
            {{
                animation: false,
    	        legend: {{display: false}},
                rotation: (-0.3 * Math.PI),
                elements: 
                {{
                    arc: 
                    {{
                        borderWidth: 0
                    }}
                }}
            }}
        }};
        window.onload = function()
        {{
            var ctx =document.getElementById('chart-area').getContext('2d');
            window.myPie=new Chart(ctx, config);
        }};
    </script>
</body>
</html>";
            return html;
        }

        public void Dispose() { }
    }
}

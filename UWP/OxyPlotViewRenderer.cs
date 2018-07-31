namespace Zebble
{
    using Windows.Storage.Streams;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
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
        Grid GridView = new Grid();
        Windows.UI.Xaml.Controls.WebView RenderedWebView;

        public async Task<FrameworkElement> Render(Renderer renderer)
        {
            try
            {
                View = (OxyPlotView)renderer.View;
                await View.InitializePlot();

                var result = new Windows.UI.Xaml.Controls.WebView();
                result.NavigateToString(GetHtml(View.OxyplotModel));
                RenderedWebView = result;

                result.LoadCompleted -= RenderedWebView_LoadCompleted;
                result.LoadCompleted += RenderedWebView_LoadCompleted;

                GridView.Children.Add(result);

                return GridView;
            }
            catch (Exception ex)
            {
                Device.Log.Error(ex.Message);
                return null;
            }

        }

        private async void RenderedWebView_LoadCompleted(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var base64Content = await RenderedWebView.InvokeScriptAsync("eval", new[] { "createBase64Image()" });

            if (base64Content == "" || base64Content == "data:,")
            {
                await RenderedWebView.InvokeScriptAsync("eval", new[] { "hideImageHolder()" });
                return;
            }

            var imageView = new ImageView { ImageData = Base64Image.Parse(base64Content).FileContents };
            var native = (await imageView.Render()).Native();
            GridView.Children.RemoveAt(0);
            GridView.Children.Add(native);
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
<div id='image-holder'>
    <img style='width:{100}%' id='chartImage' />
</div>
    <script>
        var config =
        {{
            type:'pie',
            data:
            {{
                datasets:[
                {{
                    data:[{data.Substring(0, data.Length - 1)}],
                    backgroundColor:[{colors.Substring(0, colors.Length - 1)}],label:'{model.Title}'
                }}],
                labels: [{labels.Substring(0, labels.Length - 1)}]
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
        window.onerror = function (msg, url, lineNo, columnNo, error) 
        {{       
            return false;
        }}
        window.onload = function()
        {{
            var ctx =document.getElementById('chart-area').getContext('2d');
            window.myPie=new Chart(ctx, config);
        }};

        function createBase64Image(){{
            var url_base64 = document.getElementById('chart-area').toDataURL('image/png');
            document.getElementById('chartImage').src = url_base64;
            
            return url_base64;
        }};

        function hideImageHolder(){{
            document.getElementById('chartImage').hidden = true;
        }};

        function dataURItoBlob(dataURI) 
        {{
    		var binary = atob(dataURI.split(',')[1]);
    		var array = [];
    		for(var i = 0; i < binary.length; i++) 
            {{
        		array.push(binary.charCodeAt(i));
    		}}
    	    return new Blob([new Uint8Array(array)], {{type: 'image/png'}});
		}};
    </script>
</body>
</html>";
            return html;
        }

        public void Dispose() { }
    }
    class Base64Image
    {
        public static Base64Image Parse(string base64Content)
        {
            int indexOfSemiColon = base64Content.IndexOf(";", StringComparison.OrdinalIgnoreCase);

            string dataLabel = base64Content.Substring(0, indexOfSemiColon);

            string contentType = dataLabel.Split(':').Last();

            var startIndex = base64Content.IndexOf("base64,", StringComparison.OrdinalIgnoreCase) + 7;

            var fileContents = base64Content.Substring(startIndex);

            var bytes = Convert.FromBase64String(fileContents);

            return new Base64Image
            {
                ContentType = contentType,
                FileContents = bytes
            };
        }

        public async Task<BitmapImage> ToImage()
        {
            if (FileContents == null || FileContents.Length == 0) return null;

            return await GetBitmapAsync(FileContents);
        }

        static async Task<BitmapImage> GetBitmapAsync(byte[] data)
        {
            var bitmapImage = new BitmapImage();

            using (var stream = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(stream))
                {
                    writer.WriteBytes(data);
                    await writer.StoreAsync();
                    await writer.FlushAsync();
                    writer.DetachStream();
                }

                stream.Seek(0);
                await bitmapImage.SetSourceAsync(stream);
            }

            return bitmapImage;
        }

        public string ContentType { get; set; }

        public byte[] FileContents { get; set; }

        public override string ToString()
        {
            return $"data:{ContentType};base64,{Convert.ToBase64String(FileContents)}";
        }
    }
}

[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.OxyPlot/master/icon.png "Zebble.OxyPlot"


## Zebble.OxyPlot

![logo]

A Zebble plugin that allow you add OxyPlot charts into the Zebble application.


[![NuGet](https://img.shields.io/nuget/v/Zebble.OxyPlot.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.OxyPlot/)

> With this plugin you can add different types of charts such as Bar, Pie, Box, etc.

<br>


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.OxyPlot/](https://www.nuget.org/packages/Zebble.OxyPlot/)
* Install in your platform client projects.
* Available for iOS, Android and UWP.
<br>


### Api Usage

You can add chart to the Zebble page like below:

```csharp
public override async Task OnInitializing()
{
    await base.OnInitializing();

    var plotView = new OxyPlotView
    {
        Title = "Title of Chart",
        Axes = new List<Axis> { new CategoryAxis
        {
            Position = AxisPosition.Left,
            Key = "Title of Axis",
            ItemsSource = new[]
            {
                    "Sample 1",
                    "Sample 2",
                    "Sample 3",
                    "Sample 4",
                    "Sample 5"
            }
        } },
        Series = new List<Series> {
            new Series {
                PlotType = PlotTypes.Bar,
                Data = new List<PlotData>
                {
                    new PlotData(value: val1),
                    new PlotData(value: val2),
                    new PlotData(value: val3),
                    new PlotData(value: val4),
                    new PlotData(value: val5)
                }
            }
        }
    };
    
    plotView.Width(100.Percent()).Height(300);
    await Add(plotView);
}
```

#### Chart Types

There is a property whose name is `PlotType` in each series that related to the chart type and you can set it to each one that you need.

```csharp
var series = new List<Series> {
    new Series {
           PlotType = PlotTypes.Bar,
           Data = new List<PlotData>
           {
               new PlotData(value: val1),
               new PlotData(value: val2),
               ...
           }
       },
       ...
    };
```


### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| Title            | string           | x       | x   | x       |
| Series            | Series           | x       | x   | x       |
| Axes            | Axis           | x       | x   | x       |

### Methods
| Method       | Return Type  | Parameters                          | Android | iOS | Windows |
| :----------- | :----------- | :-----------                        | :------ | :-- | :------ |
| InitializePlot         | Task| -| x       | x   | x       |

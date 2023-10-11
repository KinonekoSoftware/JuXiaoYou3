using System.Collections.Generic;
using Acorisoft.FutureGL.MigaStudio.Controls.Models;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public abstract class ChartBase : Control
    {
        protected static readonly AxisCollection  DefaultAxis;
        protected static readonly ChartDataSet    DefaultData;
        protected static readonly PieChartDataSet DefaultPieData;
        protected static readonly List<int>       DefaultHistogramData;
        protected static readonly SolidColorBrush DefaultBrush       = new SolidColorBrush(Color.FromRgb(0xc0, 0xc0, 0xc0));
        private static readonly   double[]        GuidelineHelpArray = new[] { 0.5d, 0.5d };

        static ChartBase()
        {
            DefaultAxis = new AxisCollection
            {
                new Axis
                {
                    Name = "测试1"
                },
                new Axis
                {
                    Name = "测试2"
                },
                new Axis
                {
                    Name = "测试3"
                },
                new Axis
                {
                    Name = "测试4"
                },
                new Axis
                {
                    Name = "测试5"
                },
            };

            DefaultData = new ChartDataSet
            {
                new ChartData
                {
                    Color = "#FFAB12",
                    Values = new[]
                    {
                        4d, 3d, 6d, 7d, 9d
                    },
                    Maximum = 10
                },
                new ChartData
                {
                    Color = "#f34718",
                    Values = new[]
                    {
                        10d, 11d, 12d, 13d, 15d
                    },
                    Maximum = 20
                },
            };

            DefaultPieData = new PieChartDataSet
            {
                new PieChartData
                {
                    Color = "#007ACC",
                    Value = 3,
                    Name  = "Test1"
                },
                new PieChartData
                {
                    Color = "#FFAB12",
                    Value = 13,
                    Name  = "Test2"
                },
                new PieChartData
                {
                    Color = "#9132C8",
                    Value = 30,
                    Name  = "Test3"
                },
                new PieChartData
                {
                    Color = "#FF9600",
                    Value = 8,
                    Name  = "Test4"
                },
            };

            DefaultHistogramData = new List<int>
            {
                3,
                13,
                30,
                8,
            };
        }


        protected static void PushGuideline(DrawingContext drawingContext)
        {
            drawingContext.PushGuidelineSet(new GuidelineSet(GuidelineHelpArray, GuidelineHelpArray));
        }
    }
}
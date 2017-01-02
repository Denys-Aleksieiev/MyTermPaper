using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RenderServ" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RenderServ.svc or RenderServ.svc.cs at the Solution Explorer and start debugging.
    public class RenderServ : IRenderServ
    {
        public ChartArea CreateChartArea()
        {
            var chartArea = new ChartArea();
            chartArea.Name = "File types chart";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;
            return chartArea;
        }

        public Legend CreateLegend()
        {
            var legend = new Legend();
            legend.Name = "Result Chart";
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 14);
            legend.LegendStyle = LegendStyle.Row;

            return legend;
        }

        public Series CreateSeries(IList<Tuple<int, string>> results, SeriesChartType chartType, Color color)
        {
            var seriesDetail = new Series();
            seriesDetail.Name = "Result Chart";
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Font = new System.Drawing.Font("Trebuchet MS", 14F);
            seriesDetail.Color = color;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            seriesDetail["DrawingStyle"] = "Cylinder";
            seriesDetail["PieDrawingStyle"] = "SoftEdge";
            seriesDetail.MarkerSize = 20;
            DataPoint point;

            foreach (var result in results)
            {
                point = new DataPoint();
                point.AxisLabel = result.Item2;
                point.YValues = new double[] { result.Item1 };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "File types chart";

            return seriesDetail;
        }

        public Title CreateTitle()
        {
            Title title = new Title();
            title.Text = "File types chart";
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 24F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);

            return title;
        }
    }
}

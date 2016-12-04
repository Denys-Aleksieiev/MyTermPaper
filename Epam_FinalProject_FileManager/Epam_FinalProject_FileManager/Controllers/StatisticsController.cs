using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Epam_FinalProject_FileManager_BLL.Interfaces;
using i18n.Helpers;
using Microsoft.AspNet.Identity;

namespace Epam_FinalProject_FileManager.Controllers
{
    
    public class StatisticsController : Controller
    {
        private IFileService fileService;
        public StatisticsController(IFileService fileService)
        {
            this.fileService = fileService;
        }
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public FileContentResult MyStatistics()
        {
            return ComputeStatistics(User.Identity.GetUserId());
        }

        public FileContentResult PublicStatistics()
        {
            return ComputeStatistics("4830dc12 - e379 - 4daf - a658 - 65ca17d11ed3");
        }

        public FileContentResult ComputeStatistics(string id)
        {
            int video = fileService.GetAllUserVideos(id).ToList().Count;
            int audio = fileService.GetAllUserAudios(id).ToList().Count;
            int documents = fileService.GetAllUserDocuments(id).ToList().Count;
            int other = fileService.GetAllUserOtherFiles(id).ToList().Count;
            var valueList = new List<int>();
            valueList.Add(video);
            valueList.Add(audio);
            valueList.Add(documents);
            valueList.Add(other);
            var labelList = new List<string>();
            labelList.Add("Video");
            labelList.Add("Audio");
            labelList.Add("Documents");
            labelList.Add("Other");

            return RenderStatistics(valueList, labelList);
        }

        private FileContentResult RenderStatistics(List<int> values, List<string> labels )
        {
            var dates = new List<Tuple<int, string>>();
            for(int i = 0; i < values.Count; i++)
            {
                dates.Add(new Tuple<int, string>(values[i],labels[i]));
            }

            var chart = new Chart();
            chart.Width = 1000;
            chart.Height = 700;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle());
            chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(dates, SeriesChartType.Doughnut, Color.Gold));
            chart.ChartAreas.Add(CreateChartArea());
       
            

            var ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        [NonAction]
        private Title CreateTitle()
        {
            Title title = new Title();
            title.Text = "File types chart";
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS",24F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);

            return title;
        }


        [NonAction]
        private Series CreateSeries(IList<Tuple<int, string>> results,
       SeriesChartType chartType,
       Color color)
        {
            var seriesDetail = new Series();
            seriesDetail.Name = "Files type result";
            seriesDetail.IsValueShownAsLabel = false;
            


            seriesDetail.Font =  new System.Drawing.Font("Trebuchet MS", 14F);
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
                point.LegendText = result.Item2;
               
                seriesDetail.Points.Add(point);
            }
            
            seriesDetail.Label = "#PERCENT{P0}";
            seriesDetail.ChartArea = "File types chart";
        

            return seriesDetail;
        }


        [NonAction]
        private Legend CreateLegend()
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


        [NonAction]
        private ChartArea CreateChartArea()
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
    }
}
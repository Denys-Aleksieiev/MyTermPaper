using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRenderServ" in both code and config file together.
    [ServiceContract]
    public interface IRenderServ
    {
        [OperationContract]
        Title CreateTitle();
        [OperationContract]
        Series CreateSeries(IList<Tuple<int, string>> results, SeriesChartType chartType, Color color);
        [OperationContract]
        Legend CreateLegend();
        [OperationContract]
        ChartArea CreateChartArea();
    }
}

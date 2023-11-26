using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartModelsLibrary.ChartModels
{
    public class HistogramModel
    {
        public string ChartTitle { get; set; } = "Histogram";
        public string VerticalAxisTitle { get; set; } = "Frequency";
        public string HorizontalAxisTitle { get; set; } = "Bins";
        public  IEnumerable< decimal>? Observations { get; set; }
    }
}

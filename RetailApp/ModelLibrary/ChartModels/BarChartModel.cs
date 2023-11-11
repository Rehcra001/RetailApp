using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.ChartModels
{
    public class BarChartModel
    {
        public string ChartTitle { get; set; } = "Chart Title";
        public string VerticalAxisTitle { get; set; } = "Vertical Axis Title";
        public string HorizontalAxisTitle { get; set; } = "Horizontal Axis Title";
        public IEnumerable<decimal>? Values { get; set; }
        public IEnumerable<string>? ValuesDescription { get; set; }
    }
}

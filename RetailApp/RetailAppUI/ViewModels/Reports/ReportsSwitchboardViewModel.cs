using ModelsLibrary.ChartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.ViewModels.Reports
{
    public class ReportsSwitchboardViewModel : BaseViewModel
    {
        private BarChartModel _barChartData;
        public BarChartModel BarChartData
        {
            get { return _barChartData; }
            set { _barChartData = value; OnPropertyChanged(); }
        }

        public ReportsSwitchboardViewModel()
        {
            LoadBarChartData();
        }

        private void LoadBarChartData()
        {
            BarChartData = new BarChartModel
            {
                ChartTitle = "Sales Revenue",
                VerticalAxisTitle = "Revenue",
                HorizontalAxisTitle = "Months",
                Values = LoadValues(),
                ValuesDescription = LoadDescriptions()
            };


        }

        private IEnumerable<string>? LoadDescriptions()
        {
            return new List<string>
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June"
            };
        }

        private IEnumerable<decimal>? LoadValues()
        {
            return new List<decimal>
            {
                100,
                120,
                150,
                120,
                145,
                151
            };
        }
    }
}

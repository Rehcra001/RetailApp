using ChartModelsLibrary.ChartModels;
using System.Collections.Generic;

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
                312,
                120,
                89,
                256
            };
        }
    }
}

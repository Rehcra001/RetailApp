using ChartModelsLibrary.ChartModels;

namespace DataAccessLibrary.VendorMetricsRepository
{
    public interface IVendorMetricsRepositoryYTD
    {
        (HistogramModel, string) GetLeadTimeDaysCountByVendorIdYTD(int id);
        (HistogramModel, string) GetVendorDeliveryComplianceAllProducts(int id);
    }
}

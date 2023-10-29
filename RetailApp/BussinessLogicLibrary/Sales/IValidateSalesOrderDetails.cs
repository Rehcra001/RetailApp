using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IValidateSalesOrderDetails
    {
        (bool, bool) Validate(SalesOrderHeaderModel salesOrder);
    }
}
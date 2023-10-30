using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IValidateExistingSalesOrderDetails
    {
        (bool, bool) Validate(SalesOrderHeaderModel salesOrder);
    }
}
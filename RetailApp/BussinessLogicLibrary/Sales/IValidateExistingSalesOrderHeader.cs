using ModelsLibrary;

namespace BussinessLogicLibrary.Sales
{
    public interface IValidateExistingSalesOrderHeader
    {
        (bool, bool) Validate(SalesOrderHeaderModel salesOrder);
    }
}
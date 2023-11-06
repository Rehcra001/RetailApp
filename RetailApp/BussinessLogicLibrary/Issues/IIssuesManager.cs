using ModelsLibrary;

namespace BussinessLogicLibrary.Issues
{
    public interface IIssuesManager
    {
        IEnumerable<IssueModel> GetBySalesOrderID(long id);
        void Insert(IEnumerable<InvoicingLineModel> invoiceLines);
        void Reverse(int id);
    }
}
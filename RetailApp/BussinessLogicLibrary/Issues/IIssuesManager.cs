using ModelsLibrary;

namespace BussinessLogicLibrary.Issues
{
    public interface IIssuesManager
    {
        IEnumerable<IssueModel> GetBySalesOrderID(long id);
    }
}
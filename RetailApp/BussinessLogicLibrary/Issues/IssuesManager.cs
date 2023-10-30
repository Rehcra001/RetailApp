using DataAccessLibrary.ReceiptRepository;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Issues
{
    public class IssuesManager : IIssuesManager
    {
        private readonly IIssueRepository _issueRepository;

        public IssuesManager(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public IEnumerable<IssueModel> GetBySalesOrderID(long id)
        {
            Tuple<IEnumerable<IssueModel>, string> issues = _issueRepository.GetBySalesOrderID(id).ToTuple();

            //check for errors
            if (issues.Item2 == null)
            {
                //No errors
                return issues.Item1;
            }
            else
            {
                //error raised retrieving from database
                throw new Exception(issues.Item2);
            }
        }

    }
}

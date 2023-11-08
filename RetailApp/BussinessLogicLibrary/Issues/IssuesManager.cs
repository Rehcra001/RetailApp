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

        public void Insert(IEnumerable<InvoicingLineModel> invoiceLines)
        {
            List<IssueModel> issues = new List<IssueModel>();

            
            foreach(InvoicingLineModel invoice in invoiceLines)
            {
                //Validate each line
                if (!invoice.Validate())
                {
                    throw new Exception(invoice.ValidationMessage);
                }

                //create issued for invoice
                IssueModel issue = new IssueModel
                {
                    SalesOrderID = invoice.SalesOrderID,
                    ProductID = invoice.ProductID,
                    QuantityIssued = invoice.QtyToInvoice
                };

                //validate issue
                if (!issue.Validate())
                {
                    throw new Exception(issue.ValidationMessage);
                }
                //Add to list
                issues.Add(issue);
            }

            //Save issues
            foreach (IssueModel issueModel in issues)
            {
                Tuple<IssueModel, string> issued = _issueRepository.Insert(issueModel).ToTuple();
                //Check for errors
                if (issued.Item2 != null)
                {
                    //Error raised
                    throw new Exception(issued.Item2);
                }
            }
        }

        public void Reverse(int id)
        {
            string errorMessage = _issueRepository.ReverseByID(id);
            if (errorMessage != null)
            {
                //Error raised
                throw new Exception(errorMessage);
            }
        }

    }
}

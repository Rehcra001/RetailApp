namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        (IssueModel, string) Insert(IssueModel issue);
        (IEnumerable<IssueModel>, string) GetBySalesOrderID(long id);
        string ReverseByID(int id);
    }
}

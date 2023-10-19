namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IReceiptsRepository
    {
        (IEnumerable<ReceiptModel>, string) GetByPurchaseOrderID(long id);
        (ReceiptModel, string) Insert(ReceiptModel receipt);
        string ReverseByID(int id);
    }
}

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IVATRepository
    {
        (VatModel, string) Get();
    }
}

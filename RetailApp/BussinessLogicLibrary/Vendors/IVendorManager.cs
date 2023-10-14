using ModelsLibrary;

namespace BussinessLogicLibrary.Vendors
{
    public interface IVendorManager
    {
        void Delete(VendorModel vendor);
        IEnumerable<VendorModel> GetAll();
        VendorModel GetByID(int id);
        VendorModel Insert(VendorModel vendor);
        void Update(VendorModel vendor);
    }
}
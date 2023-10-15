using ModelsLibrary;

namespace BussinessLogicLibrary.Customers
{
    public interface ICustomerManager
    {
        void Delete(CustomerModel customer);
        IEnumerable<CustomerModel> GetAll();
        CustomerModel GetByID(int id);
        CustomerModel Insert(CustomerModel customer);
        void Update(CustomerModel customer);
    }
}
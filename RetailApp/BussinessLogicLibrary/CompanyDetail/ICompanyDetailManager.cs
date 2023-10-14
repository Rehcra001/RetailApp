using ModelsLibrary;

namespace BussinessLogicLibrary.CompanyDetail
{
    public interface ICompanyDetailManager
    {
        CompanyDetailModel Get();
        CompanyDetailModel Insert(CompanyDetailModel model);
        void Update(CompanyDetailModel model);
    }
}
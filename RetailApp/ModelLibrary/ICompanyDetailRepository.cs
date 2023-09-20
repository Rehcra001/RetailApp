using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public interface ICompanyDetailRepository
    {
        (CompanyDetailModel, string) Insert(CompanyDetailModel companyDetail);
        string Update(CompanyDetailModel companyDetail);
        (CompanyDetailModel, string) Get();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface IUnitsPerRepository
    {
        (UnitsPerModel, string) Insert(UnitsPerModel unitsPer);
        string Update(UnitsPerModel unitsPer);
        (IEnumerable<UnitsPerModel>, string) GetAll();
        (UnitsPerModel, string) GetByID(int id);

    }
}

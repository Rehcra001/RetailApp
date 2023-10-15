using ModelsLibrary;

namespace BussinessLogicLibrary.UnitPers
{
    public interface IUnitPerManager
    {
        IEnumerable<UnitsPerModel> GetAll();
        UnitsPerModel GetByID(int id);
        UnitsPerModel Insert(UnitsPerModel unitsPer);
        void Update(UnitsPerModel unitsPer);
    }
}
using ModelsLibrary;

namespace BussinessLogicLibrary.Statuses
{
    public interface IStatusManager
    {
        IEnumerable<StatusModel> GetAll();
        StatusModel GetByID(int id);
    }
}
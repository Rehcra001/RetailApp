using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Statuses
{
    public class StatusManager : IStatusManager
    {
        private readonly IStatusRepository _statusRepository;

        public StatusManager(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        /// <summary>
        /// Gets a list of statuses
        /// </summary>
        /// <returns>
        /// Returns IEnumerable<StatusModel>
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the statuses
        /// </exception>
        public IEnumerable<StatusModel> GetAll()
        {
            Tuple<IEnumerable<StatusModel>, string> statuses = _statusRepository.GetAll().ToTuple();
            //check for errors
            if (statuses.Item2 == null)
            {
                //No Errors
                return statuses.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(statuses.Item2);
            }
        }

        /// <summary>
        /// Gets a status by ID
        /// </summary>
        /// <param name="id">
        /// Takes in an integer ID
        /// </param>
        /// <returns>
        /// Returns a StatusModel
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrive the status
        /// </exception>
        public StatusModel GetByID(int id)
        {
            Tuple<StatusModel, string> status = _statusRepository.GetByID(id).ToTuple();
            //Check for errors
            if (status.Item2 == null)
            {
                //No Errors
                return status.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(status.Item2);
            }
        }
    }
}

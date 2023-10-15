using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.UnitPers
{
    public class UnitPerManager : IUnitPerManager
    {
        private readonly IUnitsPerRepository _unitsPerRepository;

        public UnitPerManager(IUnitsPerRepository unitsPerRepository)
        {
            _unitsPerRepository = unitsPerRepository;
        }

        /// <summary>
        /// Gets a list of UnitPers
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable<UnitsPerModel>
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the UnitPers
        /// </exception>
        public IEnumerable<UnitsPerModel> GetAll()
        {
            Tuple<IEnumerable<UnitsPerModel>, string> unitPers = _unitsPerRepository.GetAll().ToTuple();
            //Check for errors
            if (unitPers.Item2 == null)
            {
                //No Errors
                return unitPers.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(unitPers.Item2);
            }
        }

        /// <summary>
        /// Gets a UnitPer by ID
        /// </summary>
        /// <param name="id">
        /// Takes in an integer ID
        /// </param>
        /// <returns>
        /// Returns a UnitsPerModel
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the UnitPer
        /// </exception>
        public UnitsPerModel GetByID(int id)
        {
            Tuple<UnitsPerModel, string> unitPer = _unitsPerRepository.GetByID(id).ToTuple();
            //check for errors
            if (unitPer.Item2 == null)
            {
                //no error
                return unitPer.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(unitPer.Item2);
            }
        }

        /// <summary>
        /// Inserts a new UnitPer
        /// </summary>
        /// <param name="unitsPer">
        /// Takes in a UnitsPerModel
        /// </param>
        /// <returns>
        /// Returns a UnitsPerModel
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if validation fails. 
        /// Throws an exception if unable to insert the UnitsPer
        /// </exception>
        public UnitsPerModel Insert(UnitsPerModel unitsPer)
        {
            //Check for validation errors
            if (unitsPer.Validate())
            {
                //No validation error
                Tuple<UnitsPerModel, string> insertedUnitPer = _unitsPerRepository.Insert(unitsPer).ToTuple();
                //Check for errors
                if (insertedUnitPer.Item2 == null)
                {
                    //No Errors
                    return insertedUnitPer.Item1;
                }
                else
                {
                    //Error raised
                    throw new Exception(insertedUnitPer.Item2);
                }
            }
            else
            {
                //Validation error
                throw new Exception(unitsPer.ValidationMessage);
            }
        }


        public void Update(UnitsPerModel unitsPer)
        {
            //Check for validation errors
            if (unitsPer.Validate())
            {
                //No validation Error
                string errorMessage = _unitsPerRepository.Update(unitsPer);
                //Check for errors
                if (errorMessage != null)
                {
                    //error raise
                    throw new Exception(errorMessage);
                }
            }
            else
            {
                //validation error
                throw new Exception(unitsPer.ValidationMessage);
            }
        }
    }
}

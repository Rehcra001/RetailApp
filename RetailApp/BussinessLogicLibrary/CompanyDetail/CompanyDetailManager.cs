using DataAccessLibrary.CompanyDetailRepository;
using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.CompanyDetail
{
    public class CompanyDetailManager : ICompanyDetailManager
    {
        private readonly ICompanyDetailRepository _companyDetailRepository;

        public CompanyDetailManager(ICompanyDetailRepository companyDetailRepository)
        {
            _companyDetailRepository = companyDetailRepository;
        }

        /// <summary>
        /// Gets the company detail
        /// </summary>
        /// <returns>
        /// Returns a CompanyDetailModel. 
        /// It may be a null CompanyDetailModel if it does not exist in the database yet
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if any error was raised retrieving the data
        /// </exception>
        public CompanyDetailModel Get()
        {
            Tuple<CompanyDetailModel, string> model = _companyDetailRepository.Get().ToTuple();

            //Check for errors
            if (model.Item2 == null)
            {
                //No errors
                return model.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(model.Item2);
            }
        }

        /// <summary>
        /// Inserts a CompanyModel
        /// </summary>
        /// <param name="model">
        /// Takes in a CompanyModel
        /// </param>
        /// <returns>
        /// Returns a CompanyModel with ID
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if data validation fails. 
        /// Throws an exception if insertion failed
        /// </exception>
        public CompanyDetailModel Insert(CompanyDetailModel model)
        {
            //First validate the model
            model.Validate();
            //Check if any validation errors raised
            if (string.IsNullOrWhiteSpace(model.ValidationMessage))
            {
                //No Validation error 
                //try insert into database
                Tuple<CompanyDetailModel, string> insertedModel = _companyDetailRepository.Insert(model).ToTuple();

                //check for errors inserting
                if (insertedModel.Item2 == null)
                {
                    //No error raised
                    //Returns the inserted model with ID
                    return insertedModel.Item1;
                }
                else
                {
                    //Error inserting
                    throw new Exception(insertedModel.Item2);
                }
            }
            else
            {
                //Validation error
                throw new Exception(model.ValidationMessage);
            }
        }

        /// <summary>
        /// Updates a CompanyModel 
        /// </summary>
        /// <param name="model">
        /// Takes in a CompanyModel
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception if data validation fails. 
        /// Throws an exception if update failed
        /// </exception>
        public void Update(CompanyDetailModel model)
        {
            //First validate the model
            model.Validate();
            //Check if any validation errors raised
            if (string.IsNullOrWhiteSpace(model.ValidationMessage))
            {
                //No Validation error 
                //try insert into database
                string errorMessage = _companyDetailRepository.Update(model);

                //check for errors inserting
                if (errorMessage != null)
                {
                    //Error inserting
                    throw new Exception(errorMessage);
                }
            }
            else
            {
                //Validation error
                throw new Exception(model.ValidationMessage);
            }
        }
    }
}

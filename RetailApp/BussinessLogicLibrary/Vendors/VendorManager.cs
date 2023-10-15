using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Vendors
{
    public class VendorManager : IVendorManager
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorManager(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        /// <summary>
        /// Deletes a vendor 
        /// </summary>
        /// <param name="vendor">
        /// Takes in a vendor model to delete
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception if an error is encountered deleting the vendor
        /// </exception>
        public void Delete(VendorModel vendor)
        {
            string errorMessage = _vendorRepository.Delete(vendor);
            //Check for errors
            if (errorMessage != null)
            {
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Retrieves a list of vendors
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable<VendorModel>
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an execption if any error is encountered retrieving the vendors
        /// </exception>
        public IEnumerable<VendorModel> GetAll()
        {
            Tuple<IEnumerable<VendorModel>, string> vendors = _vendorRepository.GetAll().ToTuple();

            //Check for errors
            if (vendors.Item2 == null)
            {
                //No errors
                return vendors.Item1;
            }
            else
            {
                //error raised
                throw new Exception(vendors.Item2);
            }
        }

        /// <summary>
        /// Retrieves an existing vendor by ID
        /// </summary>
        /// <param name="id">
        /// Takes in an integer ID
        /// </param>
        /// <returns>
        /// Returns a VendorModel
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an execption if any error is encountered retrieving the vendor
        /// </exception>
        public VendorModel GetByID(int id)
        {
            Tuple<VendorModel, string> vendor = _vendorRepository.GetByID(id).ToTuple();

            //check for errors
            if (vendor.Item2 == null)
            {
                //No error
                return vendor.Item1;
            }
            else
            {
                //error raised
                throw new Exception(vendor.Item2);
            }
        }


        /// <summary>
        /// Inserts a new vendor
        /// </summary>
        /// <param name="vendor">
        /// Takes in a vendor model
        /// </param>
        /// <returns>
        /// Returns a vendor model with the ID field populated
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception validation error encountered. 
        /// Throws an exception if unable to insert the vendor
        /// </exception>
        public VendorModel Insert(VendorModel vendor)
        {
            //Check if any validation errors encountered
            if (vendor.Validate())
            {
                //No Validation errors
                Tuple<VendorModel, string> insertedVendor = _vendorRepository.Insert(vendor).ToTuple();

                //check for errors
                if (insertedVendor.Item2 == null)
                {
                    return insertedVendor.Item1;
                }
                else
                {
                    //error inserting vendor
                    throw new Exception(insertedVendor.Item2);
                }
            }
            else
            {
                //Validation error
                throw new Exception(vendor.ValidationMessage);
            }
        }


        public void Update(VendorModel vendor)
        {
            //Check if any validation errors encountered
            if (vendor.Validate())
            {
                //No Validation errors
                string errorMessage = _vendorRepository.Update(vendor);

                //check for errors
                if (errorMessage != null)
                {
                    //error inserting vendor
                    throw new Exception(errorMessage);
                }
            }
            else
            {
                //Validation error
                throw new Exception(vendor.ValidationMessage);
            }
        }
    }
}

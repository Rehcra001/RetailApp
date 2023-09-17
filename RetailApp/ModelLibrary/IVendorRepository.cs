using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public interface IVendorRepository
    {
        /// <summary>
        /// Creates a new vendor
        /// </summary>
        /// <param name="vendor">
        /// Takes in the vendor model to be created 
        /// </param>
        /// <returns>
        /// Returns a vendor model with vendor id if add was successful- otherwise id will be zero
        /// Returns a string error message if add not successful - null otherwise
        /// </returns>
        (VendorModel, string) Insert(VendorModel vendor);

        /// <summary>
        /// Updates an existing vendor
        /// </summary>
        /// <param name="vendor">
        /// Takes in the vendor to be updated
        /// </param>
        /// <returns>
        /// Returns a string error message if the update was not successful - null otherwise
        /// </returns>
        string Update(VendorModel vendor);

        /// <summary>
        /// Deletes a vendor
        /// </summary>
        /// <param name="vendor">
        /// Takes in the vendor model to be deleted
        /// </param>
        /// <returns>
        /// Returns a string error message if the delete was not successful - null otherwise
        /// </returns>
        string Delete(VendorModel vendor);

        /// <summary>
        /// Gets a list of all vendors
        /// </summary>
        /// <returns>
        /// Return an observable collection of vendors if successfull - null otherwise
        /// Returns a string error message if not successfull - null otherwise
        /// </returns>
        (ObservableCollection<VendorModel>, string) GetAll();

        /// <summary>
        /// Get a vendor by ID
        /// </summary>
        /// <param name="id">
        /// Takes in the vendor ID of type int
        /// </param>
        /// <returns>
        /// Returns a vendor model if the retrieve was successfull - null otherwise
        /// Return a string error message if the retrieve was not successful
        /// </returns>
        (VendorModel, string) GetByID(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customer">
        /// Takes in the customer model to be created 
        /// </param>
        /// <returns>
        /// Returns a customer model with customer id if add was successful- otherwise id will be zero
        /// Returns a string error message if add not successful - null otherwise
        /// </returns>
        (CustomerModel, string) Insert(CustomerModel customer);

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="customer">
        /// Takes in the customer to be updated
        /// </param>
        /// <returns>
        /// Returns a string error message if the update was not successful - null otherwise
        /// </returns>
        string Update(CustomerModel customer);

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="customer">
        /// Takes in the customer model to be deleted
        /// </param>
        /// <returns>
        /// Returns a string error message if the delete was not successful - null otherwise
        /// </returns>
        string Delete(CustomerModel customer);

        /// <summary>
        /// Gets a list of all customers
        /// </summary>
        /// <returns>
        /// Return an observable collection of customers if successfull - null otherwise
        /// Returns a string error message if not successfull - null otherwise
        /// </returns>
        (IEnumerable<CustomerModel>, string) GetAll();

        /// <summary>
        /// Get a customer by ID
        /// </summary>
        /// <param name="id">
        /// Takes in the customer ID of type int
        /// </param>
        /// <returns>
        /// Returns a customer model if the retrieve was successfull - null otherwise
        /// Return a string error message if the retrieve was not successful
        /// </returns>
        (CustomerModel, string) GetByID(int id);
    }
}

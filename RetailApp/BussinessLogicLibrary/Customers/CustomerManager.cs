using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicLibrary.Customers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="customer">
        /// Takes in a CustomerModel to delete
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception if unable to delete the customer
        /// </exception>
        public void Delete(CustomerModel customer)
        {
            string errorMessage = _customerRepository.Delete(customer);
            //check for errors
            if (errorMessage != null)
            {
                //Error raised
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Gets a list of all customers
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable<CustomerModel>
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an execption if unable to retrieve the customers
        /// </exception>
        public IEnumerable<CustomerModel> GetAll()
        {
            Tuple<IEnumerable<CustomerModel>, string> customers = _customerRepository.GetAll().ToTuple();
            //check for errors
            if (customers.Item2 == null)
            {
                //No Error
                return customers.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(customers.Item2);
            }
        }

        /// <summary>
        /// Get a customer given an ID
        /// </summary>
        /// <param name="id">
        /// Takes in an integer representing the ID
        /// </param>
        /// <returns>
        /// Returns a CustomerModel
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the customer
        /// </exception>
        public CustomerModel GetByID(int id)
        {
            Tuple<CustomerModel, string> customer = _customerRepository.GetByID(id).ToTuple();
            //Check for errors
            if (customer.Item2 == null)
            {
                //No Error
                return customer.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(customer.Item2);
            }
        }

        /// <summary>
        /// Inserts a new customer
        /// </summary>
        /// <param name="customer">
        /// Takes in a CustomerModel
        /// </param>
        /// <returns>
        /// Returns a Customer Model with ID
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if validation error encountered. 
        /// Throws an exception if unable to insert customer
        /// </exception>
        public CustomerModel Insert(CustomerModel customer)
        {
            //Validate the customer
            customer.Validate();
            //check if any validation errors raised
            if (string.IsNullOrWhiteSpace(customer.ValidationMessage))
            {
                //No Validation error
                Tuple<CustomerModel, string> insertedCustomer = _customerRepository.Insert(customer).ToTuple();
                //Check for errors
                if (insertedCustomer.Item2 == null)
                {
                    //No Error
                    return insertedCustomer.Item1;
                }
                else
                {
                    //Error inserting customer
                    throw new Exception(insertedCustomer.Item2);
                }
            }
            else
            {
                //Validation error
                throw new Exception(customer.ValidationMessage);
            }
        }

        /// <summary>
        /// Updates a customer
        /// </summary>
        /// <param name="customer">
        /// Takes in a CustomerModel to update
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception if validation error encountered. 
        /// Throws an exception if unable to update the customer
        /// </exception>
        public void Update(CustomerModel customer)
        {
            //Validate customer
            customer.Validate();
            //check if any validation errors raised
            if (string.IsNullOrWhiteSpace(customer.ValidationMessage))
            {
                //No Validation error
                string errorMessage = _customerRepository.Update(customer);
                //Check for errors
                if (errorMessage != null)
                {
                    //Error raised
                    throw new Exception(errorMessage);
                }
            }
            else
            {
                //Validation error
                throw new Exception(customer.ValidationMessage);
            }
        }
    }
}

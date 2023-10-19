using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class CategoryModel
    {
        /// <summary>
        /// Holds the unique category ID
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Holds the category Name
        /// </summary>
        public string? CategoryName { get; set; }

        public string? ValidationMessage { get; private set; }


        /// <summary>
        /// Validates the data entered. 
        /// ValidationMessage property is populated with the errors list if any errors encountered.
        /// </summary>
        /// <returns>
        /// True if no errors encountered - false otherwise. If false see ValidationMessage property
        /// </returns>
        public bool Validate()
        {
            bool isValid = true;
            ValidationMessage = "";

            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                ValidationMessage += "Category Name is required.\r\n";
                isValid = false;
            }
            else if (CategoryName.Length > 50)
            {
                ValidationMessage += "Category Name cannot have more than 50 characters.\r\n";
                isValid = false;
            }

            return isValid;
        }
    }
}

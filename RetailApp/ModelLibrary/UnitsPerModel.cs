using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class UnitsPerModel
    {
        /// <summary>
        /// Unique unitPer id key
        /// </summary>
        public int UnitPerID { get; set; }

        /// <summary>
        /// Holds the unitper abbreviation (m for Meters etc)
        /// </summary>
        public string? UnitPer { get; set; }

        /// <summary>
        /// Holds a description of the unitper (Meters etc)
        /// </summary>
        public string? UnitPerDescription { get; set; }

        public string? ValidationMessage { get; private set; }

        /// <summary>
        /// Validates this model and sets the Validation message
        /// validation finds an errror
        /// </summary>
        /// <returns>
        /// Returns true if no validation errors are encounter
        /// </returns>
        public bool Validate()
        {
            bool isValid = true;
            ValidationMessage = "";

            if (string.IsNullOrWhiteSpace(UnitPer))
            {
                ValidationMessage += "Unit Per is required.\r\n";
                isValid = false;
            }
            else if (UnitPer.Length > 10)
            {
                ValidationMessage += "Unit Per cannot have more than 10 characters.\r\n";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(UnitPerDescription))
            {
                ValidationMessage += "Unit Per description cannot have more than 20 characters.\r\n";
                isValid = false;
            }


            return isValid;
        }
    }
}

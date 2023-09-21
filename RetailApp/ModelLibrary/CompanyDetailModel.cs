using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class CompanyDetailModel
    {       
        /// <summary>
        /// Unique vendor id key
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Holds is the vendors company name
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// Holds the vendors vat registration number
        /// </summary>
        public string? VatRegistrationNumber { get; set; }

        /// <summary>
        /// Holds is the vendors address
        /// </summary>
        public string? AddressLine1 { get; set; }

        /// <summary>
        /// Holds the vendors address
        /// </summary>
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// Holds the city the vendor is located in
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Holds the province the vendor is located in
        /// </summary>
        public string? Province { get; set; }

        /// <summary>
        /// Holds the postal code of the vendor
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
        /// Holds is the first name of the vendor contact
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Holds is the last name of the vendor contact
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Holds the email address of the vendor contact
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Holds the phone number of the vendor contact
        /// </summary>
        
        public string? PhoneAreaCode { get; set; }

        /// <summary>
        /// Holds the first 3 digits of the phone number
        /// </summary>
        public string? PhonePrefix { get; set; }

        /// <summary>
        /// Holds the last 4 digits of the phone number
        /// </summary>
        public string? PhoneSuffix { get; set; }

        /// <summary>
        /// Concatenates first and last name
        /// </summary>
        public string? FullName { get => $"{FirstName} {LastName}";}

        public string? ValidationMessage { get; private set; }

        public bool Validate()
        {
            ValidationMessage = "";
            bool isValid = true;

            //Company Name
            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                ValidationMessage += "Company name is required.\r\n";
                isValid = false;
            }
            else if (CompanyName.Length > 100)
            {
                ValidationMessage += "Company name cannot be longer than 100 characters.\r\n";
                isValid = false;
            }

            //Vat Registration Number
            if (string.IsNullOrWhiteSpace(VatRegistrationNumber))
            {
                ValidationMessage += "Vat registration number is required\r\n";
                isValid = false;
            }
            else if (VatRegistrationNumber.Length != 10)
            {
                ValidationMessage += "Vat registration number must be 10 characters.\r\n";
                isValid = false;
            }
            else if (!long.TryParse(VatRegistrationNumber, out _))
            {
                ValidationMessage += "Vat Registration number may only consist of digits.\r\n";
                isValid = false;
            }

            //Address Line 1
            if (string.IsNullOrWhiteSpace(AddressLine1))
            {
                ValidationMessage += "Address Line 1 is required\r\n";
                isValid = false;
            }
            else if (AddressLine1.Length > 255)
            {
                ValidationMessage += "Address Line 1 cannot have more than 255 characters\r\n";
                isValid = false;
            }

            //Address Line 2
            if (!string.IsNullOrWhiteSpace(AddressLine2) && AddressLine2.Length > 255)
            {
                ValidationMessage += "Address Line 2 cannot have more than 255 characters\r\n";
                isValid = false;
            }

            //City
            if (string.IsNullOrWhiteSpace(City))
            {
                ValidationMessage += "City is required\r\n";
                isValid = false;
            }
            else if (City.Length > 50)
            {
                ValidationMessage += "City cannot contain more than 50 characters\r\n";
                isValid = false;
            }

            //Province
            if (string.IsNullOrWhiteSpace(Province))
            {
                ValidationMessage += "Province is required.\r\n";
                isValid = false;
            }
            else if (Province.Length > 50)
            {
                ValidationMessage += "Province cannot contain more than 50 characters\r\n";
                isValid = false;
            }

            //Postal Code
            if (string.IsNullOrWhiteSpace(PostalCode))
            {
                ValidationMessage += "Postal Code is required.\r\n";
                isValid = false;
            }
            else if (PostalCode.Length != 4)
            {
                ValidationMessage += "Postal code must contain 4 characters.\r\n";
                isValid = false;
            }
            else if (!int.TryParse(PostalCode, out _))
            {
                ValidationMessage += "Postal code must consits of digits only.\r\n";
                isValid = false;
            }

            //First Name
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                ValidationMessage += "First Name is required.\r\n";
                isValid = false;
            }
            else if (FirstName.Length > 100)
            {
                ValidationMessage += "First Name cannot contain more than 100 characters.\r\n";
                isValid = false;
            }

            //Last Name
            if (string.IsNullOrWhiteSpace(LastName))
            {
                ValidationMessage += "Last Name is required.\r\n";
                isValid = false;
            }
            else if (LastName.Length > 100)
            {
                ValidationMessage += "Last Name cannot contain more than 100 characters.\r\n";
                isValid = false;
            }

            //EMail Address
            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                ValidationMessage += "EMail address is required.\r\n";
                isValid = false;
            }

            //Phone area code
            if (string.IsNullOrWhiteSpace(PhoneAreaCode))
            {
                ValidationMessage += "Phone area code is required.\r\n";
                isValid = false;
            }
            else if (PhoneAreaCode.Length != 3)
            {
                ValidationMessage += "Phone area code must consist of 3 characters.\r\n";
                isValid = false;
            }
            else if (!int.TryParse(PhoneAreaCode, out _))
            {
                ValidationMessage += "Phone area code must only contain digits.\r\n";
                isValid = false;
            }

            //Phone prefix
            if (string.IsNullOrWhiteSpace(PhonePrefix))
            {
                ValidationMessage += "Phone prefix is required.\r\n";
                isValid = false;
            }
            else if (PhonePrefix.Length != 3)
            {
                ValidationMessage += "Phone prefix must consist of 3 characters.\r\n";
                isValid = false;
            }
            else if (!int.TryParse(PhonePrefix, out _))
            {
                ValidationMessage += "Phone prefix must only contain digits.\r\n";
                isValid = false;
            }

            //Phone suffix
            if (string.IsNullOrWhiteSpace(PhoneSuffix))
            {
                ValidationMessage += "Phone suffix is required.\r\n";
                isValid = false;
            }
            else if (PhoneSuffix.Length != 4)
            {
                ValidationMessage += "Phone suffix must consist of 4 characters.\r\n";
                isValid = false;
            }
            else if (!int.TryParse(PhoneSuffix, out _))
            {
                ValidationMessage += "Phone suffix must only contain digits.\r\n";
                isValid = false;
            }


            return isValid;
        }

    }
}

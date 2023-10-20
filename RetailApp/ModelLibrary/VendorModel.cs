namespace ModelsLibrary
{
    public class VendorModel
    {       
        /// <summary>
        /// Unique vendor id key
        /// </summary>
        public int VendorID { get; set; }

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
        /// Holds the country the vedor is in
        /// </summary>
        public string? Country { get; set; }

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
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Is this a international vendor - External to this country
        /// </summary>
        public bool InternationalVendor { get; set; }

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

            
            return isValid;
        }

    }
}

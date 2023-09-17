using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class VendorModel
    {
        /// <summary>
        /// Unique vendor id key
        /// </summary>
        public int VendorID { get; set; }

        /// <summary>
        /// Holds is the first name of the vendor contact
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Holds is the last name of the vendor contact
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Holds is the vendors company name
        /// </summary>
        public string? CompanyName { get; set; }

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
        /// Holds the email address of the vendor contact
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Holds the phone number of the vendor contact
        /// </summary>
        public string? PhoneNumber { get; set; }

    }
}

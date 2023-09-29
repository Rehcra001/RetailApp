using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class VatModel
    {
        /// <summary>
        /// Holds the VAT - eg 15 for 15% VAT
        /// </summary>
        public decimal VAT { get; set; }

        /// <summary>
        /// Holds the VAT as a decimal value eg 0.15
        /// </summary>
        public decimal VatDecimal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class UnitsModel
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
    }
}

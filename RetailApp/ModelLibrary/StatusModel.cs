﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class StatusModel
    {
        /// <summary>
        /// Holds the unique order status ID
        /// </summary>
        public int StatusID { get; set; }

        /// <summary>
        /// Holds the Order status Lookup Values 
        /// </summary>
        public string? Status { get; set; }
    }
}

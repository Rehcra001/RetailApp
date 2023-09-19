using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.Services
{
    public interface ICurrentViewService
    {
        /// <summary>
        /// Global property tracks which view is currently open
        /// </summary>
        public string? CurrentView { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.Services
{
    public class SharedDataService : ISharedDataService
    {
        private object _sharedData;

        /// <summary>
        /// A singleton property that can be used to share data between viewModels
        /// </summary>
        public object SharedData { get => _sharedData; set => _sharedData = value; }
    }
}

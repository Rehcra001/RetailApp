using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.Services
{
    internal class ConnectionStringService : IConnectionStringService
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SqlRetailAppDB"].ConnectionString;
        }
    }
}

using System;
using System.Configuration;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.Utils.Infrastructure
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString
        {
            get
            {
                var server = ConfigurationManager.ConnectionStrings["CentalConnection"].ConnectionString;
                if(string.IsNullOrEmpty(server))
                    throw new Exception("A valid connection string needs to be set in the configuration file.");
                return server;
            }
        }
    }
}
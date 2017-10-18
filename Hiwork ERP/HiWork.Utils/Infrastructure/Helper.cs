using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.Utils.Infrastructure
{
    public class Helper
    {
        public static string  GenerateUniqueID(string ApplicationCode, string NextID)
        {
            string customerID = string.Empty;

            customerID = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMdd"), ApplicationCode, NextID);
           // customerID = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMdd"), NextID);
            return customerID;
        }
    }
}

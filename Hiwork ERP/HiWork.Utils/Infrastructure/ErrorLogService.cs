using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;
using System.Globalization;
namespace HiWork.Utils.Infrastructure
{
    
    public partial interface IErrorLogService
    {
       void SetErrorLog(long userId, string component, string desrciption);
    }
    public class ErrorLogService: IErrorLogService 
    {
        private readonly ISqlConnectionService _sqlConnService;
        public ErrorLogService(){
            _sqlConnService = new SqlConnectionService();
        }
        public void SetErrorLog(long userId, string component, string desrciption)
        {
            _sqlConnService.OpenConnection();
            SqlCommand cmd = new SqlCommand("SP_CreateErrorlog", _sqlConnService.CreateConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Component", component);
            cmd.Parameters.AddWithValue("@Description", desrciption);
            cmd.ExecuteNonQuery();
            _sqlConnService.CloseConnection();
        }
     
    }
}

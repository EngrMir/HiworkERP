


using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;


namespace HiWork.BLL.Services
{
    public interface IPayPalTransactionService
    {
        bool InsertPaypalTransactionData(string guid, string paymentid, long ApplicationID);
        string GetPaymentIDByGuid(string guid);
    }

    public class PayPalTransactionService : IPayPalTransactionService, IDisposable
    {
        private readonly ISqlConnectionService _sqlConnService;

        public PayPalTransactionService ()
        {
            _sqlConnService = new SqlConnectionService();
        }


        public bool InsertPaypalTransactionData(string guid, string paymentid, long ApplicationID)
        {
            bool IsSuccessful;
            SqlCommand cmd;

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_SaveDeletePaypalTransaction", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@GUID", guid);
                cmd.Parameters.AddWithValue("@PAYMENTID", paymentid);
                cmd.Parameters.AddWithValue("@APPID", ApplicationID);
                cmd.Parameters.AddWithValue("@STATEMENT_TYPE", "INSERT");
                cmd.ExecuteNonQuery();
                IsSuccessful = true;
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return IsSuccessful;
        }


        public string GetPaymentIDByGuid(string guid)
        {
            string paymentid;
            SqlCommand cmd;
            SqlParameter ReturnParameter;

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetPaypalPaymentID", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GUID", guid);
                ReturnParameter = cmd.Parameters.Add("@PAYMENTID", SqlDbType.NVarChar, 1000);
                ReturnParameter.Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                paymentid = ReturnParameter.Value.ToString();
            }
            catch(Exception ex)
            {
                paymentid = null;
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return paymentid;
        }


        public virtual void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
            return;
        }
    }
}

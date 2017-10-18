using HiWork.BLL.Models;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{
    public partial interface IAccountingService 
    {
        List<StaffPaymentModel> GetStaffPaymentList(BaseViewModel model);
    }
    public class AccountingService : IAccountingService
    {

        private readonly ISqlConnectionService _sqlConnService;

        public AccountingService()
        {
            _sqlConnService= new SqlConnectionService();
        }
        public List<StaffPaymentModel> GetStaffPaymentList(BaseViewModel model)
        {
            List<StaffPaymentModel> staffPaymentList = new List<StaffPaymentModel>();
            try
            {
                StaffPaymentModel staffpayment;
          
                _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetStaffPayment", _sqlConnService.CreateConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                command.Parameters.AddWithValue("@STAFFNO", DBNull.Value);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    staffpayment = new StaffPaymentModel();

                  //  staffpayment.Number = dr["Number"].ToString();
                    staffpayment.Name = dr["Name"].ToString();
                   // staffpayment.TradingCorporationID = Convert.ToInt32(dr["TradingCorporationID"].ToString());
                 //   staffpayment.TradingCorporation = dr["TradingCorporation"].ToString();
                    staffpayment.DepositeType = dr["DepositeType"].ToString();
                    if (dr["AccountTypeID"] != DBNull.Value)
                    {
                        staffpayment.DepositeTypeId = Convert.ToInt64(dr["AccountTypeID"].ToString());
                    }
              
                    staffpayment.Bank = dr["Bank"].ToString();

                    staffpayment.BankId = dr["BankId"]!=DBNull.Value? Guid.Parse(dr["BankId"].ToString()): Guid.Empty;
                    staffpayment.Branch = dr["Branch"].ToString();
                    staffpayment.BranchId = dr["BankBranchID"]!=DBNull.Value? Guid.Parse(dr["BankBranchID"].ToString()):Guid.Empty;
                    staffpayment.AccountNumber = dr["AccountNumber"].ToString();
                    staffpayment.AccountHolder = dr["AccountHolder"].ToString();
                   // staffpayment.RemittableAmount = decimal.Parse(dr["RemittableAmount"].ToString());
                  //  staffpayment.Carryover = decimal.Parse(dr["Carryover"].ToString());
                  //  staffpayment.LastAdjustmentAmount = decimal.Parse(dr["LastAdjustmentAmount"].ToString());
                 //   staffpayment.ScheduledRemittanceAmount = decimal.Parse(dr["ScheduledRemittanceAmount"].ToString());
                  //  staffpayment.StaffBurdenFee = decimal.Parse(dr["StaffBurdenFee"].ToString());
                 //   staffpayment.BcauseBurdenFee = decimal.Parse(dr["BcauseBurdenFee"].ToString());
                 //   staffpayment.StaffTransferAmount = decimal.Parse(dr["StaffTransferAmount"].ToString());
                 //   staffpayment.BcauseTransferAmount = decimal.Parse(dr["BcauseTransferAmount"].ToString());
                 //   staffpayment.MoneyTransfer = decimal.Parse(dr["MoneyTransfer"].ToString());
                 //   staffpayment.RemittanceDate = Convert.ToDateTime(dr["RemittanceDate"].ToString());
                  //  staffpayment.RemittedAmount = decimal.Parse(dr["RemittedAmount"].ToString());
                 //   staffpayment.AdjustmentAmount = decimal.Parse(dr["AdjustmentAmount"].ToString());
                 //   staffpayment.Notes = dr["Notes"].ToString();
                    staffpayment.Email = dr["Email"].ToString();

                    staffPaymentList.Add(staffpayment);
                }
                _sqlConnService.CloseConnection();
           }
            catch(Exception ex) { }
            return staffPaymentList;
        }
    }
}

using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace HiWork.BLL.Services
{

    public partial interface IHomeApiService
    {
        dynamic Get_OrderDetails(HomeSearchModel model);
        dynamic Get_StaffDetails(HomeSearchModel model);
        dynamic Get_ClientDetails(HomeSearchModel model);
    }

    public class HomeService : IHomeApiService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public HomeService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
        }
        public dynamic Get_OrderDetails(HomeSearchModel model)
        {
            var orderModel = new List<EstimationModel>();
            try
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = false;
                var cultureId = new SqlParameter("Culture", model.CurrentCulture);
                var type = new SqlParameter("SearchType", model.Search.Type);
                var value = new SqlParameter("Value", model.Search.Value);
                
                IQueryable<EstimationModel> query = _dbContext.Database.SqlQuery<EstimationModel>
                    ("EXEC SP_GetHomeSearchData @Culture, @SearchType, @Value", cultureId, type, value).AsQueryable();

                orderModel = query.ToList();
                //orderModel.ForEach(el => { el.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), el.EstimationStatus); });
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Home Search", message);
                throw new Exception(message);
            }
            finally
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return orderModel;
        }

        public dynamic Get_StaffDetails(HomeSearchModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetHomeSearchData", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Culture", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@SearchType", model.Search.Type);
                cmd.Parameters.AddWithValue("@Value", model.Search.Value);

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }

        public dynamic Get_ClientDetails(HomeSearchModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetHomeSearchData", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Culture", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@SearchType", model.Search.Type);
                cmd.Parameters.AddWithValue("@Value", model.Search.Value);

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == false)
                return;
            if (this._dbContext == null)
                return;
            this._dbContext.Dispose();
            this._dbContext = null;
            return;
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            return;
        }
    }
}

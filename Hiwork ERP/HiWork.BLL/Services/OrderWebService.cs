
using HiWork.DAL;
using HiWork.BLL.Models;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using HiWork.DAL.Database;
using System.Data.Entity;
using System.Linq;
using AutoMapper;

namespace HiWork.BLL.Services
{
    public partial interface IOrderWebService
    {
        List<OrderWebModel> GetAllWebOrderList(OrderFilter filter);
        List<OrderWebModel> GetAppointedOrderList(OrderFilter filter);

        string SaveWebOrder(OrderWebModel model);
        OrderWebModel OrderOperation(OrderWebModel model, string type);
        List<MessageModel> GetAllMessageList(OrderWebModel model);
    }

    public class OrderWebService : IOrderWebService, IDisposable
    {
        private readonly ISqlConnectionService _sqlConnService;
        private CentralDBEntities _dbContext;

        public OrderWebService()
        {
            _sqlConnService = new SqlConnectionService();
            _dbContext = new CentralDBEntities();
        }

        public List<OrderWebModel> GetAllWebOrderList(OrderFilter filter)
        {
            SqlCommand cmd, cmd2;
            SqlDataReader dataReader, dataReader2;
            List<OrderWebModel> _orders = new List<OrderWebModel>();
            OrderWebDocumentsModel DocModel;
            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAllOrders_Web", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CultureId", filter.cultureId);
                cmd.Parameters.AddWithValue("@ApplicationId", filter.ApplicationId);
                cmd.Parameters.AddWithValue("@TranslationType", filter.translationType);
                cmd.Parameters.AddWithValue("@OrderId", filter.orderId);
                cmd.Parameters.AddWithValue("@OrderNo", filter.orderNo);
                cmd.Parameters.AddWithValue("@SourceLangID", filter.srcLangId);
                cmd.Parameters.AddWithValue("@TargetLangID", filter.trgLangId);
                cmd.Parameters.AddWithValue("@SpecialFieldID", filter.specialFieldId);
                cmd.Parameters.AddWithValue("@ClientID", filter.clientId);
                cmd.Parameters.AddWithValue("@TranslatorID", filter.translatorId);
                cmd.Parameters.AddWithValue("@OrderStatus", filter.orderStatus);
                cmd.Parameters.AddWithValue("@StartDate", filter.startDate);
                cmd.Parameters.AddWithValue("@EndDate", filter.endDate);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    OrderWebModel order = new OrderWebModel();
                    order.ID = Guid.Parse(dataReader["ID"].ToString());
                    order.DeliveryPlanID = Convert.ToInt64(dataReader["DeliveryPlanID"].ToString());
                    order.ClientID = Guid.Parse(dataReader["ClientId"].ToString());
                    order.ClientNo = Convert.ToInt64(dataReader["ClientNo"].ToString());
                    order.ApplicationName = dataReader["ApplicationName"].ToString();
                    order.AssignedTranslatorID = dataReader["AssignedTranslatorID"] == DBNull.Value ? (Guid?)null : Guid.Parse(dataReader["AssignedTranslatorID"].ToString());
                    order.CommentToTranslator = dataReader["CommentToTranslator"].ToString();
                    order.DeliveryComment = dataReader["DeliveryComment"].ToString();
                    order.CompanyNotes = dataReader["CompanyNotes"].ToString();
                    order.CommentToBcause = dataReader["CommentToBcause"].ToString();
                    order.CurrencyCode = dataReader["CurrencyCode"].ToString();
                    order.CurrencySymbol = dataReader["CurrencySymbol"].ToString();
                    order.CurrencyName = dataReader["CurrencyName"].ToString();
                    order.CompletionDate = dataReader["CompletionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["CompletionDate"]);                  
                    order.CurrencyID = Convert.ToInt64(dataReader["CurrencyID"].ToString());
                    order.DeliveryDate = dataReader["DeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["DeliveryDate"]);
                    order.DeliveryLevelName = dataReader["DeliveryLevelName"].ToString();
                    order.StartDate = dataReader["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["StartDate"]);
                    order.EndDate = dataReader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["EndDate"]);
                    order.RequestDate = dataReader["RequestDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["RequestDate"]);
                    order.OrderStatus = dataReader["OrderStatus"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["OrderStatus"]);
                    order.DeliveryPlan = dataReader["DeliveryPlan"].ToString();
                    order.InvoiceNo = dataReader["InvoiceNo"].ToString();
                    order.TranslationType = Convert.ToInt32(dataReader["TranslationType"].ToString());
                    order.PaymentAmount = Convert.ToDecimal(dataReader["PaymentAmount"].ToString());
                    order.TranslatorFee = Convert.ToDecimal(dataReader["TranslatorFee"].ToString());
                    order.EstimatedPrice = Convert.ToDecimal(dataReader["EstimatedPrice"].ToString());
                    order.UnitPrice = Convert.ToDecimal(dataReader["UnitPrice"].ToString());
                    order.Discount = Convert.ToDecimal(dataReader["Discount"].ToString());
                    order.PriceAfterDiscount = Convert.ToDecimal(dataReader["PriceAfterDiscount"].ToString());
                    order.ConsumptionTax = Convert.ToDecimal(dataReader["ConsumptionTax"].ToString());
                    order.OrderDate = dataReader["OrderDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["OrderDate"]);
                    order.OrderId = Convert.ToInt64(dataReader["OrderID"].ToString());
                    order.OrderNo = dataReader["OrderNo"].ToString();
                    order.TranslatorNo = dataReader["TranslatorNo"] == DBNull.Value ? (long?)null : Convert.ToInt64(dataReader["TranslatorNo"].ToString());
                    order.OrderStatusName = dataReader["OrderStatusName"].ToString();
                    order.TranslatorName = dataReader["FirstName"].ToString()+" "+ dataReader["MiddleName"].ToString()+" "+ dataReader["LastName"].ToString();
                    order.MenuScript = dataReader["MenuScript"].ToString();
                    order.TranslatorFee = Convert.ToDecimal(dataReader["TranslatorFee"].ToString());
                    order.WordCount = Convert.ToInt64(dataReader["WordCount"].ToString());
                    order.CountType = dataReader["CountType"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["CountType"].ToString());
                    order.TargetLanguageID = Guid.Parse(dataReader["TargetLanguageID"].ToString());
                    order.SourceLanguageID = Guid.Parse(dataReader["SourceLanguageID"].ToString());
                    order.TranslationFieldID = Guid.Parse(dataReader["TranslationFieldID"].ToString());
                    order.TargetLanguage = dataReader["TargetLanguage"].ToString();
                    order.SourceLanguage = dataReader["SourceLanguage"].ToString();
                    order.EvaluationScore = Convert.ToDecimal(dataReader["EvaluationScore"].ToString());
                    order.EvaluationComment = dataReader["EvaluationComment"].ToString();
                    order.TranslationFieldName = dataReader["TranslationFieldName"].ToString();
                    order.PaymentDate = dataReader["PaymentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["PaymentDate"].ToString());
                    
                    cmd2 = new SqlCommand("SP_GetOrderWebDocuments", _sqlConnService.CreateConnection());
                    order.WebDocumentList = new List<OrderWebDocumentsModel>();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@OrderID", order.ID);
                    dataReader2 = cmd2.ExecuteReader();
                    while (dataReader2.Read())
                    {
                        DocModel = new OrderWebDocumentsModel();
                        DocModel.ID = Guid.Parse(dataReader2["ID"].ToString());
                        DocModel.OrderID = Guid.Parse(dataReader2["OrderID"].ToString());
                        DocModel.FileName = dataReader2["Name"].ToString();
                        DocModel.FileDescription = dataReader2["FileDescription"].ToString();
                        DocModel.DownloadURL = dataReader2["DownloadURL"].ToString();
                        DocModel.UploadDate = dataReader2["UploadDate"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataReader2["UploadDate"].ToString());
                        DocModel.WordCount = Convert.ToInt64(dataReader2["WordCount"].ToString());
                        DocModel.OriginalFileName = dataReader2["OriginalName"].ToString();
                        DocModel.Extension = dataReader2["Extension"].ToString();
                        DocModel.FileSize = Convert.ToInt64(dataReader2["FileSize"].ToString());
                        order.WebDocumentList.Add(DocModel);
                    }
                    dataReader2.Close();
                    //_orders.Add(order);
                    order.MessageList = GetAllMessageList(order);
                    _orders.Add(order);
                }
                dataReader.Close();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            
            return _orders;
        }

        public List<MessageModel> GetAllMessageList(OrderWebModel model)
        {
            List<MessageModel> datalist = new List<MessageModel>();
            List<Message> masterdatalist =new List<Message>();
            MessageModel template;
            masterdatalist = _dbContext.Messages.ToList();
            foreach(Message record in masterdatalist)
            {
                if(record.SenderID==model.ClientID)
                {
                    template = Mapper.Map<Message, MessageModel>(record);
                    datalist.Add(template);
                }
            }

            return datalist;

        }
        public List<OrderWebModel> GetAppointedOrderList(OrderFilter filter)
        {
            SqlCommand cmd, cmd2;
            SqlDataReader dataReader, dataReader2;
            List<OrderWebModel> _orders = new List<OrderWebModel>();
            OrderWebDocumentsModel DocModel;
            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAppointedOrders_Web", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CultureId", filter.cultureId);
                cmd.Parameters.AddWithValue("@ApplicationId", filter.ApplicationId);
                cmd.Parameters.AddWithValue("@TranslatorID", filter.translatorId);
                cmd.CommandType = CommandType.StoredProcedure;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    OrderWebModel order = new OrderWebModel();
                    order.ID = Guid.Parse(dataReader["ID"].ToString());
                    order.DeliveryPlanID = Convert.ToInt64(dataReader["DeliveryPlanID"].ToString());
                    order.ClientID = Guid.Parse(dataReader["ClientId"].ToString());
                    order.ClientNo = Convert.ToInt64(dataReader["ClientNo"].ToString());
                    order.ApplicationName = dataReader["ApplicationName"].ToString();
                    order.AssignedTranslatorID = dataReader["AssignedTranslatorID"] == DBNull.Value ? (Guid?)null : Guid.Parse(dataReader["AssignedTranslatorID"].ToString());
                    order.CommentToTranslator = dataReader["CommentToTranslator"].ToString();
                    order.DeliveryComment = dataReader["DeliveryComment"].ToString();
                    order.CompanyNotes = dataReader["CompanyNotes"].ToString();
                    order.CommentToBcause = dataReader["CommentToBcause"].ToString();
                    order.CurrencySymbol = dataReader["CurrencySymbol"].ToString();
                    order.CurrencyCode = dataReader["CurrencyCode"].ToString();
                    order.CompletionDate = dataReader["CompletionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["CompletionDate"]);
                    order.OrderStatus = dataReader["OrderStatus"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["OrderStatus"]);
                    order.CurrencyID = Convert.ToInt64(dataReader["CurrencyID"].ToString());
                    order.DeliveryDate = dataReader["DeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["DeliveryDate"]);
                    order.DeliveryLevelName = dataReader["DeliveryLevelName"].ToString();
                    order.StartDate = dataReader["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["StartDate"]);
                    order.EndDate = dataReader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["EndDate"]);
                    order.RequestDate = dataReader["RequestDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["RequestDate"]);
                    order.DeliveryPlan = dataReader["DeliveryPlan"].ToString();
                    order.InvoiceNo = dataReader["InvoiceNo"].ToString();
                    order.TranslationType = Convert.ToInt32(dataReader["TranslationType"].ToString());
                    order.PaymentAmount = Convert.ToDecimal(dataReader["PaymentAmount"].ToString());
                    order.UnitPrice = Convert.ToDecimal(dataReader["UnitPrice"].ToString());
                    order.Discount = Convert.ToDecimal(dataReader["Discount"].ToString());
                    order.PriceAfterDiscount = Convert.ToDecimal(dataReader["PriceAfterDiscount"].ToString());
                    order.ConsumptionTax = Convert.ToDecimal(dataReader["ConsumptionTax"].ToString());
                    order.OrderDate = dataReader["OrderDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["OrderDate"]);
                    order.OrderId = Convert.ToInt64(dataReader["OrderID"].ToString());
                    order.OrderNo = dataReader["OrderNo"].ToString();
                    order.OrderStatusName = dataReader["OrderStatusName"].ToString();
                    order.TranslatorName = dataReader["FirstName"].ToString() + " " + dataReader["MiddleName"].ToString() + " " + dataReader["LastName"].ToString();
                    order.TranslatorNo = dataReader["TranslatorNo"] == DBNull.Value ? (long?)null : Convert.ToInt64(dataReader["TranslatorNo"].ToString());
                    order.MenuScript = dataReader["MenuScript"].ToString();
                    order.TranslatorFee = Convert.ToDecimal(dataReader["TranslatorFee"].ToString());
                    order.WordCount = Convert.ToInt64(dataReader["WordCount"].ToString());
                    order.CountType = dataReader["CountType"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["CountType"].ToString());
                    order.TargetLanguageID = Guid.Parse(dataReader["TargetLanguageID"].ToString());
                    order.SourceLanguageID = Guid.Parse(dataReader["SourceLanguageID"].ToString());
                    order.TranslationFieldID = Guid.Parse(dataReader["TranslationFieldID"].ToString());
                    order.TargetLanguage = dataReader["TargetLanguage"].ToString();
                    order.SourceLanguage = dataReader["SourceLanguage"].ToString();
                    order.EvaluationScore = Convert.ToDecimal(dataReader["EvaluationScore"].ToString());
                    order.EvaluationComment = dataReader["EvaluationComment"].ToString();
                    order.TranslationFieldName = dataReader["TranslationFieldName"].ToString();
                    order.PaymentDate = dataReader["PaymentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["PaymentDate"].ToString());

                    order.WebDocumentList = new List<OrderWebDocumentsModel>();
                    cmd2 = new SqlCommand("SP_GetOrderWebDocuments", _sqlConnService.CreateConnection());
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@OrderID", order.ID);
                    dataReader2 = cmd2.ExecuteReader();
                    while (dataReader2.Read())
                    {
                        DocModel = new OrderWebDocumentsModel();
                        DocModel.ID = Guid.Parse(dataReader2["ID"].ToString());
                        DocModel.OrderID = Guid.Parse(dataReader2["OrderID"].ToString());
                        DocModel.FileName = dataReader2["Name"].ToString();
                        DocModel.FileDescription = dataReader2["FileDescription"].ToString();
                        DocModel.DownloadURL = dataReader2["DownloadURL"].ToString();
                        DocModel.UploadDate = dataReader2["UploadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader2["UploadDate"].ToString());
                        DocModel.WordCount = Convert.ToInt64(dataReader2["WordCount"].ToString());
                        DocModel.OriginalFileName = dataReader2["OriginalName"].ToString();
                        DocModel.Extension = dataReader2["Extension"].ToString();
                        DocModel.FileSize = Convert.ToInt64(dataReader2["FileSize"].ToString());
                        order.WebDocumentList.Add(DocModel);
                    }
                    dataReader2.Close();
                    _orders.Add(order);
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return _orders;
        }

        public string SaveWebOrder(OrderWebModel model)
        {
            bool IsSuccessful;
            SqlCommand cmd, cmd2;
            string StatementType;
            string OrderNo = null;
            SqlParameter ReturnParameter;

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_SaveDeleteOrderWeb", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                ReturnParameter = cmd.Parameters.Add("@ORDER_NO", SqlDbType.NVarChar, 250);
                ReturnParameter.Direction = ParameterDirection.Output;

                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@InvoiceNo", model.InvoiceNo);
                cmd.Parameters.AddWithValue("@SourceLanguageID", model.SourceLanguageID);
                cmd.Parameters.AddWithValue("@TargetLanguageID", model.TargetLanguageID);
                cmd.Parameters.AddWithValue("@TranslationFieldID", model.TranslationFieldID);
                cmd.Parameters.AddWithValue("@AssignedTranslatorID", model.AssignedTranslatorID);
                cmd.Parameters.AddWithValue("@ClientID", model.ClientID);
                cmd.Parameters.AddWithValue("@DeliveryPlanID", model.DeliveryPlanID);
                cmd.Parameters.AddWithValue("@DeliveryPlan", model.DeliveryPlan);
                cmd.Parameters.AddWithValue("@DeliveryLevelName", model.DeliveryLevelName);
                cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                cmd.Parameters.AddWithValue("@IntroducerID", model.IntroducerID);
                cmd.Parameters.AddWithValue("@TranslationType", model.TranslationType);
                cmd.Parameters.AddWithValue("@OrderDate", model.OrderDate);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@DeliveryDate", model.DeliveryDate);
                cmd.Parameters.AddWithValue("@CompletionDate", model.CompletionDate);
                cmd.Parameters.AddWithValue("@OrderStatus", model.OrderStatus);
                cmd.Parameters.AddWithValue("@PaymentStatus", model.PaymentStatus);
                cmd.Parameters.AddWithValue("@PaymentMethod", model.PaymentMethod);
                cmd.Parameters.AddWithValue("@WordCount", model.WordCount);
                cmd.Parameters.AddWithValue("@CountType", model.CountType);
                cmd.Parameters.AddWithValue("@PaymentAmount", model.PaymentAmount);
                cmd.Parameters.AddWithValue("@TranslatorFee", model.TranslatorFee);
                cmd.Parameters.AddWithValue("@EstimatedPrice", model.EstimatedPrice);
                cmd.Parameters.AddWithValue("@UnitPrice", model.UnitPrice);
                cmd.Parameters.AddWithValue("@Discount", model.Discount);
                cmd.Parameters.AddWithValue("@PriceAfterDiscount", model.PriceAfterDiscount);
                cmd.Parameters.AddWithValue("@ConsumptionTax", model.ConsumptionTax);
                cmd.Parameters.AddWithValue("@RequestDate", model.RequestDate);
                cmd.Parameters.AddWithValue("@PaymentDate", model.PaymentDate);
                cmd.Parameters.AddWithValue("@CommentToTranslator", model.CommentToTranslator);
                cmd.Parameters.AddWithValue("@MenuScript", model.MenuScript);
                cmd.Parameters.AddWithValue("@DeliveryComment", model.DeliveryComment);
                cmd.Parameters.AddWithValue("@CompanyNotes", model.CompanyNotes);
                cmd.Parameters.AddWithValue("@CommentToBcause", model.CommentToBcause);
                cmd.Parameters.AddWithValue("@ReferenceFileName", model.ReferenceFileName);
                cmd.Parameters.AddWithValue("@ReferenceDownloadURL", model.ReferenceDownloadURL);

                if (model.ID == Guid.Empty)
                {
                    model.ID = Guid.NewGuid();
                    StatementType = "Insert";
                }
                else
                {
                    StatementType = "Update";
                }
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                cmd.ExecuteNonQuery();
                OrderNo = ReturnParameter.Value.ToString();

                foreach (OrderWebDocumentsModel DocModel in model.WebDocumentList)
                {
                    cmd2 = new SqlCommand("SP_SaveDeleteOrderWebDocuments", _sqlConnService.CreateConnection());
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@OrderID", model.ID);
                    cmd2.Parameters.AddWithValue("@Name", DocModel.FileName);
                    cmd2.Parameters.AddWithValue("@FileDescription", DocModel.FileDescription);
                    cmd2.Parameters.AddWithValue("@DownloadURL", DocModel.DownloadURL);
                    cmd2.Parameters.AddWithValue("@UploadDate", DocModel.UploadDate);
                    cmd2.Parameters.AddWithValue("@WordCount", DocModel.WordCount);
                    cmd2.Parameters.AddWithValue("@OriginalName", DocModel.OriginalFileName);
                    cmd2.Parameters.AddWithValue("@Extension", DocModel.Extension);
                    cmd2.Parameters.AddWithValue("@FileSize", DocModel.FileSize);
                    if (DocModel.ID == Guid.Empty)
                    {
                        DocModel.ID = Guid.NewGuid();
                        StatementType = "Insert";
                    }
                    else
                    {
                        StatementType = "Update";
                    }
                    cmd2.Parameters.AddWithValue("@ID", DocModel.ID);
                    cmd2.Parameters.AddWithValue("@StatementType", StatementType);
                    cmd2.ExecuteNonQuery();
                }
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
            return OrderNo;
        }

        public OrderWebModel OrderOperation(OrderWebModel model, string type)
        {
            SqlCommand cmd;
            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_OrderOperation_Translator", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@TranslatorId", model.AssignedTranslatorID);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@StatusId", model.OrderStatus);
                cmd.Parameters.AddWithValue("@DeliveryDate", model.DeliveryDate);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return model;
        }

        public virtual void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
            return;
        }
    }
}

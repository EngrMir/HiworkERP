using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using HiWork.DAL.Repositories;
using System.Globalization;
using HiWork.Utils.Infrastructure.Contract;
using AutoMapper;

namespace HiWork.BLL.Services
{
   public partial interface IShortTermEstimationService
    {
        bool SaveEstimation(EstimationModel model);
        List<EstimationModel> GetAllEstimationList(BaseViewModel model);
        List<EstimationModel> DeleteEstimation(EstimationModel model);
        List<TranslationCertificateSettingsModel> GetTranslationCertificateSettingsList(BaseViewModel model);
        List<EstimationDetailsModel> GetEstimationDetailsListByID(BaseViewModel model, string EstimationID);
        List<EstimationFileModel> GetEstimationFilesListByID(BaseViewModel model, string EstimationID, string EstimationDetailsID);
        bool SaveEstimationProject(EstimationProjectModel model);
        string GetEstimationProjectNextNumber(BaseViewModel model);
    }

    public class ShortTermEstimationService : IShortTermEstimationService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public ShortTermEstimationService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
        }

        public List<TranslationCertificateSettingsModel> GetTranslationCertificateSettingsList(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            TranslationCertificateSettingsModel SettingsModel;
            List<TranslationCertificateSettingsModel> SettingsList = new List<TranslationCertificateSettingsModel>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAllTranslationCertificateSettings", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    SettingsModel = new TranslationCertificateSettingsModel();
                    SettingsModel.ID = Convert.ToInt64(DataReader["ID"].ToString());
                    SettingsModel.CertificateType = (CertificateType)Convert.ToInt32(DataReader["CertificateType"].ToString());
                    SettingsModel.UnitPrice = Convert.ToDecimal(DataReader["UnitPrice"].ToString());
                    SettingsList.Add(SettingsModel);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return SettingsList;
        }


        /* Function to retrive a list of EstimationDetails by Estimaton ID */
        /* Programmed by Ashis Kr. Das */
        public List<EstimationDetailsModel> GetEstimationDetailsListByID(BaseViewModel model, string EstimationID)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            EstimationDetailsModel DetailsModel;
            List<EstimationDetailsModel> DataList = new List<EstimationDetailsModel>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAllEstimationDetails", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimationID", Guid.Parse(EstimationID));
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    DetailsModel = new EstimationDetailsModel();
                    DetailsModel.ID = Guid.Parse(DataReader["ID"].ToString());
                    DetailsModel.EstimationID = Guid.Parse(DataReader["EstimationID"].ToString());
                    DetailsModel.SourceLanguageID = Guid.Parse(DataReader["SourceLanguageID"].ToString());
                    DetailsModel.TargetLanguageID = Guid.Parse(DataReader["TargetLanguageID"].ToString());
                    DetailsModel.ServiceTypeID = Guid.Parse(DataReader["ServiceType"].ToString());
                    DetailsModel.UnitPrice1 = Convert.ToDecimal(DataReader["UnitPrice1"].ToString());
                    DetailsModel.PageCount1 = Convert.ToInt32(DataReader["PageCount1"].ToString());
                    DetailsModel.Discount1 = Convert.ToDecimal(DataReader["Discount1"].ToString());
                    DetailsModel.UnitPrice2 = Convert.ToDecimal(DataReader["UnitPrice2"].ToString());
                    DetailsModel.PageCount2 = Convert.ToInt32(DataReader["PageCount2"].ToString());
                    DetailsModel.Discount2 = Convert.ToDecimal(DataReader["Discount2"].ToString());
                    DetailsModel.UnitPrice3 = Convert.ToDecimal(DataReader["UnitPrice3"].ToString());
                    DetailsModel.PageCount3 = Convert.ToInt32(DataReader["PageCount3"].ToString());
                    DetailsModel.Discount3 = Convert.ToDecimal(DataReader["Discount3"].ToString());
                    DetailsModel.UnitPrice4 = Convert.ToDecimal(DataReader["UnitPrice4"].ToString());
                    DetailsModel.PageCount4 = Convert.ToInt32(DataReader["PageCount4"].ToString());
                    DetailsModel.Discount4 = Convert.ToDecimal(DataReader["Discount4"].ToString());
                    DetailsModel.UnitPrice5 = Convert.ToDecimal(DataReader["UnitPrice5"].ToString());
                    DetailsModel.PageCount5 = Convert.ToInt32(DataReader["PageCount5"].ToString());
                    DetailsModel.Discount5 = Convert.ToDecimal(DataReader["Discount5"].ToString());
                    DetailsModel.BasicTime = Convert.ToInt32(DataReader["BasicTime"].ToString());
                    DetailsModel.AdditionalBasicAmount = Convert.ToDecimal(DataReader["AdditionalBasicAmount"].ToString());
                    DetailsModel.ExtraTime = Convert.ToInt32(DataReader["ExtraTime"].ToString());
                    //DetailsModel.LastnightTime = Convert.ToInt32(DataReader["LastnightTime"].ToString());
                    //DetailsModel.MovingTime = Convert.ToInt32(DataReader["MovingTime"].ToString());
                    DetailsModel.BasicAmount = Convert.ToDecimal(DataReader["BasicAmount"].ToString());
                    DetailsModel.ExtensionAmount = Convert.ToDecimal(DataReader["ExtensionAmount"].ToString());
                    DetailsModel.ExtraAmount = Convert.ToDecimal(DataReader["ExtraAmount"].ToString());
                    DetailsModel.LateAtNightAmount = Convert.ToDecimal(DataReader["LateAtNightAmount"].ToString());
                    //DetailsModel.MovingAmount = Convert.ToDecimal(DataReader["MovingAmount"].ToString());
                    DetailsModel.NumberOfDays = Convert.ToInt32(DataReader["NumberOfDays"].ToString());
                    DetailsModel.NumberOfPeople = Convert.ToInt32(DataReader["NumberOfPeople"].ToString());
                    DetailsModel.OtherAmount = Convert.ToDecimal(DataReader["OtherAmount"].ToString());
                    DetailsModel.CertificateAmount = Convert.ToDecimal(DataReader["CertificateAmount"].ToString());
                    DetailsModel.ExcludeTax = Convert.ToBoolean(DataReader["ExcludeTax"].ToString());
                    DetailsModel.Contents = DataReader["Contents"].ToString();
                    DetailsModel.LengthMinute = Convert.ToDecimal(DataReader["LengthMinute"].ToString());
                    DetailsModel.WithTranslation = Convert.ToBoolean(DataReader["WithTranslation"].ToString());
                    DataList.Add(DetailsModel);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return DataList;
        }


        /* Function to retrive a list of EstimationFiles by Estimaton ID in conjunction with EstimationDetails ID */
        /* Programmed by Ashis Kr. Das */
        public List<EstimationFileModel> GetEstimationFilesListByID(BaseViewModel model, string EstimationID, string EstimationDetailsID)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            EstimationFileModel FileModel;
            List<EstimationFileModel> FileList = new List<EstimationFileModel>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAllEstimationFiles", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateID", Guid.Parse(EstimationID));
                cmd.Parameters.AddWithValue("@EstimateDetailsID", Guid.Parse(EstimationDetailsID));
                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    FileModel = new EstimationFileModel();
                    FileModel.ID = Guid.Parse(DataReader["ID"].ToString());
                    FileModel.EstimateID = Guid.Parse(DataReader["EstimateID"].ToString());
                    FileModel.EstimateDetailsID = Guid.Parse(DataReader["EstimateDetailsID"].ToString());
                    FileModel.FileName = DataReader["FileName"].ToString();
                    FileModel.DownloadURL = DataReader["DownloadURL"].ToString();
                    FileModel.TranslationText = DataReader["TranslationText"].ToString();
                    FileModel.WordCount = Convert.ToInt32(DataReader["WordCount"].ToString());
                    FileList.Add(FileModel);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return FileList;
        }

        private string GenerateEstimationNumber(EstimationModel model)
        {
            string NextRegistrationID;
            string AppCode;
            IApplicationService appService = new ApplicationService(new ApplicationRepository(new UnitOfWork()));

            NextRegistrationID = GetNextRegistrationID(model);
            AppCode = appService.GetApplicationCode(model.ApplicationId);
            return Helper.GenerateUniqueID(AppCode, NextRegistrationID);
        }


        /* Written by Ashis Kr. Das */
        public bool SaveEstimation(EstimationModel model)
        {
            SqlCommand cmd, cmd2, cmd3;
            bool isSuccessful = false;
            Guid EstimationID;

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_SaveEstimation", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                string EstimationNumber = GenerateEstimationNumber(model);
                cmd.Parameters.AddWithValue("@InquiryDate", model.InquiryDate);
                cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@EstimationRouteID", model.EstimateRouteID);
                cmd.Parameters.AddWithValue("@OutwardSalesID", model.OutwardSalesID);
                cmd.Parameters.AddWithValue("@LargeSalesID", model.LargeSalesID);
                cmd.Parameters.AddWithValue("@SalesPersonID", model.SalesPersonID);
                cmd.Parameters.AddWithValue("@AssistantID", model.AssistantID);
                cmd.Parameters.AddWithValue("@CoordinatorID", model.CoordinatorID);
                cmd.Parameters.AddWithValue("@ApprovalID", model.ApprovalID);
                cmd.Parameters.AddWithValue("@ClientID", model.ClientID);
                cmd.Parameters.AddWithValue("@TradingID", model.TradingID);
                cmd.Parameters.AddWithValue("@TeamID", model.TeamID);
                cmd.Parameters.AddWithValue("@IsCompanyPrivate", model.IsCompanyPrivate);
                cmd.Parameters.AddWithValue("@ClientStatus", model.ClientStatus);
                cmd.Parameters.AddWithValue("@SubSpecializedFieldID", model.SubSpecializedFieldID);
                cmd.Parameters.AddWithValue("@BillingCompanyName_" + model.CurrentCulture, model.BillingCompanyName);
                cmd.Parameters.AddWithValue("@BillingTo", model.BillingTo);
                cmd.Parameters.AddWithValue("@BillingEmailCC", model.BillingEmailCC);
                cmd.Parameters.AddWithValue("@BillingAddress_" + model.CurrentCulture, model.BillingAddress);
                cmd.Parameters.AddWithValue("@BillingContactNo", model.BillingContactNo);
                cmd.Parameters.AddWithValue("@BillingFax", model.BillingFax);
                cmd.Parameters.AddWithValue("@IsPostingBill", model.IsPostingBill);
                cmd.Parameters.AddWithValue("@PaymentTerms", model.PaymentTerms);
                cmd.Parameters.AddWithValue("@DeliveryCompanyName_" + model.CurrentCulture, model.DeliveryCompanyName);
                cmd.Parameters.AddWithValue("@DeliveryTo", model.DeliveryTo);
                cmd.Parameters.AddWithValue("@DeliveryEmailCC", model.DeliveryEmailCC);
                cmd.Parameters.AddWithValue("@DeliveryAddress_" + model.CurrentCulture, model.DeliveryAddress);
                cmd.Parameters.AddWithValue("@DeliveryContactNo", model.DeliveryContactNo);
                cmd.Parameters.AddWithValue("@DeliveryFax", model.DeliveryFax);
                cmd.Parameters.AddWithValue("@DeliveryInstruction", model.DeliveryInstruction);
                cmd.Parameters.AddWithValue("@RemarksCoordinatorType", model.RemarksCoordinatorType);
                cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                cmd.Parameters.AddWithValue("@IsProspect", model.IsProspect);
                cmd.Parameters.AddWithValue("@IsUndisclosed", model.IsUndisclosed);
                cmd.Parameters.AddWithValue("@EstimationType", EstimationType.ShortTermDispatch);
                cmd.Parameters.AddWithValue("@EstimationStatus", model.EstimationStatus);
                cmd.Parameters.AddWithValue("@SpecializedFieldID", model.SpecializedFieldID);
                cmd.Parameters.AddWithValue("@IsInternalPurpose", model.IsInternalPurpose);
                cmd.Parameters.AddWithValue("@IsExternalPurpose", model.IsExternalPurpose);
                cmd.Parameters.AddWithValue("@IsPrintPurpose", model.IsPrintPurpose);
                cmd.Parameters.AddWithValue("@IsWebPurpose", model.IsWebPurpose);
                cmd.Parameters.AddWithValue("@IsOtherPurpose", model.IsOtherPurpose);
                cmd.Parameters.AddWithValue("@OtherPurposeText", model.OtherPurposeText);
                cmd.Parameters.AddWithValue("@PriorityQuality", model.PriorityQuality);
                cmd.Parameters.AddWithValue("@PriorityPrice", model.PriorityPrice);
                cmd.Parameters.AddWithValue("@PriorityDelivery", model.PriorityDelivery);
                cmd.Parameters.AddWithValue("@PriorityTender", model.PriorityTender);
                cmd.Parameters.AddWithValue("@KnownByGoogle", model.KnownByGoogle);
                cmd.Parameters.AddWithValue("@KnownByYahoo", model.KnownByYahoo);
                cmd.Parameters.AddWithValue("@KnownByEmail", model.KnownByEmail);
                cmd.Parameters.AddWithValue("@KnownByOthers", model.KnownByOthers);
                cmd.Parameters.AddWithValue("@KnownOtherText", model.KnownOtherText);
                cmd.Parameters.AddWithValue("@FinalDeliveryDate", model.FinalDeliveryDate);
                cmd.Parameters.AddWithValue("@FirstDeliveryDate", model.FirstDeliveryDate);
                cmd.Parameters.AddWithValue("@CoordinatorNotes_" + model.CurrentCulture, model.CoordinatorNotes);
                cmd.Parameters.AddWithValue("@Remarks_" + model.CurrentCulture, model.CurrentCulture);
                cmd.Parameters.AddWithValue("@IsRemarksHideInPDF", model.IsRemarksHideInPDF);
                cmd.Parameters.AddWithValue("@IsOrderReceived", model.IsOrderReceived);
                cmd.Parameters.AddWithValue("@DiscountTotal", model.DiscountTotal);
                cmd.Parameters.AddWithValue("@AverageUnitPrice", model.AverageUnitPrice);
                cmd.Parameters.AddWithValue("@ActualUnitPrice", model.ActualUnitPrice);
                cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
                cmd.Parameters.AddWithValue("@PurposeDetails", model.PurposeDetails);
                cmd.Parameters.AddWithValue("@OrderTitle", model.OrderTitle);
                cmd.Parameters.AddWithValue("@IssuedByTranslator", model.IssuedByTranslator);
                cmd.Parameters.AddWithValue("@IssuedByCompany", model.IssuedByCompany);
                cmd.Parameters.AddWithValue("@PriceCertification", model.PriceCertification);
                cmd.Parameters.AddWithValue("@OtherItemName_" + model.CurrentCulture, model.OtherItemName);
                cmd.Parameters.AddWithValue("@OtherItemUnitPrice", model.OtherItemUnitPrice);
                cmd.Parameters.AddWithValue("@OtherItemNumber", model.OtherItemNumber);
                cmd.Parameters.AddWithValue("@OtherAmount", model.OtherAmount);
                cmd.Parameters.AddWithValue("@TaxEstimation", model.TaxEstimation);
                cmd.Parameters.AddWithValue("@QuotationInclTax", model.QuotationInclTax);
                cmd.Parameters.AddWithValue("@QuotationExclTax", model.QuotationExclTax);
                cmd.Parameters.AddWithValue("@ConsumptionOnTax", model.ConsumptionOnTax);
                cmd.Parameters.AddWithValue("@ExcludedTaxCost", model.ExcludedTaxCost);
                cmd.Parameters.AddWithValue("@IsCampaign", model.IsCampaign);
                cmd.Parameters.AddWithValue("@IsSpecialPrice", model.IsSpecialPrice);
                cmd.Parameters.AddWithValue("@IsSpecialDeal", model.IsSpecialDeal);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CurrentUserID);

                if (model.ID == Guid.Empty)
                {
                    EstimationID = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", EstimationID);
                    cmd.Parameters.AddWithValue("@EstimationNo", EstimationNumber);
                    cmd.Parameters.AddWithValue("@StatementType", "Insert");
                }
                else
                {
                    EstimationID = model.ID;
                    cmd.Parameters.AddWithValue("@ID", EstimationID);
                    cmd.Parameters.AddWithValue("@EstimationNo", model.EstimationNo);
                    cmd.Parameters.AddWithValue("@StatementType", "Update");
                }
                cmd.ExecuteNonQuery();              // Run stored procedure to store Estimation on database

                foreach (EstimationDetailsModel DetailsModel in model.EstimationItems)
                {
                    Guid EstimationDetailsID;

                    cmd2 = new SqlCommand("SP_SaveEstimationDetails", _sqlConnService.CreateConnection());
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@EstimationID", EstimationID);
                    cmd2.Parameters.AddWithValue("@SourceLanguageID", DetailsModel.SourceLanguageID);
                    cmd2.Parameters.AddWithValue("@TargetLanguageID", DetailsModel.TargetLanguageID);
                    cmd2.Parameters.AddWithValue("@ServiceType", DetailsModel.ServiceTypeID);
                    cmd2.Parameters.AddWithValue("@UnitPrice1", DetailsModel.UnitPrice1);
                    cmd2.Parameters.AddWithValue("@UnitPrice2", DetailsModel.UnitPrice2);
                    cmd2.Parameters.AddWithValue("@UnitPrice3", DetailsModel.UnitPrice3);
                    cmd2.Parameters.AddWithValue("@UnitPrice4", DetailsModel.UnitPrice4);
                    cmd2.Parameters.AddWithValue("@UnitPrice5", DetailsModel.UnitPrice5);
                    cmd2.Parameters.AddWithValue("@Discount1", DetailsModel.Discount1);
                    cmd2.Parameters.AddWithValue("@Discount2", DetailsModel.Discount2);
                    cmd2.Parameters.AddWithValue("@Discount3", DetailsModel.Discount3);
                    cmd2.Parameters.AddWithValue("@Discount4", DetailsModel.Discount4);
                    cmd2.Parameters.AddWithValue("@Discount5", DetailsModel.Discount5);
                    cmd2.Parameters.AddWithValue("@PageCount1", DetailsModel.PageCount1);
                    cmd2.Parameters.AddWithValue("@PageCount2", DetailsModel.PageCount2);
                    cmd2.Parameters.AddWithValue("@PageCount3", DetailsModel.PageCount3);
                    cmd2.Parameters.AddWithValue("@PageCount4", DetailsModel.PageCount4);
                    cmd2.Parameters.AddWithValue("@PageCount5", DetailsModel.PageCount5);
                    cmd2.Parameters.AddWithValue("@BasicTime", DetailsModel.BasicTime);
                    cmd2.Parameters.AddWithValue("@AdditionalBasicAmount", DetailsModel.AdditionalBasicAmount);
                    cmd2.Parameters.AddWithValue("@ExtraTime", DetailsModel.ExtraTime);
                    //cmd2.Parameters.AddWithValue("@LastnightTime", DetailsModel.LastnightTime);
                    //cmd2.Parameters.AddWithValue("@MovingTime", DetailsModel.MovingTime);
                    cmd2.Parameters.AddWithValue("@BasicAmount", DetailsModel.BasicAmount);
                    cmd2.Parameters.AddWithValue("@ExtensionAmount", DetailsModel.ExtensionAmount);
                    cmd2.Parameters.AddWithValue("@ExtraAmount", DetailsModel.ExtraAmount);
                    cmd2.Parameters.AddWithValue("@LateAtNightAmount", DetailsModel.LateAtNightAmount);
                    //cmd2.Parameters.AddWithValue("@MovingAmount", DetailsModel.MovingAmount);
                    cmd2.Parameters.AddWithValue("@NumberOfDays", DetailsModel.NumberOfDays);
                    cmd2.Parameters.AddWithValue("@NumberOfPeople", DetailsModel.NumberOfPeople);
                    cmd2.Parameters.AddWithValue("@OtherAmount", DetailsModel.OtherAmount);
                    cmd2.Parameters.AddWithValue("@CertificateAmount", DetailsModel.CertificateAmount);
                    cmd2.Parameters.AddWithValue("@ExcludeTax", DetailsModel.ExcludeTax);
                    cmd2.Parameters.AddWithValue("@LengthMinute", DetailsModel.LengthMinute);
                    cmd2.Parameters.AddWithValue("@WithTranslation", DetailsModel.WithTranslation);
                    cmd2.Parameters.AddWithValue("@Contents", DetailsModel.Contents);
                    if (DetailsModel.ID == Guid.Empty)
                    {
                        EstimationDetailsID = Guid.NewGuid();
                        cmd2.Parameters.AddWithValue("@ID", EstimationDetailsID);
                        cmd2.Parameters.AddWithValue("@StatementType", "Insert");
                    }
                    else
                    {
                        EstimationDetailsID = DetailsModel.ID;
                        cmd2.Parameters.AddWithValue("@ID", EstimationDetailsID);
                        cmd2.Parameters.AddWithValue("@StatementType", "Update");
                    }
                    cmd2.ExecuteNonQuery(); 
                    isSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return isSuccessful;
        }

        public bool SaveEstimationProject(EstimationProjectModel model)
        {
            bool status = false;
            try
            {
                IUnitOfWork uwork = new UnitOfWork();
                IEstimationRepository estimationRepository = new EstimationRepository(uwork);
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var mapModel = Mapper.Map<EstimationProjectModel, EstimationProject>(model);

                if (model.ID == Guid.Empty)
                {
                    mapModel.ID = Guid.NewGuid();
                    var result = estimationRepository.SaveEstimationProject(mapModel);

                    if (result != null)
                    {
                        var estimations = estimationRepository.GetSelectedEstimationList(model.EstimationList);

                        estimations.All(e => { e.ProjectID = result.ID; return true; });

                        // var dbEstimations = Mapper.Map<List<Estimation>, List<EstimationModel>>(estimations);

                        foreach (var estimation in estimations)
                        {
                            //SaveEstimation(estimation);
                            estimationRepository.UpdateEstimation(estimation);
                        }
                    }
                }
                else
                {

                }




                status = true;
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            return status;

        }

        public string GetEstimationProjectNextNumber(BaseViewModel model)
        {
            string NexrProjectNumer = string.Empty;
            try
            {
                IUnitOfWork uwork = new UnitOfWork();
                IEstimationRepository estimationRepository = new EstimationRepository(uwork);
                IApplicationService appService = new ApplicationService(new ApplicationRepository(uwork));
                //var NextID = estimationRepository.GetEstimationProjectNextNumber(model);
                var NextID = 1;
                NextID = NextID == null ? 0 : NextID;
                NextID = NextID + 1;
                NexrProjectNumer = Helper.GenerateUniqueID(appService.GetApplicationCode(model.ApplicationId), NextID.ToString());

            }
            catch (Exception ex)
            {

            }
            return NexrProjectNumer;
        }


        private string GetNextRegistrationID(EstimationModel model)
        {
            var items = (from a in _dbContext.Estimations
                         select a.RegistrationID).ToList();
            if (items != null && items.Count > 0)
            {
                return (items.Max() + 1).ToString();
            }
            else
            {
                return "1";
            }
            //Estimation LastEstimation;
            //LastEstimation = _dbContext.Estimations.Where(es => es.ApplicationID == model.ApplicationId).LastOrDefault();
            //return (LastEstimation.RegistrationID + 1).ToString();
        }


        /* Written by Ashis Kr. Das */
        public List<EstimationModel> GetAllEstimationList(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            EstimationModel Estimation;
            List<EstimationModel> EstimationList = new List<EstimationModel>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAllEstimation", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@EstimationType", DBNull.Value);
                cmd.Parameters.AddWithValue("@EstimationNo", DBNull.Value);
                cmd.Parameters.AddWithValue("@ClientID", DBNull.Value);
                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    Estimation = new EstimationModel();
                    Estimation.ID = Guid.Parse(DataReader["ID"].ToString());
                    Estimation.RegistrationID = Convert.ToInt64(DataReader["RegistrationID"].ToString());
                    Estimation.EstimationNo = DataReader["EstimationNo"].ToString();
                    Estimation.InquiryDate = Convert.ToDateTime(DataReader["InquiryDate"].ToString());
                    Estimation.RegistrationDate = Convert.ToDateTime(DataReader["RegistrationDate"].ToString());
                    Estimation.ApplicationId = Convert.ToInt32(DataReader["ApplicationID"].ToString());
                   
                    Estimation.EstimateRouteID = DataReader["EstimateRouteID"] != DBNull.Value ? Guid.Parse(DataReader["EstimateRouteID"].ToString()) : Guid.Empty;
                    Estimation.OutwardSalesID = DataReader["OutwardSalesID"] != DBNull.Value ? Guid.Parse(DataReader["OutwardSalesID"].ToString()) : Guid.Empty;
                    Estimation.LargeSalesID = DataReader["LargeSalesID"] != DBNull.Value ? Guid.Parse(DataReader["LargeSalesID"].ToString()) : Guid.Empty;
                    Estimation.SalesPersonID = DataReader["SalesPersonID"] != DBNull.Value ? Guid.Parse(DataReader["SalesPersonID"].ToString()) : Guid.Empty;
                    Estimation.AssistantID = DataReader["AssistantID"] != DBNull.Value ? Guid.Parse(DataReader["AssistantID"].ToString()) : Guid.Empty;
                    Estimation.CoordinatorID = DataReader["CoordinatorID"] != DBNull.Value ? Guid.Parse(DataReader["CoordinatorID"].ToString()) : Guid.Empty;
                    Estimation.ApprovalID = DataReader["ApprovalID"] != DBNull.Value ? Guid.Parse(DataReader["ApprovalID"].ToString()) : Guid.Empty;
                    Estimation.ClientID = DataReader["ClientID"] != DBNull.Value ? Guid.Parse(DataReader["ClientID"].ToString()) : Guid.Empty;
                    Estimation.ProjectID = DataReader["ProjectID"] != DBNull.Value ? Guid.Parse(DataReader["ProjectID"].ToString()) : Guid.Empty;
                    Estimation.TradingID = DataReader["TradingID"] != DBNull.Value ? Guid.Parse(DataReader["TradingID"].ToString()) : Guid.Empty;
                    Estimation.TeamID = DataReader["TeamID"] != DBNull.Value ? Guid.Parse(DataReader["AffiliateTeamID"].ToString()) : Guid.Empty;
                    Estimation.SubSpecializedFieldID = DataReader["SubSpecializedFieldID"] != DBNull.Value ? Guid.Parse(DataReader["SubSpecializedFieldID"].ToString()) : Guid.Empty;
                    Estimation.CurrencyID = Convert.ToInt64(DataReader["CurrencyID"].ToString());

                    Estimation.ClientName = Estimation.ClientID != Guid.Empty ? DataReader["ClientName"].ToString() : string.Empty;
                    Estimation.CurrencyName = DataReader["CurrencyID"] != DBNull.Value ? DataReader["CurrencyName"].ToString() : string.Empty;
                    Estimation.OutwardSalesName = Estimation.OutwardSalesID != Guid.Empty ? DataReader["OutwardSalesName"].ToString() : string.Empty;
                    Estimation.LargeSalesName = Estimation.LargeSalesID != Guid.Empty ? DataReader["LargeSalesName"].ToString() : string.Empty;
                    Estimation.SalesPersonName = Estimation.SalesPersonID != Guid.Empty ? DataReader["SalesPersonName"].ToString() : string.Empty;
                    Estimation.AssistantName = Estimation.AssistantID != Guid.Empty ? DataReader["AssistantName"].ToString() : string.Empty;
                    Estimation.CoordinatorName = Estimation.CoordinatorID != Guid.Empty ? DataReader["CoordinatorName"].ToString() : string.Empty;
                    Estimation.ApprovalName = Estimation.ApprovalID != Guid.Empty ? DataReader["ApprovalName"].ToString() : string.Empty;
                    Estimation.TradingName = Estimation.TradingID != Guid.Empty ? DataReader["TradingName"].ToString() : string.Empty;
                    Estimation.TeamName = Estimation.TeamID != Guid.Empty ? DataReader["AffiliateTeamName"].ToString() : string.Empty;

                    Estimation.IsCompanyPrivate = Convert.ToBoolean(DataReader["IsCompanyPrivate"].ToString());
                    Estimation.ClientStatus = Convert.ToInt32(DataReader["ClientStatus"].ToString());
                    Estimation.BillingCompanyName = DataReader["BillingCompanyName"].ToString();
                    Estimation.BillingTo = DataReader["BillingTo"].ToString();
                    Estimation.BillingEmailCC = DataReader["BillingEmailCC"].ToString();
                    Estimation.BillingAddress = DataReader["BillingAddress"].ToString();
                    Estimation.BillingContactNo = DataReader["BillingContactNo"].ToString();
                    Estimation.BillingFax = DataReader["BillingFax"].ToString();
                    Estimation.IsPostingBill = Convert.ToBoolean(DataReader["IsPostingBill"].ToString());
                    Estimation.PaymentTerms = DataReader["PaymentTerms"].ToString();
                    Estimation.DeliveryCompanyName = DataReader["DeliveryCompanyName"].ToString();
                    Estimation.DeliveryTo = DataReader["DeliveryTo"].ToString();
                    Estimation.DeliveryEmailCC = DataReader["DeliveryEmailCC"].ToString();
                    Estimation.DeliveryAddress = DataReader["DeliveryAddress"].ToString();
                    Estimation.DeliveryContactNo = DataReader["DeliveryContactNo"].ToString();
                    Estimation.DeliveryFax = DataReader["DeliveryFax"].ToString();
                    Estimation.DeliveryInstruction = DataReader["DeliveryInstruction"].ToString();
                    Estimation.RemarksCoordinatorType = DataReader["RemarksCoordinatorType"].ToString();

                    Estimation.IsProspect = Convert.ToBoolean(DataReader["IsProspect"].ToString());
                    Estimation.IsUndisclosed = Convert.ToBoolean(DataReader["IsUndisclosed"].ToString());
                    Estimation.EstimationType = (EstimationType)Convert.ToInt32(DataReader["EstimationType"].ToString());
                    Estimation.EstimationStatus = (EstimationStatus)Convert.ToInt32(DataReader["EstimationStatus"].ToString());
                    Estimation.EstimationTypeName = Enum.GetName(typeof(EstimationType), Estimation.EstimationType);
                    Estimation.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), Estimation.EstimationStatus);
                    Estimation.SpecializedFieldID = Guid.Parse(DataReader["SpecializedFieldID"].ToString());
                    Estimation.IsInternalPurpose = Convert.ToBoolean(DataReader["IsInternalPurpose"].ToString());
                    Estimation.IsExternalPurpose = Convert.ToBoolean(DataReader["IsExternalPurpose"].ToString());
                    Estimation.IsPrintPurpose = Convert.ToBoolean(DataReader["IsPrintPurpose"].ToString());
                    Estimation.IsWebPurpose = Convert.ToBoolean(DataReader["IsWebPurpose"].ToString());
                    Estimation.IsOtherPurpose = Convert.ToBoolean(DataReader["IsOtherPurpose"].ToString());
                    Estimation.OtherPurposeText = DataReader["OtherPurposeText"].ToString();
                    Estimation.PriorityQuality = Convert.ToBoolean(DataReader["PriorityQuality"].ToString());
                    Estimation.PriorityPrice = Convert.ToBoolean(DataReader["PriorityPrice"].ToString());
                    Estimation.PriorityDelivery = Convert.ToBoolean(DataReader["PriorityDelivery"].ToString());
                    Estimation.PriorityTender = Convert.ToBoolean(DataReader["PriorityTender"].ToString());
                    Estimation.KnownByGoogle = Convert.ToBoolean(DataReader["KnownByGoogle"].ToString());
                    Estimation.KnownByYahoo = Convert.ToBoolean(DataReader["KnownByYahoo"].ToString());
                    Estimation.KnownByOthers = Convert.ToBoolean(DataReader["KnownByOthers"].ToString());
                    Estimation.KnownOtherText = DataReader["KnownOtherText"].ToString();
                    Estimation.FinalDeliveryDate = Convert.ToDateTime(DataReader["FinalDeliveryDate"].ToString());
                    Estimation.FirstDeliveryDate = Convert.ToDateTime(DataReader["FirstDeliveryDate"].ToString());
                    Estimation.CoordinatorNotes = DataReader["CoordinatorNotes"].ToString();
                    Estimation.Remarks = DataReader["Remarks"].ToString();
                    Estimation.IsRemarksHideInPDF = Convert.ToBoolean(DataReader["IsRemarksHideInPDF"].ToString());
                    Estimation.IsOrderReceived = Convert.ToBoolean(DataReader["IsOrderReceived"].ToString());
                    Estimation.DiscountTotal = Convert.ToDecimal(DataReader["DiscountTotal"].ToString());
                    Estimation.AverageUnitPrice = Convert.ToDecimal(DataReader["AverageUnitPrice"].ToString());
                    Estimation.ActualUnitPrice = Convert.ToDecimal(DataReader["ActualUnitPrice"].ToString());
                    Estimation.PurposeDetails = DataReader["PurposeDetails"].ToString();
                    Estimation.OrderTitle = DataReader["OrderTitle"].ToString();

                    Estimation.IssuedByTranslator = Convert.ToInt64(DataReader["IssuedByTranslator"].ToString());
                    Estimation.IssuedByCompany = Convert.ToInt64(DataReader["IssuedByCompany"].ToString());
                    Estimation.PriceCertification = Convert.ToDecimal(DataReader["PriceCertification"].ToString());
                    Estimation.OtherItemName = DataReader["OtherItemName"].ToString();
                    Estimation.OtherItemUnitPrice = Convert.ToDecimal(DataReader["OtherItemUnitPrice"].ToString());
                    Estimation.OtherItemNumber = Convert.ToInt32(DataReader["OtherItemNumber"].ToString());
                    Estimation.OtherAmount = Convert.ToDecimal(DataReader["OtherAmount"].ToString());
                    Estimation.TaxEstimation = Convert.ToDecimal(DataReader["TaxEstimation"].ToString());
                    Estimation.QuotationInclTax = Convert.ToDecimal(DataReader["QuotationInclTax"].ToString());
                    Estimation.QuotationExclTax = Convert.ToDecimal(DataReader["QuotationExclTax"].ToString());
                    Estimation.ConsumptionOnTax = Convert.ToDecimal(DataReader["ConsumptionOnTax"].ToString());
                    Estimation.ExcludedTaxCost = Convert.ToDecimal(DataReader["ExcludedTaxCost"].ToString());
                    Estimation.IsCampaign = Convert.ToBoolean(DataReader["IsCampaign"].ToString());
                    Estimation.IsSpecialPrice = Convert.ToBoolean(DataReader["IsSpecialPrice"].ToString());
                    Estimation.IsSpecialDeal = Convert.ToBoolean(DataReader["IsSpecialDeal"].ToString());
                    EstimationList.Add(Estimation);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return EstimationList;
        }

        public List<EstimationModel> DeleteEstimation(EstimationModel model)
        {
            Estimation Estimation;
            List<Estimation> MasterDataList;

            try
            {
                MasterDataList = _dbContext.Estimations.ToList();
                Estimation = MasterDataList.Find(item => item.ID == model.ID);
                if (Estimation != null)
                {
                    //Estimation.IsDeleted = true;
                    _dbContext.Entry(Estimation).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }

            BaseModel.CurrentCulture = model.CurrentCulture;
            BaseModel.CurrentUserID = model.CurrentUserID;
            return GetAllEstimationList(BaseModel);
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

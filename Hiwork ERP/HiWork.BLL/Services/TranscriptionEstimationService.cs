using AutoMapper;
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
using System.IO;
using HiWork.BLL.ServiceHelper;

namespace HiWork.BLL.Services
{
    public partial interface ITranscriptionEstimationService
    {
        bool SaveTranscriptionEstimation(CommonModelHelper_Transcription model);
        List<EstimationModel> GetAllTranscriptionEstimationList(BaseViewModel model);
        bool DeleteTranscriptionEstimation(CommonModelHelper_Transcription model);
        List<TranslationCertificateSettingsModel> GetTranslationCertificateSettingsList(BaseViewModel model);
        List<EstimationDetail> GetAllTranscriptionEstimationDetailsList(BaseViewModel model, Guid estimationID);
    }

    public class TranscriptionEstimationService : ITranscriptionEstimationService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public TranscriptionEstimationService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
        }

        public List<TranslationCertificateSettingsModel> GetTranslationCertificateSettingsList(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader Reader;
            TranslationCertificateSettingsModel SettingsModel;
            List<TranslationCertificateSettingsModel> SettingsList = new List<TranslationCertificateSettingsModel>();

            _sqlConnService.OpenConnection();
            try
            {
                cmd = new SqlCommand("SP_GetAllTranslationCertificateSettings", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                Reader = cmd.ExecuteReader();
                while (Reader.Read() == true)
                {
                    SettingsModel = new TranslationCertificateSettingsModel();
                    SettingsModel.ID = Convert.ToInt64(Reader["ID"].ToString());
                    SettingsModel.CertificateType = (CertificateType)Convert.ToInt32(Reader["CertificateType"].ToString());
                    SettingsModel.UnitPrice = Convert.ToInt64(Reader["UnitPrice"].ToString());
                    SettingsList.Add(SettingsModel);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Transcription Estimation Input", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return SettingsList;
        }

        private string GenerateEstimationNumber(long appid)
        {
            string RegistrationIdNext;
            string Today, AppCode;
            StringBuilder buffer = new StringBuilder();
            IApplicationService appService = new ApplicationService(new ApplicationRepository(new UnitOfWork()));

            RegistrationIdNext = GetNextRegistrationID();
            AppCode = appService.GetApplicationCode(appid);
            Today = DateTime.Now.ToString("yyyyMMdd", CultureInfo.CurrentCulture);
            buffer.Append(Today);
            buffer.Append(AppCode);
            buffer.Append(RegistrationIdNext);
            return buffer.ToString();
        }

        public bool SaveTranscriptionEstimation(CommonModelHelper_Transcription model)
        {
            var isSuccessful = true;
            try
            {
                var culturalItems = new List<string> { "BillingAddress", "ClientAddress", "BillingCompanyName", "DeliveryCompanyName", "DeliveryAddress", "Remarks", "CoordinatorNotes", "QuotationNotes" };
                ModelBinder.SetCulturalValue(model.Estimation, model, culturalItems);

                model.Estimation.EstimationNo = GenerateEstimationNumber(model.ApplicationID);
                model.Estimation.EstimationType = (int)EstimationType.Transcription;
                if(model.Estimation.EstimationStatus == 0)
                {
                    model.Estimation.EstimationStatus = (int)EstimationStatus.Ordered;
                }
                ModelBinder.ModifyGuidValue(model.Estimation);
                //cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                if (model.Estimation.ID == Guid.Empty)
                {
                    model.Estimation.ID = Guid.NewGuid();
                    model.Estimation.RegistrationDate = DateTime.Now;
                    _dbContext.Estimations.Add(model.Estimation);
                }
                else
                {
                    //model.Estimation.FirstDeliveryDate = null;
                    //model.Estimation.FinalDeliveryDate = null;
                    _dbContext.Entry(model.Estimation).State = EntityState.Modified;
                }
                //Save or update Estimation details
                foreach (var item in model.EstimationDetails)
                {
                    item.EstimationID = model.Estimation.ID;
                    ModelBinder.ModifyGuidValue(item);

                    if ((item.ID == Guid.Empty) && (!item.IsMarkedForDelete))
                    {
                        item.ID = Guid.NewGuid();
                        var mapItem = Mapper.Map<EstimationDetailsModel, EstimationDetail>(item);
                        _dbContext.EstimationDetails.Add(mapItem);
                    }
                    else
                    {
                        if ((!item.IsMarkedForDelete) && (item.ID != Guid.Empty))
                        {
                            var mapItem = Mapper.Map<EstimationDetailsModel, EstimationDetail>(item);
                            _dbContext.Entry(mapItem).State = EntityState.Modified;
                        }
                        if ((item.IsMarkedForDelete) && (item.ID != Guid.Empty))
                        {
                            var itm = _dbContext.EstimationDetails.Find(item.ID);
                            _dbContext.EstimationDetails.Remove(itm);
                        }
                    }
                }
                #region "Commented"
                ////Save file type
                //var existingFileTypes = _dbContext.EstimationDeliveryFileTypes.Where(x => x.Estimation.ID == model.Estimation.ID).ToList();
                //var existingWorkContent = _dbContext.EstimationWorkContents.Where(x => x.Estimation.ID == model.Estimation.ID).ToList();
                //existingFileTypes?.ForEach(eft =>
                //{
                //    _dbContext.EstimationDeliveryFileTypes.Remove(eft);
                //});
                //existingWorkContent?.ForEach(ewc =>
                //{
                //    _dbContext.EstimationWorkContents.Remove(ewc);
                //});
                //model.FileTypes?.ForEach(ft =>
                //{
                //    var fileType = new EstimationDeliveryFileType
                //    {
                //        ID = Guid.NewGuid(),
                //        Estimation = model.Estimation,
                //        FileType = ft.FileType,
                //        Version = ft.Version,
                //        IsDeleted = false
                //    };
                //    _dbContext.EstimationDeliveryFileTypes.Add(fileType);
                //});
                ////Save work content
                //model.WorkContents?.ForEach(wc =>
                //{
                //    var content = new EstimationWorkContent
                //    {
                //        ID = Guid.NewGuid(),
                //        Estimation = model.Estimation,
                //        WorkContent = wc.WorkContent,
                //        IsDeleted = false
                //    };
                //    _dbContext.EstimationWorkContents.Add(content);
                //});
                #endregion "Commented"
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "TranscriptionEstimation", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }

        #region "Commented Save"
        //public bool SaveTranscriptionEstimation(EstimationModel model)
        //{
        //    SqlCommand cmd, cmd2, cmd3;
        //    bool isSuccessful = false;
        //    Guid EstimationID;
        //    string StatementType;

        //    try
        //    {
        //        _sqlConnService.OpenConnection();
        //        cmd = new SqlCommand("SP_SaveEstimation", _sqlConnService.CreateConnection());
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        string EstimationNumber = GenerateEstimationNumber(model.ApplicationId);
        //        cmd.Parameters.AddWithValue("@InquiryDate", model.InquiryDate);
        //        cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
        //        cmd.Parameters.AddWithValue("@EstimationRouteID", model.EstimateRouteID);
        //        cmd.Parameters.AddWithValue("@OutwardSalesID", model.OutwardSalesID);
        //        cmd.Parameters.AddWithValue("@LargeSalesID", model.LargeSalesID);
        //        cmd.Parameters.AddWithValue("@SalesPersonID", model.SalesPersonID);
        //        cmd.Parameters.AddWithValue("@AssistantID", model.AssistantID);
        //        cmd.Parameters.AddWithValue("@CoordinatorID", model.CoordinatorID);
        //        cmd.Parameters.AddWithValue("@ApprovalID", model.ApprovalID);
        //        cmd.Parameters.AddWithValue("@ClientID", model.ClientID);
        //        cmd.Parameters.AddWithValue("@TradingID", model.TradingID);
        //        cmd.Parameters.AddWithValue("@AffiliateTeamID", model.AffiliateTeamID);
        //        cmd.Parameters.AddWithValue("@IsCompanyPrivate", model.IsCompanyPrivate);
        //        cmd.Parameters.AddWithValue("@ClientStatus", model.ClientStatus);
        //        cmd.Parameters.AddWithValue("@SubSpecializedFieldID", model.SubSpecializedFieldID);
        //        cmd.Parameters.AddWithValue("@ClientDepartmentID", model.ClientDepartmentID);
        //        cmd.Parameters.AddWithValue("@BusinessCategoryID", model.BusinessCategoryID);
        //        cmd.Parameters.AddWithValue("@ClientPersonInCharge", model.ClientPersonInCharge);
        //        cmd.Parameters.AddWithValue("@ClientEmailCC", model.ClientEmailCC);
        //        cmd.Parameters.AddWithValue("@ClientAddress_" + model.CurrentCulture, model.ClientAddress);
        //        cmd.Parameters.AddWithValue("@ClientContactNo", model.ClientContactNo);
        //        cmd.Parameters.AddWithValue("@ClientFax", model.ClientFax);
        //        cmd.Parameters.AddWithValue("@BillingCompanyName_" + model.CurrentCulture, model.BillingCompanyName);
        //        cmd.Parameters.AddWithValue("@BillingTo", model.BillingTo);
        //        cmd.Parameters.AddWithValue("@BillingEmailCC", model.BillingEmailCC);
        //        cmd.Parameters.AddWithValue("@BillingAddress_" + model.CurrentCulture, model.BillingAddress);
        //        cmd.Parameters.AddWithValue("@BillingContactNo", model.BillingContactNo);
        //        cmd.Parameters.AddWithValue("@BillingFax", model.BillingFax);
        //        cmd.Parameters.AddWithValue("@IsPostingBill", model.IsPostingBill);
        //        cmd.Parameters.AddWithValue("@PaymentTerms", model.PaymentTerms);
        //        cmd.Parameters.AddWithValue("@DeliveryCompanyName_" + model.CurrentCulture, model.DeliveryCompanyName);
        //        cmd.Parameters.AddWithValue("@DeliveryTo", model.DeliveryTo);
        //        cmd.Parameters.AddWithValue("@DeliveryEmailCC", model.DeliveryEmailCC);
        //        cmd.Parameters.AddWithValue("@DeliveryAddress_" + model.CurrentCulture, model.DeliveryAddress);
        //        cmd.Parameters.AddWithValue("@DeliveryContactNo", model.DeliveryContactNo);
        //        cmd.Parameters.AddWithValue("@DeliveryFax", model.DeliveryFax);
        //        cmd.Parameters.AddWithValue("@DeliveryInstruction", model.DeliveryInstruction);
        //        cmd.Parameters.AddWithValue("@RemarksCoordinatorType", model.RemarksCoordinatorType);
        //        cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
        //        cmd.Parameters.AddWithValue("@IsProspect", model.IsProspect);
        //        cmd.Parameters.AddWithValue("@IsUndisclosed", model.IsUndisclosed);
        //        cmd.Parameters.AddWithValue("@EstimationType", model.EstimationType);
        //        cmd.Parameters.AddWithValue("@EstimationStatus", model.EstimationStatus);
        //        cmd.Parameters.AddWithValue("@SpecializedFieldID", model.SpecializedFieldID);
        //        cmd.Parameters.AddWithValue("@IsInternalPurpose", model.IsInternalPurpose);
        //        cmd.Parameters.AddWithValue("@IsExternalPurpose", model.IsExternalPurpose);
        //        cmd.Parameters.AddWithValue("@IsPrintPurpose", model.IsPrintPurpose);
        //        cmd.Parameters.AddWithValue("@IsWebPurpose", model.IsWebPurpose);
        //        cmd.Parameters.AddWithValue("@IsOtherPurpose", model.IsOtherPurpose);
        //        cmd.Parameters.AddWithValue("@OtherPurposeText", model.OtherPurposeText);
        //        cmd.Parameters.AddWithValue("@PriorityQuality", model.PriorityQuality);
        //        cmd.Parameters.AddWithValue("@PriorityPrice", model.PriorityPrice);
        //        cmd.Parameters.AddWithValue("@PriorityDelivery", model.PriorityDelivery);
        //        cmd.Parameters.AddWithValue("@PriorityTender", model.PriorityTender);
        //        cmd.Parameters.AddWithValue("@KnownByGoogle", model.KnownByGoogle);
        //        cmd.Parameters.AddWithValue("@KnownByYahoo", model.KnownByYahoo);
        //        cmd.Parameters.AddWithValue("@KnownByEmail", model.KnownByEmail);
        //        cmd.Parameters.AddWithValue("@KnownByBing", model.KnownByBing);
        //        cmd.Parameters.AddWithValue("@KnownByOthers", model.KnownByOthers);
        //        cmd.Parameters.AddWithValue("@KnownOtherText", model.KnownOtherText);
        //        cmd.Parameters.AddWithValue("@FinalDeliveryDate", model.FinalDeliveryDate);
        //        cmd.Parameters.AddWithValue("@FirstDeliveryDate", model.FirstDeliveryDate);
        //        cmd.Parameters.AddWithValue("@CoordinatorNotes_" + model.CurrentCulture, model.CoordinatorNotes);
        //        cmd.Parameters.AddWithValue("@Remarks_" + model.CurrentCulture, model.CurrentCulture);
        //        cmd.Parameters.AddWithValue("@IsRemarksHideInPDF", model.IsRemarksHideInPDF);
        //        cmd.Parameters.AddWithValue("@IsOrderReceived", model.IsOrderReceived);
        //        cmd.Parameters.AddWithValue("@DiscountTotal", model.DiscountTotal);
        //        cmd.Parameters.AddWithValue("@AverageUnitPrice", model.AverageUnitPrice);
        //        cmd.Parameters.AddWithValue("@ActualUnitPrice", model.ActualUnitPrice);
        //        cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
        //        cmd.Parameters.AddWithValue("@PurposeDetails", model.PurposeDetails);
        //        cmd.Parameters.AddWithValue("@OrderTitle", model.OrderTitle);
        //        cmd.Parameters.AddWithValue("@IssuedByTranslator", model.IssuedByTranslator);
        //        cmd.Parameters.AddWithValue("@IssuedByCompany", model.IssuedByCompany);
        //        cmd.Parameters.AddWithValue("@PriceCertification", model.PriceCertification);
        //        cmd.Parameters.AddWithValue("@OtherItemName_" + model.CurrentCulture, model.OtherItemName);
        //        cmd.Parameters.AddWithValue("@OtherItemUnitPrice", model.OtherItemUnitPrice);
        //        cmd.Parameters.AddWithValue("@OtherItemNumber", model.OtherItemNumber);
        //        cmd.Parameters.AddWithValue("@OtherAmount", model.OtherAmount);
        //        cmd.Parameters.AddWithValue("@TaxEstimation", model.TaxEstimation);
        //        cmd.Parameters.AddWithValue("@QuotationInclTax", model.QuotationInclTax);
        //        cmd.Parameters.AddWithValue("@QuotationExclTax", model.QuotationExclTax);
        //        cmd.Parameters.AddWithValue("@ConsumptionOnTax", model.ConsumptionOnTax);
        //        cmd.Parameters.AddWithValue("@ExcludedTaxCost", model.ExcludedTaxCost);
        //        cmd.Parameters.AddWithValue("@IsCampaign", model.IsCampaign);
        //        cmd.Parameters.AddWithValue("@IsSpecialPrice", model.IsSpecialPrice);
        //        cmd.Parameters.AddWithValue("@IsSpecialDeal", model.IsSpecialDeal);
        //        cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
        //        cmd.Parameters.AddWithValue("@SubtotalAfterDiscount", model.SubtotalAfterDiscount);
        //        cmd.Parameters.AddWithValue("@AttachedMaterialFileName", model.AttachedMaterialFileName);
        //        cmd.Parameters.AddWithValue("@AttachedMaterialDownloadURL", model.AttachedMaterialDownloadURL);

        //        //string EstimationNumber = GenerateEstimationNumber(model.ApplicationId);
        //        //cmd.Parameters.AddWithValue("@EstimationNo", EstimationNumber);
        //        //cmd.Parameters.AddWithValue("@InquiryDate", model.InquiryDate);
        //        //cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
        //        //cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
        //        //cmd.Parameters.AddWithValue("@DivisionID", model.TradingID);
        //        //cmd.Parameters.AddWithValue("@EstimationRouteID", model.EstimateRouteID);
        //        //cmd.Parameters.AddWithValue("@OutwardSalesID", model.OutwardSalesID);
        //        //cmd.Parameters.AddWithValue("@LargeSalesID", model.LargeSalesID);
        //        //cmd.Parameters.AddWithValue("@SalesPersonID", model.SalesPersonID);
        //        //cmd.Parameters.AddWithValue("@AssistantID", model.AssistantID);
        //        //cmd.Parameters.AddWithValue("@CoordinatorID", model.CoordinatorID);
        //        //cmd.Parameters.AddWithValue("@ApprovalID", model.ApprovalID);
        //        //cmd.Parameters.AddWithValue("@ClientID", model.ClientID);
        //        //cmd.Parameters.AddWithValue("@TradingID", model.TradingID);
        //        //cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
        //        //cmd.Parameters.AddWithValue("@AffiliateTeamID", model.AffiliateTeamID);
        //        //cmd.Parameters.AddWithValue("@IsCompanyPrivate", model.IsCompanyPrivate);
        //        //cmd.Parameters.AddWithValue("@ClientStatus", model.ClientStatus);
        //        //cmd.Parameters.AddWithValue("@SubSpecializedFieldID", model.SubSpecializedFieldID);
        //        //cmd.Parameters.AddWithValue("@BillingCompanyName_" + model.CurrentCulture, model.BillingCompanyName);
        //        //cmd.Parameters.AddWithValue("@BillingContactNo", model.BillingContactNo);
        //        //cmd.Parameters.AddWithValue("@BillingFax", model.BillingFax);
        //        //cmd.Parameters.AddWithValue("@IsPostingBill", model.IsPostingBill);
        //        //cmd.Parameters.AddWithValue("@PaymentTerms", model.PaymentTerms);
        //        //cmd.Parameters.AddWithValue("@DeliveryCompanyName_" + model.CurrentCulture, model.DeliveryCompanyName);
        //        //cmd.Parameters.AddWithValue("@DeliveryTo", model.DeliveryTo);
        //        //cmd.Parameters.AddWithValue("@DeliveryEmailCC", model.DeliveryEmailCC);
        //        //cmd.Parameters.AddWithValue("@DeliveryAddress_" + model.CurrentCulture, model.DeliveryAddress);
        //        //cmd.Parameters.AddWithValue("@DeliveryContactNo", model.DeliveryContactNo);
        //        //cmd.Parameters.AddWithValue("@DeliveryFax", model.DeliveryFax);
        //        //cmd.Parameters.AddWithValue("@DeliveryInstruction", model.DeliveryInstruction);
        //        //cmd.Parameters.AddWithValue("@RemarksCoordinatorType", model.RemarksCoordinatorType);
        //        //cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
        //        //cmd.Parameters.AddWithValue("@IsProspect", model.IsProspect);
        //        //cmd.Parameters.AddWithValue("@IsUndisclosed", model.IsUndisclosed);
        //        //cmd.Parameters.AddWithValue("@EstimationType", (int)EstimationType.Transcription);
        //        //cmd.Parameters.AddWithValue("@EstimationStatus", (int)EstimationStatus.Created);
        //        //cmd.Parameters.AddWithValue("@SpecializedFieldID", model.SpecializedFieldID);
        //        //cmd.Parameters.AddWithValue("@IsInternalPurpose", model.IsInternalPurpose);
        //        //cmd.Parameters.AddWithValue("@IsExternalPurpose", model.IsExternalPurpose);
        //        //cmd.Parameters.AddWithValue("@IsPrintPurpose", model.IsPrintPurpose);
        //        //cmd.Parameters.AddWithValue("@IsWebPurpose", model.IsWebPurpose);
        //        //cmd.Parameters.AddWithValue("@IsOtherPurpose", model.IsOtherPurpose);
        //        //cmd.Parameters.AddWithValue("@OtherPurposeText", model.OtherPurposeText);
        //        //cmd.Parameters.AddWithValue("@PriorityQuality", model.PriorityQuality);
        //        //cmd.Parameters.AddWithValue("@PriorityPrice", model.PriorityPrice);
        //        //cmd.Parameters.AddWithValue("@PriorityDelivery", model.PriorityDelivery);
        //        //cmd.Parameters.AddWithValue("@PriorityTender", model.PriorityTender);
        //        //cmd.Parameters.AddWithValue("@KnownByGoogle", model.KnownByGoogle);
        //        //cmd.Parameters.AddWithValue("@KnownByYahoo", model.KnownByYahoo);
        //        //cmd.Parameters.AddWithValue("@KnownByEmail", model.KnownByEmail);
        //        //cmd.Parameters.AddWithValue("@KnownOtherText", model.KnownOtherText);
        //        //cmd.Parameters.AddWithValue("@FinalDeliveryDate", model.FinalDeliveryDate);
        //        //cmd.Parameters.AddWithValue("@FirstDeliveryDate", model.FirstDeliveryDate);
        //        //cmd.Parameters.AddWithValue("@CoordinatorNotes_" + model.CurrentCulture, model.CoordinatorNotes);
        //        //cmd.Parameters.AddWithValue("@Remarks_" + model.CurrentCulture, model.CurrentCulture);
        //        //cmd.Parameters.AddWithValue("@IsRemarksHideInPDF", model.IsRemarksHideInPDF);
        //        //cmd.Parameters.AddWithValue("@IsOrderReceived", model.IsOrderReceived);
        //        //cmd.Parameters.AddWithValue("@DiscountTotal", model.DiscountTotal);
        //        //cmd.Parameters.AddWithValue("@AverageUnitPrice", model.AverageUnitPrice);
        //        //cmd.Parameters.AddWithValue("@ActualUnitPrice", model.ActualUnitPrice);
        //        //cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
        //        //cmd.Parameters.AddWithValue("@PurposeDetails", model.PurposeDetails);
        //        //cmd.Parameters.AddWithValue("@OrderTitle", model.OrderTitle);
        //        //cmd.Parameters.AddWithValue("@IssuedByTranslator", model.IssuedByTranslator);
        //        //cmd.Parameters.AddWithValue("@IssuedByCompany", model.IssuedByCompany);
        //        //cmd.Parameters.AddWithValue("@PriceCertification", model.PriceCertification);
        //        //cmd.Parameters.AddWithValue("@OtherItemName_" + model.CurrentCulture, model.OtherItemName);
        //        //cmd.Parameters.AddWithValue("@OtherItemUnitPrice", model.OtherItemUnitPrice);
        //        //cmd.Parameters.AddWithValue("@OtherAmount", model.OtherAmount);
        //        //cmd.Parameters.AddWithValue("@TaxEstimation", model.TaxEstimation);
        //        //cmd.Parameters.AddWithValue("@QuotationInclTax", model.QuotationInclTax);
        //        //cmd.Parameters.AddWithValue("@QuotationExclTax", model.QuotationExclTax);
        //        //cmd.Parameters.AddWithValue("@ConsumptionOnTax", model.ConsumptionOnTax);
        //        //cmd.Parameters.AddWithValue("@ExcludedTaxCost", model.ExcludedTaxCost);
        //        //cmd.Parameters.AddWithValue("@IsCampaign", model.IsCampaign);
        //        //cmd.Parameters.AddWithValue("@IsSpecialPrice", model.IsSpecialPrice);
        //        //cmd.Parameters.AddWithValue("@IsSpecialDeal", model.IsSpecialDeal);
        //        //cmd.Parameters.AddWithValue("@BusinessCategoryID", model.BusinessCategoryID);
        //        //cmd.Parameters.AddWithValue("@ClientPersonInCharge", model.ClientPersonInCharge);
        //        //cmd.Parameters.AddWithValue("@ClientEmailCC", model.ClientEmailCC);
        //        //cmd.Parameters.AddWithValue("@ClientAddress_" + model.CurrentCulture, model.ClientAddress);
        //        //cmd.Parameters.AddWithValue("@ClientContactNo", model.ClientContactNo);
        //        //cmd.Parameters.AddWithValue("@ClientFax", model.ClientFax);
        //        //cmd.Parameters.AddWithValue("@BillingTo", model.BillingTo);
        //        //cmd.Parameters.AddWithValue("@BillingEmailCC", model.BillingEmailCC);
        //        //cmd.Parameters.AddWithValue("@BillingAddress_" + model.CurrentCulture, model.BillingAddress);
        //        //cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
        //        //cmd.Parameters.AddWithValue("@SubtotalAfterDiscount", model.SubtotalAfterDiscount);
        //        //cmd.Parameters.AddWithValue("@AttachedMaterialFileName", model.AttachedMaterialFileName);
        //        //cmd.Parameters.AddWithValue("@AttachedMaterialDownloadURL", model.AttachedMaterialDownloadURL);

        //        #region "commented"
        //        //cmd.Parameters.AddWithValue("@EstimationNo", EstimationNumber);
        //        //cmd.Parameters.AddWithValue("@InquiryDate", model.InquiryDate);
        //        //cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
        //        //cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
        //        //cmd.Parameters.AddWithValue("@DivisionID", model.TradingID);
        //        //cmd.Parameters.AddWithValue("@EstimationRouteID", model.EstimateRouteID);
        //        //cmd.Parameters.AddWithValue("@OutwardSalesID", model.OutwardSalesID);
        //        //cmd.Parameters.AddWithValue("@LargeSalesID", model.LargeSalesID);
        //        //cmd.Parameters.AddWithValue("@SalesPersonID", model.SalesPersonID);
        //        //cmd.Parameters.AddWithValue("@AssistantID", model.AssistantID);
        //        //cmd.Parameters.AddWithValue("@CoordinatorID", model.CoordinatorID);
        //        //cmd.Parameters.AddWithValue("@ApprovalID", model.ApprovalID);
        //        //cmd.Parameters.AddWithValue("@ClientID", model.ClientID);
        //        //cmd.Parameters.AddWithValue("@TradingID", model.TradingID);
        //        //cmd.Parameters.AddWithValue("@ProjectID", model.ProjectID);
        //        //cmd.Parameters.AddWithValue("@AffiliateTeamID", model.AffiliateTeamID);
        //        //cmd.Parameters.AddWithValue("@IsCompanyPrivate", model.IsCompanyPrivate);
        //        //cmd.Parameters.AddWithValue("@ClientStatus", model.ClientStatus);
        //        //cmd.Parameters.AddWithValue("@SpecializedFieldID", model.SpecializedFieldID);
        //        //cmd.Parameters.AddWithValue("@SubSpecializedFieldID", model.SubSpecializedFieldID);
        //        //cmd.Parameters.AddWithValue("@BillingCompanyName_" + model.CurrentCulture, model.BillingCompanyName);
        //        //cmd.Parameters.AddWithValue("@BillingContactNo", model.BillingContactNo);
        //        //cmd.Parameters.AddWithValue("@BillingFax", model.BillingFax);
        //        //cmd.Parameters.AddWithValue("@IsPostingBill", model.IsPostingBill);
        //        //cmd.Parameters.AddWithValue("@PaymentTerms", model.PaymentTerms);
        //        //cmd.Parameters.AddWithValue("@DeliveryCompanyName_" + model.CurrentCulture, model.DeliveryCompanyName);
        //        //cmd.Parameters.AddWithValue("@DeliveryTo", model.DeliveryTo);
        //        //cmd.Parameters.AddWithValue("@DeliveryEmailCC", model.DeliveryEmailCC);
        //        //cmd.Parameters.AddWithValue("@DeliveryAddress_" + model.CurrentCulture, model.DeliveryAddress);
        //        //cmd.Parameters.AddWithValue("@DeliveryContactNo", model.DeliveryContactNo);
        //        //cmd.Parameters.AddWithValue("@DeliveryFax", model.DeliveryFax);
        //        //cmd.Parameters.AddWithValue("@DeliveryInstruction", model.DeliveryInstruction);
        //        //cmd.Parameters.AddWithValue("@RemarksCoordinatorType", model.RemarksCoordinatorType);
        //        //cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
        //        //cmd.Parameters.AddWithValue("@IsProspect", model.IsProspect);
        //        //cmd.Parameters.AddWithValue("@IsUndisclosed", model.IsUndisclosed);
        //        //cmd.Parameters.AddWithValue("@EstimationType", (int) EstimationType.Transcription);
        //        //cmd.Parameters.AddWithValue("@EstimationStatus", (int) EstimationStatus.Created);                
        //        //cmd.Parameters.AddWithValue("@IsInternalPurpose", model.IsInternalPurpose);
        //        //cmd.Parameters.AddWithValue("@IsExternalPurpose", model.IsExternalPurpose);
        //        //cmd.Parameters.AddWithValue("@IsPrintPurpose", model.IsPrintPurpose);
        //        //cmd.Parameters.AddWithValue("@IsWebPurpose", model.IsWebPurpose);
        //        //cmd.Parameters.AddWithValue("@IsOtherPurpose", model.IsOtherPurpose);
        //        //cmd.Parameters.AddWithValue("@OtherPurposeText", model.OtherPurposeText);
        //        //cmd.Parameters.AddWithValue("@PriorityQuality", model.PriorityQuality);
        //        //cmd.Parameters.AddWithValue("@PriorityPrice", model.PriorityPrice);
        //        //cmd.Parameters.AddWithValue("@PriorityDelivery", model.PriorityDelivery);
        //        //cmd.Parameters.AddWithValue("@PriorityTender", model.PriorityTender);
        //        //cmd.Parameters.AddWithValue("@KnownByGoogle", model.KnownByGoogle);
        //        //cmd.Parameters.AddWithValue("@KnownByYahoo", model.KnownByYahoo);
        //        //cmd.Parameters.AddWithValue("@KnownByEmail", model.KnownByEmail);
        //        //cmd.Parameters.AddWithValue("@KnownOtherText", model.KnownOtherText);
        //        //cmd.Parameters.AddWithValue("@FinalDeliveryDate", model.FinalDeliveryDate);
        //        //cmd.Parameters.AddWithValue("@FirstDeliveryDate", model.FirstDeliveryDate);
        //        //cmd.Parameters.AddWithValue("@CoordinatorNotes_" + model.CurrentCulture, model.CoordinatorNotes);
        //        //cmd.Parameters.AddWithValue("@Remarks_" + model.CurrentCulture, model.CurrentCulture);
        //        //cmd.Parameters.AddWithValue("@IsRemarksHideInPDF", model.IsRemarksHideInPDF);
        //        //cmd.Parameters.AddWithValue("@IsOrderReceived", model.IsOrderReceived);
        //        //cmd.Parameters.AddWithValue("@DiscountTotal", model.DiscountTotal);
        //        //cmd.Parameters.AddWithValue("@AverageUnitPrice", model.AverageUnitPrice);
        //        //cmd.Parameters.AddWithValue("@ActualUnitPrice", model.ActualUnitPrice);
        //        //cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
        //        //cmd.Parameters.AddWithValue("@PurposeDetails", model.PurposeDetails);
        //        //cmd.Parameters.AddWithValue("@OrderTitle", model.OrderTitle);
        //        //cmd.Parameters.AddWithValue("@IssuedByTranslator", model.IssuedByTranslator);
        //        //cmd.Parameters.AddWithValue("@IssuedByCompany", model.IssuedByCompany);
        //        //cmd.Parameters.AddWithValue("@PriceCertification", model.PriceCertification);
        //        //cmd.Parameters.AddWithValue("@OtherItemName_" + model.CurrentCulture, model.OtherItemName);
        //        //cmd.Parameters.AddWithValue("@OtherItemUnitPrice", model.OtherItemUnitPrice);
        //        //cmd.Parameters.AddWithValue("@OtherAmount", model.OtherAmount);
        //        //cmd.Parameters.AddWithValue("@TaxEstimation", model.TaxEstimation);
        //        //cmd.Parameters.AddWithValue("@QuotationInclTax", model.QuotationInclTax);
        //        //cmd.Parameters.AddWithValue("@QuotationExclTax", model.QuotationExclTax);
        //        //cmd.Parameters.AddWithValue("@ConsumptionOnTax", model.ConsumptionOnTax);
        //        //cmd.Parameters.AddWithValue("@ExcludedTaxCost", model.ExcludedTaxCost);
        //        //cmd.Parameters.AddWithValue("@IsCampaign", model.IsCampaign);
        //        //cmd.Parameters.AddWithValue("@IsSpecialPrice", model.IsSpecialPrice);
        //        //cmd.Parameters.AddWithValue("@IsSpecialDeal", model.IsSpecialDeal);
        //        //cmd.Parameters.AddWithValue("@SubtotalAfterDiscount", model.SubtotalAfterDiscount);
        //        //cmd.Parameters.AddWithValue("@QuotationTotalInclTax", model.QuotationTotalInclTax);
        //        //cmd.Parameters.AddWithValue("@QuotationTotalExclTax", model.QuotationTotalExclTax);
        //        //cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);

        //        //cmd.Parameters.AddWithValue("@BusinessCategoryID", model.BusinessCategoryID);
        //        //cmd.Parameters.AddWithValue("@ClientPersonInCharge", model.ClientPersonInCharge);
        //        //cmd.Parameters.AddWithValue("@ClientEmailCC", model.ClientEmailCC);
        //        //cmd.Parameters.AddWithValue("@ClientAddress_" + model.CurrentCulture, model.ClientAddress);
        //        //cmd.Parameters.AddWithValue("@ClientContactNo", model.ClientContactNo);
        //        //cmd.Parameters.AddWithValue("@ClientFax", model.ClientFax);
        //        //cmd.Parameters.AddWithValue("@BillingTo", model.BillingTo);
        //        //cmd.Parameters.AddWithValue("@BillingEmailCC", model.BillingEmailCC);
        //        //cmd.Parameters.AddWithValue("@BillingAddress_" + model.CurrentCulture, model.BillingAddress);
        //        #endregion "commented"
        //        if (model.ID == Guid.Empty)
        //        {
        //            EstimationID = Guid.NewGuid();
        //            cmd.Parameters.AddWithValue("@ID", EstimationID);
        //            cmd.Parameters.AddWithValue("@StatementType", "Insert");
        //            StatementType = "Insert";
        //        }
        //        else
        //        {
        //            EstimationID = model.ID;
        //            cmd.Parameters.AddWithValue("@ID", EstimationID);
        //            cmd.Parameters.AddWithValue("@StatementType", "Update");
        //            StatementType = "Update";
        //        }

        //        //string tmp = cmd.CommandText.ToString();
        //        //foreach (SqlParameter p in cmd.Parameters)
        //        //{
        //        //    tmp += tmp.Replace('@' + p.ParameterName.ToString(), "'" + p.Value.ToString() + "'");                    
        //        //}

        //        cmd.ExecuteNonQuery();

        //        foreach (EstimationDetailsModel DetailsModel in model.TranscriptionEstimationItems)
        //        {
        //            Guid EstimationDetailsID;

        //            cmd2 = new SqlCommand("SP_SaveEstimationDetails", _sqlConnService.CreateConnection());
        //            cmd2.CommandType = CommandType.StoredProcedure;
        //            cmd2.Parameters.AddWithValue("@EstimationID", EstimationID);
        //            cmd2.Parameters.AddWithValue("@SourceLanguageID", DetailsModel.SourceLanguageID);
        //            cmd2.Parameters.AddWithValue("@TargetLanguageID", DetailsModel.TargetLanguageID);
        //            cmd2.Parameters.AddWithValue("@ServiceType", DetailsModel.ServiceTypeID);   //
        //            cmd2.Parameters.AddWithValue("@UnitPrice1", DetailsModel.UnitPrice1);
        //            cmd2.Parameters.AddWithValue("@Discount1", DetailsModel.Discount1);
        //            cmd2.Parameters.AddWithValue("@ExcludedTaxCost", DetailsModel.ExcludedTaxCost);
        //            cmd2.Parameters.AddWithValue("@Contents", DetailsModel.Contents);
        //            cmd2.Parameters.AddWithValue("@LengthMinute", DetailsModel.LengthMinute);
        //            cmd2.Parameters.AddWithValue("@WithTranslation", DetailsModel.WithTranslation);

        //            cmd2.Parameters.AddWithValue("@StatementType", StatementType);

        //            if (DetailsModel.ID == Guid.Empty)
        //            {
        //                EstimationDetailsID = Guid.NewGuid();
        //                cmd2.Parameters.AddWithValue("@ID", EstimationDetailsID);
        //            }
        //            else
        //            {
        //                EstimationDetailsID = DetailsModel.ID;
        //                cmd2.Parameters.AddWithValue("@ID", EstimationDetailsID);
        //            }
        //            cmd2.ExecuteNonQuery();
        //            isSuccessful = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        isSuccessful = false;
        //        IErrorLogService errorLog = new ErrorLogService();
        //        string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
        //        errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
        //        throw new Exception(message);
        //    }
        //    finally
        //    {
        //        _sqlConnService.CloseConnection();
        //    }
        //    return isSuccessful;
        //}
        #endregion "Commented Save"
        private string GetNextRegistrationID()
        {
            var item = _dbContext.Estimations.OrderByDescending(e => e.RegistrationID).Select(e => e.RegistrationID).FirstOrDefault();
            return (item + 1).ToString();
        }

        public List<EstimationModel> GetAllTranscriptionEstimationList(BaseViewModel model)
        {
            EstimationModel EstimationModel;
            List<Estimation> MasterDataList;
            List<EstimationModel> EstimationModelList = new List<EstimationModel>();

            try
            {
                MasterDataList = _dbContext.Estimations.ToList();
                if (MasterDataList != null)
                {
                    foreach (Estimation MasterData in MasterDataList)
                    {
                        //if (MasterData.IsDeleted == true)
                        //    continue;

                        EstimationModel = Mapper.Map<Estimation, EstimationModel>(MasterData);
                        EstimationModel.CurrentUserID = model.CurrentUserID;
                        EstimationModel.CurrentCulture = model.CurrentCulture;
                        EstimationModelList.Add(EstimationModel);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            return EstimationModelList;
        }

        public List<EstimationDetail> GetAllTranscriptionEstimationDetailsList(BaseViewModel model, Guid EstimationID)
        {
            var items = _dbContext.EstimationDetails.Where(e => e.Estimation.ID == EstimationID).ToList();
            return items;
        }

        public bool DeleteTranscriptionEstimation(CommonModelHelper_Transcription model)
        {
            bool flag = false;
            try
            {
                var estimation = _dbContext.Estimations.Find(model.Estimation.ID);
                if (estimation != null)
                {
                    estimation.IsDeleted = true;
                    _dbContext.Entry(estimation).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            return flag;
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

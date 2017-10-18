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
    public partial interface IEstimationService
    {
        bool SaveEstimation(EstimationModel model);
        List<EstimationModel> GetAllEstimationList(BaseViewModel model);
        List<EstimationModel> DeleteEstimation(EstimationModel model);
        List<TranslationCertificateSettingsModel> GetTranslationCertificateSettingsList(BaseViewModel model);
        List<EstimationDetailsModel> GetEstimationDetailsListByID(BaseViewModel model, string EstimationID);
        List<EstimationFileModel> GetEstimationFilesListByID(BaseViewModel model, string EstimationID, string EstimationDetailsID);
        bool SaveEstimationProject(EstimationProjectModel model);
        string GetEstimationProjectNextNumber(BaseViewModel model);
        EstimationModel ApprovalRequest(EstimationModel model);
        EstimationModel ApprovePendingRequest(EstimationModel model);
        EstimationModel EmailConfirmation(EstimationModel model);
        EstimationModel GetEstimationByID(BaseViewModel model, Guid id);
        List<EstimationModel> GetApprovalEstimationList(BaseViewModel model);
        bool OrderEstimationStatus(Guid id);
        bool OrderEstimationOrderLoss(Guid id);
    }

    public class EstimationService : IEstimationService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public EstimationService()
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
                    DetailsModel.SourceLanguageName = DataReader["SourceLanguageName"].ToString();
                    DetailsModel.TargetLanguageName = DataReader["TargetLanguageName"].ToString();
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
                    DetailsModel.BasicAmount = Convert.ToDecimal(DataReader["BasicAmount"].ToString());
                    DetailsModel.NumberOfDays = Convert.ToInt32(DataReader["NumberOfDays"].ToString());
                    DetailsModel.NumberOfPeople = Convert.ToInt32(DataReader["NumberOfPeople"].ToString());
                    DetailsModel.OtherAmount = Convert.ToDecimal(DataReader["OtherAmount"].ToString());
                    DetailsModel.CertificateAmount = Convert.ToDecimal(DataReader["CertificateAmount"].ToString());
                    DetailsModel.ExcludeTax = Convert.ToBoolean(DataReader["ExcludeTax"].ToString());
                    DetailsModel.Contents = DataReader["Contents"].ToString();
                    DetailsModel.LengthMinute = Convert.ToDecimal(DataReader["LengthMinute"].ToString());
                    DetailsModel.WithTranslation = Convert.ToBoolean(DataReader["WithTranslation"].ToString());
                    DetailsModel.IsMarkedForDelete = false;
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


        /* Get actual identity value for our Estimation table using a SP */
        private string GetNextRegistrationID(EstimationModel model)
        {
            SqlCommand cmd;
            long NextRegID;
            SqlParameter ReturnParameter;
            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_RetrieveNextIdentity", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TABLENAME", "Estimation");
                ReturnParameter = cmd.Parameters.Add("@NEXT_IDENTITY_VALUE", SqlDbType.BigInt);
                ReturnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                NextRegID = Convert.ToInt64(ReturnParameter.Value.ToString());
            }
            catch (Exception ex)
            {
                NextRegID = 0;
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return NextRegID.ToString();
        }


        /* Written by Ashis Kr. Das */
        public bool SaveEstimation(EstimationModel model)
        {
            SqlCommand cmd, cmd2, cmd3;
            bool isSuccessful = false;
            Guid EstimationID;

            try
            {
                string EstimationNumber = GenerateEstimationNumber(model);
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_SaveEstimation", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InquiryDate", model.InquiryDate);
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
                cmd.Parameters.AddWithValue("@ClientDepartmentID", model.ClientDepartmentID);
                cmd.Parameters.AddWithValue("@BusinessCategoryID", model.BusinessCategoryID);
                cmd.Parameters.AddWithValue("@ClientPersonInCharge", model.ClientPersonInCharge);
                cmd.Parameters.AddWithValue("@ClientEmailCC", model.ClientEmailCC);
                cmd.Parameters.AddWithValue("@ClientAddress_" + model.CurrentCulture, model.ClientAddress);
                cmd.Parameters.AddWithValue("@ClientContactNo", model.ClientContactNo);
                cmd.Parameters.AddWithValue("@ClientFax", model.ClientFax);
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
                cmd.Parameters.AddWithValue("@EstimationType", model.EstimationType);
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
                cmd.Parameters.AddWithValue("@KnownByBing", model.KnownByBing);
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
                    cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@StatementType", "Insert");
                }
                else
                {
                    EstimationID = model.ID;
                    cmd.Parameters.AddWithValue("@ID", EstimationID);
                    cmd.Parameters.AddWithValue("@EstimationNo", model.EstimationNo);
                    cmd.Parameters.AddWithValue("@RegistrationDate", model.RegistrationDate);
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

                    cmd2.Parameters.AddWithValue("@NumberOfDays", DetailsModel.NumberOfDays);
                    cmd2.Parameters.AddWithValue("@NumberOfPeople", DetailsModel.NumberOfPeople);
                    cmd2.Parameters.AddWithValue("@OtherAmount", DetailsModel.OtherAmount);
                    cmd2.Parameters.AddWithValue("@CertificateAmount", DetailsModel.CertificateAmount);
                    cmd2.Parameters.AddWithValue("@ExcludeTax", DetailsModel.ExcludeTax);
                    cmd2.Parameters.AddWithValue("@LengthMinute", DetailsModel.LengthMinute);
                    cmd2.Parameters.AddWithValue("@WithTranslation", DetailsModel.WithTranslation);

                    if (DetailsModel.IsMarkedForDelete == true)
                    {
                        EstimationDetailsID = Guid.Empty;
                        cmd2.Parameters.AddWithValue("@ID", DetailsModel.ID);
                        cmd2.Parameters.AddWithValue("@StatementType", "Delete");
                    }
                    else if (DetailsModel.ID == Guid.Empty)
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
                    cmd2.ExecuteNonQuery();             // Run stored procedure to store EstimationDetails on database
                    if (DetailsModel.IsMarkedForDelete == true)
                        goto LOOP_END;

                    cmd3 = new SqlCommand("SP_SaveEstimationFiles", _sqlConnService.CreateConnection());
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@EstimateID", EstimationID);
                    cmd3.Parameters.AddWithValue("@EstimateDetailsID", EstimationDetailsID);
                    cmd3.Parameters.AddWithValue("@FileName", DetailsModel.Document.FileName);
                    cmd3.Parameters.AddWithValue("@DownloadURL", DetailsModel.Document.DownloadURL);
                    cmd3.Parameters.AddWithValue("@TranslationText", DetailsModel.Document.TranslationText);
                    cmd3.Parameters.AddWithValue("@WordCount", DetailsModel.Document.WordCount);

                    if (DetailsModel.Document.ID == Guid.Empty)
                    {
                        cmd3.Parameters.AddWithValue("@ID", Guid.NewGuid());
                        cmd3.Parameters.AddWithValue("@StatementType", "Insert");
                    }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@ID", DetailsModel.Document.ID);
                        cmd3.Parameters.AddWithValue("@StatementType", "Update");
                    }
                    cmd3.ExecuteNonQuery();             // Run stored procedure to store EstimationFiles on database
                    
                    LOOP_END:
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


        /* Written by Ashis Kr. Das */
        public List<EstimationModel> GetAllEstimationList(BaseViewModel model)
        {
            var estimationList = new List<EstimationModel>();
            try
            {
                /* Entity Framework change tracking off. It makes EF super fast. Always set AutoDetectChangesEnabled = true; at the end of job. */
                _dbContext.Configuration.AutoDetectChangesEnabled = false;
                /**/
                var cultureId = new SqlParameter("CultureId", model.CurrentCulture);
                var estimationType = new SqlParameter("EstimationType", DBNull.Value);
                var estimationNo = new SqlParameter("EstimationNo", DBNull.Value);
                var clientID = new SqlParameter("ClientID", DBNull.Value);
                IQueryable<EstimationModel> query = _dbContext.Database.SqlQuery<EstimationModel>("exec SP_GetAllEstimation @CultureId, @EstimationType, @EstimationNo, @ClientID ",
                                                                                                cultureId, estimationType, estimationNo, clientID).OrderBy(x => x.CreatedDate).AsQueryable();
                estimationList = query.ToList();
                estimationList.ForEach(el => {
                    el.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), el.EstimationStatus);
                    el.PageButtonAttribute = new PageAttributes(el.EstimationStatusName);
                    el.CurrentCulture = model.CurrentCulture;
                    el.CurrentUserID = model.CurrentUserID;
                    el.EstimationStatusID = el.EstimationStatusID;
                });
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
                _dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return estimationList;
        }

        public List<EstimationModel> GetApprovalEstimationList(BaseViewModel model)
        {
            var estimationList = new List<EstimationModel>();
            try
            {
                /* Entity Framework change tracking off. It makes EF super fast. Always set AutoDetectChangesEnabled = true; at the end of job. */
                _dbContext.Configuration.AutoDetectChangesEnabled = false;
                /**/
                var cultureId = new SqlParameter("CultureId", model.CurrentCulture);
                var estimationType = new SqlParameter("EstimationType", DBNull.Value);
                var estimationNo = new SqlParameter("EstimationNo", DBNull.Value);
                var clientID = new SqlParameter("ClientID", DBNull.Value);
                IQueryable<EstimationModel> query = _dbContext.Database.SqlQuery<EstimationModel>("exec SP_GetApprovalEstimation @CultureId, @EstimationType, @EstimationNo, @ClientID ",
                                                                                                cultureId, estimationType, estimationNo, clientID).OrderBy(x => x.CreatedDate).AsQueryable();
                estimationList = query.ToList();
                estimationList.ForEach(el => { el.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), el.EstimationStatus); });
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
                _dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return estimationList;
        }

        public EstimationModel GetEstimationByID(BaseViewModel model, Guid id)
        {
            var estimation = new EstimationModel();
            try
            {
                /* Entity Framework change tracking off. It makes EF super fast. Always set AutoDetectChangesEnabled = true; at the end of job. */
                _dbContext.Configuration.AutoDetectChangesEnabled = false;
                /**/
                var cultureId = new SqlParameter("CultureId", model.CurrentCulture);
                var estimationType = new SqlParameter("EstimationType", DBNull.Value);
                var estimationNo = new SqlParameter("EstimationNo", DBNull.Value);
                var clientID = new SqlParameter("ClientID", DBNull.Value);
                var eID = new SqlParameter("ID", id);
                IQueryable<EstimationModel> query = _dbContext.Database.SqlQuery<EstimationModel>("exec SP_GetSpecificEstimation @CultureId, @EstimationType, @EstimationNo, @ClientID, @ID",
                                                                                                cultureId, estimationType, estimationNo, clientID, eID).AsQueryable();
                estimation = query.ToList()[0];
                estimation.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), estimation.EstimationStatus);
                estimation.PageButtonAttribute = new PageAttributes(estimation.EstimationStatusName);
                estimation.EstimationStatusID = estimation.EstimationStatusID;
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
                _dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return estimation;
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

        public EstimationModel ApprovalRequest(EstimationModel model)
        {
            var result = new EstimationModel();
            try
            {
                var estimation = _dbContext.Estimations.Find(model.ID);
                var employee = _dbContext.Employees.Find(estimation.ApprovalID);
                var emailService = new EmailService();
                var sb = new StringBuilder();
                if (estimation.EstimationStatus != (int)EstimationStatus.Waiting_for_approval)
                {
                    //Update estimation
                    estimation.EstimationStatus = (int)EstimationStatus.Waiting_for_approval;
                    _dbContext.Entry(estimation).State = EntityState.Modified;

                    //Add EstimationApproval
                    var obj = new EstimationApproval
                    {
                        ID = Guid.NewGuid(),
                        ApplicationID = model.ApplicationId,
                        ApproverID = estimation.ApprovalID,
                        EstimationID = estimation.ID,
                        Estimation = estimation,
                        OrderID = _dbContext.Orders.SingleOrDefault(e => e.Estimation.ID == estimation.ID)?.ID,
                        Description = $"Estimation No: {estimation.EstimationNo} needs to approve.",
                        Status = (int)EstimationApprovalStatus.Unread,
                        CreatedBy = model.CurrentUserID,
                        CreatedDate = DateTime.Now.ToUniversalTime()
                    };
                    _dbContext.EstimationApprovals.Add(obj);
                }
                var subject = $"Quotation email for Estimation No : {estimation.EstimationNo}";
                var empName = employee.GetType().GetProperty($"Name_{model.CurrentCulture}").GetValue(employee, null)?.ToString();
                sb.Append($"Dear {empName},"); sb.Append("<br/>");
                sb.Append($"You have an approval request for Estimation No: {estimation.EstimationNo}.");
                sb.Append($"&nbsp;<a href={model.PageUrl}>Click here</a> to go to the estimation page.");
                sb.Append("<br/><br/>");
                sb.Append("Thank you<br/>HiWork Team");
                emailService.SendEmail(employee.Email, null, null, subject, sb.ToString(), null, true);
                _dbContext.SaveChanges();
                
                result.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), estimation.EstimationStatus);
                result.PageButtonAttribute = new PageAttributes(result.EstimationStatusName);
                result.EstimationStatusID = estimation.EstimationStatus;
            }
            catch (Exception ex)
            {
                result = null;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "ApprovalRequest", message);
                throw new Exception(message);
            }
            return result;
        }

        public EstimationModel ApprovePendingRequest(EstimationModel model)
        {
            var result = new EstimationModel();
            try
            {
                var estimation = _dbContext.Estimations.Find(model.ID);
                var estimationApproval = _dbContext.EstimationApprovals.Find(model.ID);
                if (estimation.EstimationStatus != (int)EstimationStatus.Approved)
                {
                    //Update Estimation
                    estimation.EstimationStatus = (int)EstimationStatus.Approved;
                    _dbContext.Entry(estimation).State = EntityState.Modified;

                    //Update EstimationApproval
                    estimationApproval.Status = (int)EstimationApprovalStatus.Approved;
                    _dbContext.Entry(estimationApproval).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    result.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), estimation.EstimationStatus);
                    result.PageButtonAttribute = new PageAttributes(result.EstimationStatusName);
                    result.EstimationStatusID = estimation.EstimationStatus;
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "ApprovePendingRequest", message);
                throw new Exception(message);
            }
            return result;
        }

        public EstimationModel EmailConfirmation(EstimationModel model)
        {
            var result = new EstimationModel();
            try
            {
                var estimation = _dbContext.Estimations.Find(model.ID);
                var employee = _dbContext.Employees.Find(estimation.ApprovalID);
                var coordinator = _dbContext.Employees.Find(model.CoordinatorID ?? new Guid());
                var emailService = new EmailService();
                var emials = new List<KeyValuePair<string, Employee>>();
                if (estimation.EstimationStatus != (int)EstimationStatus.Ordered)
                {
                    //Update Estimation
                    estimation.EstimationStatus = (int)EstimationStatus.Ordered;
                    _dbContext.Entry(estimation).State = EntityState.Modified;

                    //Send confirmation email
                    var dictonary = new Dictionary<string, Employee>()
                    {
                        {employee.Email, employee},
                        {coordinator.Email, coordinator}
                    };
                    emials.AddRange(dictonary);
                    var subject = $"Quotation confirmation email for Estimation No : {estimation.EstimationNo}";

                    emials.ForEach(email =>
                    {
                        if (!string.IsNullOrEmpty(email.Key))
                        {
                            var sb = new StringBuilder();
                            var empName = email.Value.GetType().GetProperty($"Name_{model.CurrentCulture}").GetValue(email.Value, null)?.ToString();
                            sb.Append($"Dear {empName},"); sb.Append("<br/>");
                            sb.Append($"You approved the request for Estimation No: {estimation.EstimationNo}.");
                            sb.Append($"&nbsp;<a href={model.PageUrl}>Click here</a> to go to the estimation page.");
                            sb.Append("<br/><br/>");
                            sb.Append("Thank you<br/>HiWork Team");
                            emailService.SendEmail(email.Key, null,null, subject, sb.ToString(), null, true);
                        }
                    });
                    _dbContext.SaveChanges();

                    result.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), estimation.EstimationStatus);
                    result.PageButtonAttribute = new PageAttributes(result.EstimationStatusName);
                    result.EstimationStatusID = estimation.EstimationStatus;
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EmailConfirmation", message);
                throw new Exception(message);
            }
            return result;
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

        public bool OrderEstimationStatus(Guid id)
        {
            bool result = false;
            try
            {
                var estimation = _dbContext.Estimations.Find(id);
            estimation.EstimationStatus = 6;
            _dbContext.SaveChanges();
                result= true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public bool OrderEstimationOrderLoss(Guid id)
        {
            bool result = false;
            try
            {
                var estimation = _dbContext.Estimations.Find(id);
                estimation.EstimationStatus = 15; 
                _dbContext.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}

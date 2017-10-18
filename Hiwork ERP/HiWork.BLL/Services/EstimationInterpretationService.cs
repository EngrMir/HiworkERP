/* ******************************************************************************************************************
 * Service for Estimation Interpretation Service
 * Programmed by    :   Md. Al-Amin Hossain (b-Bd_14 Hossain)
 * Date             :   22-Sep-2017
 * *****************************************************************************************************************/

using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils;
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
    public partial interface IEstimationInterpretationService
    {
        bool SaveEstimation(EstimationModel model);
        List<EstimationDetailsModel> GetEstimationDetailsByID(EstimationDetailsModel model , Guid id);
        List<EstimationFileModel> GetFileListByID(EstimationFileModel model, Guid id);
        List<OrderModel> GetOrderIDByID(EstimationModel model, Guid id);
        List<OrderStaffAllowanceModel> GetStaffAllowanceListByID(Guid id);
    }
    class EstimationInterpretationService : IEstimationInterpretationService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public EstimationInterpretationService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
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

        private string GenerateEstimationNumber(EstimationModel model)
        {
            string NextRegistrationID;
            string AppCode;
            IApplicationService appService = new ApplicationService(new ApplicationRepository(new UnitOfWork()));

            NextRegistrationID = GetNextRegistrationID(model);
            AppCode = appService.GetApplicationCode(model.ApplicationId);
            return Helper.GenerateUniqueID(AppCode, NextRegistrationID);
        }

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
                cmd.Parameters.AddWithValue("@EstimationType",model.EstimationType);
                //cmd.Parameters.AddWithValue("@EstimationStatus", model.EstimationStatus);
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
                cmd.Parameters.AddWithValue("@Remarks_" + model.CurrentCulture, model.Remarks);
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
                cmd.Parameters.AddWithValue("@IsPromotion", model.IsPromotion);
                cmd.Parameters.AddWithValue("@IsSpecialPayment", model.IsSpecialPayment);
                cmd.Parameters.AddWithValue("@IsPastComplaint", model.IsPastComplaint);
                cmd.Parameters.AddWithValue("@IsExpertise", model.IsExpertise);
                cmd.Parameters.AddWithValue("@IsOnGoingTask", model.IsOnGoingTask);
                cmd.Parameters.AddWithValue("@IsOverSeas", model.IsOverseas);
                cmd.Parameters.AddWithValue("@IsJapan", model.IsJapan);
                cmd.Parameters.AddWithValue("@CountryID", model.CountryID);
                cmd.Parameters.AddWithValue("@KnownByIntroduction", model.KnownByIntroduction);
                cmd.Parameters.AddWithValue("@KnownIntroductionText", model.KnownIntroductionText);
                cmd.Parameters.AddWithValue("@AttachedMaterialFileName", model.AttachedMaterialFileName);
                cmd.Parameters.AddWithValue("@AttachedMaterialDownloadURL", model.AttachedMaterialDownloadURL);
                cmd.Parameters.AddWithValue("@IsContentAll", model.IsContentAll);
                cmd.Parameters.AddWithValue("@IsPerformance", model.IsPerformance);
                cmd.Parameters.AddWithValue("@IsAccuracy", model.IsAccuracy);
                cmd.Parameters.AddWithValue("@IsLocal", model.IsLocal);
                cmd.Parameters.AddWithValue("@QuotationNotes_"+model.CurrentCulture, model.QuotationNotes);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@UnitID", model.UnitID);
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
                    DetailsModel.StartTime = DetailsModel.NewStartTime;
                    DetailsModel.FinishTime = DetailsModel.NewFinishTime;

                    cmd2 = new SqlCommand("SP_SaveEstimationDetails", _sqlConnService.CreateConnection());
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@EstimationID", EstimationID);
                    cmd2.Parameters.AddWithValue("@SourceLanguageID", DetailsModel.SourceLanguageID);
                    cmd2.Parameters.AddWithValue("@TargetLanguageID", DetailsModel.TargetLanguageID);
                    cmd2.Parameters.AddWithValue("@ServiceType", DetailsModel.ServiceType);
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
                    cmd2.Parameters.AddWithValue("@AdditionalTime", DetailsModel.AdditionalTime);
                    cmd2.Parameters.AddWithValue("@ExtensionTime", DetailsModel.ExtensionTime);
                    cmd2.Parameters.AddWithValue("@ExtraTime", DetailsModel.ExtraTime);
                    cmd2.Parameters.AddWithValue("@LateNightTime", DetailsModel.LateNightTime);
                    cmd2.Parameters.AddWithValue("@TransferTime", DetailsModel.TransferTime);  
                    cmd2.Parameters.AddWithValue("@BasicAmount", DetailsModel.BasicAmount);
                    cmd2.Parameters.AddWithValue("@ExtensionAmount", DetailsModel.ExtensionAmount);
                    cmd2.Parameters.AddWithValue("@ExtraAmount", DetailsModel.ExtraAmount);
                    cmd2.Parameters.AddWithValue("@LateAtNightAmount", DetailsModel.LateAtNightAmount);
                    cmd2.Parameters.AddWithValue("@TransferAmount", DetailsModel.TransferAmount);
                    cmd2.Parameters.AddWithValue("@NumberOfDays", DetailsModel.NumberOfDays);
                    cmd2.Parameters.AddWithValue("@NumberOfPeople", DetailsModel.NumberOfPeople);
                    cmd2.Parameters.AddWithValue("@OtherAmount", DetailsModel.OtherAmount);
                    cmd2.Parameters.AddWithValue("@CertificateAmount", DetailsModel.CertificateAmount);
                    cmd2.Parameters.AddWithValue("@ExcludeTax", DetailsModel.ExcludeTax);
                    cmd2.Parameters.AddWithValue("@LengthMinute", DetailsModel.LengthMinute);
                    cmd2.Parameters.AddWithValue("@WithTranslation", DetailsModel.WithTranslation);
                    cmd2.Parameters.AddWithValue("@StartDate", DetailsModel.StartDate);
                    cmd2.Parameters.AddWithValue("@CompletionDate", DetailsModel.CompletionDate);
                    cmd2.Parameters.AddWithValue("@StartTime", DetailsModel.StartTime);
                    cmd2.Parameters.AddWithValue("@FinishTime", DetailsModel.FinishTime);
                    cmd2.Parameters.AddWithValue("@Contents", DetailsModel.Contents);
                    cmd2.Parameters.AddWithValue("@IsOverseas", DetailsModel.IsOverseas);
                    cmd2.Parameters.AddWithValue("@Total", DetailsModel.Total);
                    cmd2.Parameters.AddWithValue("@TotalAfterDiscount", DetailsModel.TotalAfterDiscount);
                    cmd2.Parameters.AddWithValue("@DiscountedPrice", DetailsModel.DiscountedPrice);
                    cmd2.Parameters.AddWithValue("@DiscountRate", DetailsModel.DiscountRate);
                    cmd2.Parameters.AddWithValue("@UnitPriceSubTotal", DetailsModel.UnitPriceSubTotal);
                    cmd2.Parameters.AddWithValue("@PaymentRate", DetailsModel.PaymentRate);
                    cmd2.Parameters.AddWithValue("@ExpectedPayment", DetailsModel.ExpectedPayment);



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

                    isSuccessful = true;
                LOOP_END:;
                }

               foreach(OrderStaffAllowanceModel collection in model.CollectionFee)
                {
                    if (collection.SubTotal > 0 || collection.IsMarkedForDelete==true)
                    {
                        Guid AllowanceID;
                        cmd3 = new SqlCommand("SP_SaveOrderStaffAllowance", _sqlConnService.CreateConnection());
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@ItemName", collection.ItemName);
                        cmd3.Parameters.AddWithValue("@UnitPrice", collection.UnitPrice);
                        cmd3.Parameters.AddWithValue("@NoOfPersons", collection.NoOfPersons);
                        cmd3.Parameters.AddWithValue("@NoOfDays", collection.NoOfDays);
                        cmd3.Parameters.AddWithValue("@SubTotal", collection.SubTotal);
                        cmd3.Parameters.AddWithValue("@AllowanceType", collection.AllowanceType);
                        cmd3.Parameters.AddWithValue("@IsCompleteSet", collection.IsCompleteSet);
                        cmd3.Parameters.AddWithValue("@IsExclTax", collection.IsExclTax);

                        if (collection.IsMarkedForDelete == true)
                        {
                            //Delete the record
                            cmd3.Parameters.AddWithValue("@ID", collection.ID);
                            cmd3.Parameters.AddWithValue("@StatementType", "Delete");
                        }
                        else if (collection.EstimationID == Guid.Empty)
                        {
                            // Insert the record
                            collection.EstimationID = EstimationID;
                            AllowanceID = Guid.NewGuid();
                            collection.ID = AllowanceID;
                            cmd3.Parameters.AddWithValue("@ID", collection.ID);
                            cmd3.Parameters.AddWithValue("@EstimationID", collection.EstimationID);
                            cmd3.Parameters.AddWithValue("@StatementType", "Insert");
                        }
                        else
                        {
                            // Update the record
                            cmd3.Parameters.AddWithValue("@ID", collection.ID);
                            cmd3.Parameters.AddWithValue("@EstimationID", collection.EstimationID);
                            cmd3.Parameters.AddWithValue("@StatementType", "Update");

                        }

                        cmd3.ExecuteNonQuery();

                    }
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

        public List<EstimationDetailsModel> GetEstimationDetailsByID(EstimationDetailsModel model, Guid id)
        {
            List<EstimationDetail> MasterDataList;
            Estimation Estimation;
            List<EstimationDetailsModel> estimationlist = new List<EstimationDetailsModel>();
            EstimationDetailsModel estimationtemplate = new EstimationDetailsModel();
            try
            {
                MasterDataList = _dbContext.EstimationDetails.ToList();
                //Estimation = MasterDataList.Find(item => item.ID == model.ID);
                if (MasterDataList != null)
                {
                    foreach (EstimationDetail a in MasterDataList)
                    {
                        if (a.EstimationID == id)
                        {
                            estimationtemplate = Mapper.Map < EstimationDetail, EstimationDetailsModel>(a);
                            estimationlist.Add(estimationtemplate);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return estimationlist;

        }

        public List<EstimationFileModel> GetFileListByID(EstimationFileModel model, Guid id)
        {
            List<EstimationFile> MasterDataList;
            List<EstimationFileModel> filelist = new  List <EstimationFileModel> ();
            EstimationFileModel filetemplate = new EstimationFileModel();
            try
            {
                MasterDataList = _dbContext.EstimationFiles.ToList();

                if(MasterDataList !=null)
                {
                    foreach(EstimationFile a in MasterDataList)
                    {
                        if(a.EstimateID==id)
                        {
                            filetemplate=Mapper.Map<EstimationFile ,EstimationFileModel > (a);
                            filelist.Add(filetemplate);
                        }
                    }
                }
            }

            catch(Exception ex)
            {

            }
            return filelist;
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

        public List<OrderModel> GetOrderIDByID(EstimationModel model, Guid id)
        {
            List<OrderModel> datalist = new List<OrderModel>();
            List<Order> MasterDataList;
            OrderModel datatemplate;
            try
            {
                MasterDataList = _dbContext.Orders.ToList();  
                  foreach (Order a in MasterDataList)
                    {
                    if (a.EstimationID==id )
                    {
                        datatemplate = Mapper.Map<Order, OrderModel>(a);
                        datalist.Add(datatemplate);
                    }
                    }
                return datalist;
            }
            catch(Exception ex)
            {
                throw new Exception( ex.Message);
            }

        }

        public List<OrderStaffAllowanceModel> GetStaffAllowanceListByID(Guid id)
        {
            List<Order_StaffAllowance> masterdatalist = new List<Order_StaffAllowance>();
            List<OrderStaffAllowanceModel> datalist = new List<OrderStaffAllowanceModel>();
            OrderStaffAllowanceModel template;
            try
            {
                masterdatalist = _dbContext.Order_StaffAllowance.ToList();
                foreach (Order_StaffAllowance data in masterdatalist)
                {
                    if (data.EstimationID == id)
                    {
                        template = Mapper.Map<Order_StaffAllowance, OrderStaffAllowanceModel>(data);
                        datalist.Add(template);
                    }
                }

                return datalist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}

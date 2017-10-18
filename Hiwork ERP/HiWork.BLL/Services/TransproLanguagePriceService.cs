

/* ******************************************************************************************************************
 * Service for TransproLanguagePriceCategory & TransproLanguagePriceDetails Entity
 * Date             :   14-September-2017
 * By               :   Ashis Kr. Das
 * *****************************************************************************************************************/


using HiWork.BLL.Models;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Repositories;

namespace HiWork.BLL.Services
{
    public interface ITransproLanguagePriceService
    {
        List<TransproLanguagePriceCategoryModel> GetTransproLanguagePriceCategoryList(TransproLanguagePriceCategoryQueryModel model);
        List<TransproLanguagePriceDetailsModel> GetTransproLanguagePriceDetailsList(BaseViewModel model, string PriceCategoryID);
        List<TransproLanguagePriceCategoryModel> SaveTransproLanguagePriceCategory(TransproLanguagePriceCategoryModel model);
        List<TransproLanguagePriceCategoryModel> DeleteTransproLanguagePriceCategory(TransproLanguagePriceCategoryModel PriceCategoryModel);

        TransproLanguagePriceViewListModel? GetTransproLanguagePriceViewList(BaseViewModel model);
    }

    public class TransproLanguagePriceService : ITransproLanguagePriceService
    {
        private readonly ISqlConnectionService _sqlConnService, _sqlConnService2;

        public TransproLanguagePriceService()
        {
            _sqlConnService = new SqlConnectionService();
            _sqlConnService2 = new SqlConnectionService();
        }



        /*******************************************************************************************************************************
         * *****************************************************************************************************************************
         * *****************************************************************************************************************************
         * *****************************************************************************************************************************/


        public List<TransproLanguagePriceCategoryModel> DeleteTransproLanguagePriceCategory(TransproLanguagePriceCategoryModel PriceCategoryModel)
        {
            SqlCommand cmd;

            try
            {
                _sqlConnService.OpenConnection();

                cmd = new SqlCommand("SP_SaveDeleteTransproLanguagePriceDetails", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PriceCategoryID", PriceCategoryModel.ID);
                cmd.Parameters.AddWithValue("@StatementType", "DeleteByPriceCategoryID");
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SP_SaveDeleteTransproLanguagePriceCategory", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", PriceCategoryModel.ID);
                cmd.Parameters.AddWithValue("@StatementType", "Delete");
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(PriceCategoryModel.CurrentUserID, "Transpro", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }

            TransproLanguagePriceCategoryQueryModel BaseModel = new TransproLanguagePriceCategoryQueryModel();
            BaseModel.CurrentCulture = PriceCategoryModel.CurrentCulture;
            BaseModel.CurrentUserID = PriceCategoryModel.CurrentUserID;
            return this.GetTransproLanguagePriceCategoryList(BaseModel);
        }


        public List<TransproLanguagePriceCategoryModel> GetTransproLanguagePriceCategoryList(TransproLanguagePriceCategoryQueryModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            TransproLanguagePriceCategoryModel CategoryModel;
            List<TransproLanguagePriceCategoryModel> CategoryList = new List<TransproLanguagePriceCategoryModel>();
            BaseViewModel BaseModel = new BaseViewModel();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetTransproLanguagePriceCategory", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceLanguageID", model.GetSourceLanguageID());
                cmd.Parameters.AddWithValue("@TargetLanguageID", model.GetTargetLanguageID());
                cmd.Parameters.AddWithValue("@SpecialityFieldID", model.GetSpecialityFieldID());
                cmd.Parameters.AddWithValue("@SubSpecialityFieldID", model.GetSubSpecilityFieldID());
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    CategoryModel = new TransproLanguagePriceCategoryModel();
                    CategoryModel.ID = Guid.Parse(DataReader["ID"].ToString());
                    CategoryModel.SourceLanguageID = Guid.Parse(DataReader["SourceLanguageID"].ToString());
                    CategoryModel.TargetLanguageID = Guid.Parse(DataReader["TargetLanguageID"].ToString());
                    CategoryModel.SpecialityFieldID = Guid.Parse(DataReader["SpecialityFieldID"].ToString());
                    CategoryModel.SubSpecialityFieldID = Guid.Parse(DataReader["SubSpecialityFieldID"].ToString());
                    CategoryModel.CurrencyID = Convert.ToInt64(DataReader["CurrencyID"].ToString());
                    CategoryModel.SourceLanguageName = DataReader["SourceLanguageName"].ToString();
                    CategoryModel.TargetLanguageName = DataReader["TargetLanguageName"].ToString();
                    CategoryModel.CurrencyName = DataReader["CurrencyName"].ToString();
                    CategoryModel.CurrencySymbol = DataReader["CurrencySymbol"].ToString();
                    CategoryModel.SpecializedFieldName = DataReader["SpecializedFieldName"].ToString();
                    CategoryModel.SubSpecializedFieldName = DataReader["SubSpecializedFieldName"].ToString();
                    CategoryModel.Description = DataReader["Description"].ToString();
                    CategoryModel.WordPerPage = Convert.ToInt64(DataReader["WordPerPage"].ToString());
                    CategoryModel.IsLightPrice = Convert.ToBoolean(DataReader["IsLightPrice"].ToString());
                    CategoryModel.IsBusinessPrice = Convert.ToBoolean(DataReader["IsBusinessPrice"].ToString());
                    CategoryModel.IsExpertPrice = Convert.ToBoolean(DataReader["IsExpertPrice"].ToString());
                    CategoryModel.IsActive = Convert.ToBoolean(DataReader["IsActive"].ToString());
                    CategoryModel.IsDeleted = Convert.ToBoolean(DataReader["IsDeleted"].ToString());
                    CategoryModel.CurrentUserID = model.CurrentUserID;
                    CategoryModel.CurrentCulture = model.CurrentCulture;
                    BaseModel.CurrentCulture = model.CurrentCulture;
                    BaseModel.CurrentUserID = model.CurrentUserID;
                    CategoryModel.PriceDetailsList = this.GetTransproLanguagePriceDetailsList(BaseModel, CategoryModel.ID.ToString());
                    CategoryList.Add(CategoryModel);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Transpro", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return CategoryList;
        }

        public List<TransproLanguagePriceDetailsModel> GetTransproLanguagePriceDetailsList(BaseViewModel model, string PriceCategoryID)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            TransproLanguagePriceDetailsModel DetailsModel;
            List<TransproLanguagePriceDetailsModel> DetailsList = new List<TransproLanguagePriceDetailsModel>();

            try
            {
                _sqlConnService2.OpenConnection();
                cmd = new SqlCommand("SP_GetTransproLanguagePriceDetails", _sqlConnService2.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PriceCategoryID", Guid.Parse(PriceCategoryID));
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    DetailsModel = new TransproLanguagePriceDetailsModel();
                    DetailsModel.ID = Guid.Parse(DataReader["ID"].ToString());
                    DetailsModel.PriceCategoryID = Guid.Parse(DataReader["PriceCategoryID"].ToString());
                    DetailsModel.DeliveryPlanID = Convert.ToInt64(DataReader["DeliveryPlanID"].ToString());
                    DetailsModel.DeliveryPlanName = DataReader["DeliveryPlanName"].ToString();
                    DetailsModel.DeliveryPlanType = Convert.ToInt32(DataReader["DeliveryPlanType"].ToString());
                    DetailsModel.DeliveryPlanTime = Convert.ToInt32(DataReader["DeliveryPlanTime"].ToString());
                    DetailsModel.LightPrice = Convert.ToDecimal(DataReader["LightPrice"].ToString());
                    DetailsModel.BusinessPrice = Convert.ToDecimal(DataReader["BusinessPrice"].ToString());
                    DetailsModel.ExpertPrice = Convert.ToDecimal(DataReader["ExpertPrice"].ToString());
                    DetailsModel.CurrentUserID = model.CurrentUserID;
                    DetailsModel.CurrentCulture = model.CurrentCulture;
                    DetailsModel.IsDefaultForView = Convert.ToBoolean(DataReader["IsDefaultForView"].ToString());
                    DetailsModel.SortBy = Convert.ToInt32(DataReader["SortBy"].ToString());
                    DetailsModel.IsMarkedForDelete = false;
                    DetailsList.Add(DetailsModel);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Transpro", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService2.CloseConnection();
            }
            return DetailsList;
        }

        public List<TransproLanguagePriceCategoryModel> SaveTransproLanguagePriceCategory(TransproLanguagePriceCategoryModel model)
        {
            SqlCommand cmd, cmd2;
            Guid PriceCategoryID;
            string StatementType;

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_SaveDeleteTransproLanguagePriceCategory", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceLanguageID", model.SourceLanguageID);
                cmd.Parameters.AddWithValue("@TargetLanguageID", model.TargetLanguageID);
                cmd.Parameters.AddWithValue("@SpecialityFieldID", model.SpecialityFieldID);
                cmd.Parameters.AddWithValue("@SubSpecialityFieldID", model.SubSpecialityFieldID);
                cmd.Parameters.AddWithValue("@Description_" + model.CurrentCulture, model.Description);
                cmd.Parameters.AddWithValue("@WordPerPage", model.WordPerPage);
                cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                cmd.Parameters.AddWithValue("@IsLightPrice", model.IsLightPrice);
                cmd.Parameters.AddWithValue("@IsBusinessPrice", model.IsBusinessPrice);
                cmd.Parameters.AddWithValue("@IsExpertPrice", model.IsExpertPrice);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);

                if (model.ID == Guid.Empty)
                {
                    PriceCategoryID = Guid.NewGuid();
                    model.ID = PriceCategoryID;
                    StatementType = "Insert";
                }
                else
                {
                    PriceCategoryID = model.ID;
                    StatementType = "Update";
                }
                cmd.Parameters.AddWithValue("@ID", PriceCategoryID);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                cmd.ExecuteNonQuery();

                foreach (TransproLanguagePriceDetailsModel PriceDetailsModel in model.PriceDetailsList)
                {
                    cmd2 = new SqlCommand("SP_SaveDeleteTransproLanguagePriceDetails", _sqlConnService.CreateConnection());
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@PriceCategoryID", PriceCategoryID);
                    cmd2.Parameters.AddWithValue("@DeliveryPlanID", PriceDetailsModel.DeliveryPlanID);
                    cmd2.Parameters.AddWithValue("@LightPrice", PriceDetailsModel.LightPrice);
                    cmd2.Parameters.AddWithValue("@BusinessPrice", PriceDetailsModel.BusinessPrice);
                    cmd2.Parameters.AddWithValue("@ExpertPrice", PriceDetailsModel.ExpertPrice);
                    cmd2.Parameters.AddWithValue("@IsDefaultForView", PriceDetailsModel.IsDefaultForView);
                    cmd2.Parameters.AddWithValue("@SortBy", PriceDetailsModel.SortBy);

                    if (PriceDetailsModel.IsMarkedForDelete == true)
                    {
                        StatementType = "Delete";
                    }
                    else if (PriceDetailsModel.ID == Guid.Empty)
                    {
                        PriceDetailsModel.ID = Guid.NewGuid();
                        StatementType = "Insert";
                    }
                    else
                    {
                        StatementType = "Update";
                    }
                    cmd2.Parameters.AddWithValue("@ID", PriceDetailsModel.ID);
                    cmd2.Parameters.AddWithValue("@StatementType", StatementType);
                    cmd2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Transpro", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            
            TransproLanguagePriceCategoryQueryModel BaseModel = new TransproLanguagePriceCategoryQueryModel();
            BaseModel.CurrentCulture = model.CurrentCulture;
            BaseModel.CurrentUserID = model.CurrentUserID;
            return this.GetTransproLanguagePriceCategoryList(BaseModel);
        }


        public TransproLanguagePriceViewListModel? GetTransproLanguagePriceViewList(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            ILanguageService LangService = new LanguageService(new LanguageRepository(uWork));
            IEstimationSpecializedFieldService SpeService = new EstimationSpecializedFieldService(new EstimationSpecializedFieldRepository(uWork));
            IEstimationSubSpecializedFieldService SubSpeService
                                = new EstimationSubSpecializedFieldService(new EstimationSubSpecializedFieldRepository(uWork));
            ICurrencyService CurService = new CurrencyService(new CurrencyRepository(uWork));
            ITransproDeliveryPlanService DelService = new TransproDeliveryPlanService(new TransproDeliveryPlanRepository(uWork));

            TransproLanguagePriceViewListModel ViewModel;
            ViewModel.CurrentUserID = model.CurrentUserID;
            ViewModel.CurrentCulture = model.CurrentCulture;
            ViewModel.LanguageList = LangService.GetAllLanguageList(model);
            ViewModel.SpecializedFieldList = SpeService.GetAllEstimationSpecializedFieldList(model);
            ViewModel.SubSpecializedFieldList = SubSpeService.GetAllEstimationSubSpecializedFieldList(model);
            ViewModel.CurrencyList = CurService.GetAllCurrencyList(model);
            ViewModel.DeliveryPlanList = DelService.GetAllTransproDeliveryType(model);
            return ViewModel;
        }
    }
}

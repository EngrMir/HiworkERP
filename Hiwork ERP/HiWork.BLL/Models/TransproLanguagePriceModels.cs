

/* ******************************************************************************************************************
 * Data Models for TransproLanguagePriceCategory & TransproLanguagePriceDetails Entity
 * Date             :   14-September-2017
 * By               :   Ashis Kr. Das
 * *****************************************************************************************************************/



using System;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{

    public class TransproLanguagePriceCategoryModel : BaseDomainModel<TransproLanguagePriceCategoryModel>
    {
        public Guid ID { get; set; }
        public Guid SourceLanguageID { get; set; }
        public Guid TargetLanguageID { get; set; }
        public Guid SpecialityFieldID { get; set; }
        public Guid SubSpecialityFieldID { get; set; }
        public long CurrencyID { get; set; }
        public string SourceLanguageName { get; set; }
        public string TargetLanguageName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string SpecializedFieldName { get; set; }
        public string SubSpecializedFieldName { get; set; }
        public bool IsLightPrice { get; set; }
        public bool IsBusinessPrice { get; set; }
        public bool IsExpertPrice { get; set; }
        public string Description { get; set; }
        public long WordPerPage { get; set; }
        
        public List<TransproLanguagePriceDetailsModel> PriceDetailsList { get; set; }
    }


    public class TransproLanguagePriceDetailsModel
    {
        public Guid ID { get; set; }
        public Guid PriceCategoryID { get; set; }
        public long DeliveryPlanID { get; set; }
        public string DeliveryPlanName { get; set; }
        public int DeliveryPlanType { get; set; }
        public int DeliveryPlanTime { get; set; }
        public decimal LightPrice { get; set; }
        public decimal BusinessPrice { get; set; }
        public decimal ExpertPrice { get; set; }
        public bool IsDefaultForView { get; set; }
        public int SortBy { get; set; }
        
        public long CurrentUserID { get; set; }
        public string CurrentCulture { get; set; }
        public bool IsMarkedForDelete { get; set; }
    }


    public struct TransproLanguagePriceViewListModel
    {
        public string CurrentCulture;
        public long CurrentUserID;
        public List<LanguageModel> LanguageList;
        public List<EstimationSpecializedFieldModel> SpecializedFieldList;
        public List<EstimationSubSpecializedFieldModel> SubSpecializedFieldList;
        public List<CurrencyModel> CurrencyList;
        public List<TransproDeliveryPlanSettingModel> DeliveryPlanList;
    }

    public class TransproLanguagePriceCategoryQueryModel : BaseDomainModel<TransproLanguagePriceCategoryQueryModel>
    {
        public string SourceLanguageID { get; set; }
        public string TargetLanguageID { get; set; }
        public string SpecialityFieldID { get; set; }
        public string SubSpecialityFieldID { get; set; }

        public object GetSourceLanguageID()
        {
            if (SourceLanguageID == null || SourceLanguageID == string.Empty)
                return DBNull.Value;
            else
                return Guid.Parse(SourceLanguageID);
        }

        public object GetTargetLanguageID()
        {
            if (TargetLanguageID == null || TargetLanguageID == string.Empty)
                return DBNull.Value;
            else
                return Guid.Parse(TargetLanguageID);
        }

        public object GetSpecialityFieldID()
        {
            if (SpecialityFieldID == null || SpecialityFieldID == string.Empty)
                return DBNull.Value;
            else
                return Guid.Parse(SpecialityFieldID);
        }

        public object GetSubSpecilityFieldID()
        {
            if (SubSpecialityFieldID == null || SubSpecialityFieldID == string.Empty)
                return DBNull.Value;
            else
                return Guid.Parse(SubSpecialityFieldID);
        }
    }
}

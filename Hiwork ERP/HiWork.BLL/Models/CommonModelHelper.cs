using HiWork.DAL.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HiWork.BLL.Models
{
    public class CommonModelHelper
    {
        public CommonModelHelper(){}
        //Single entity goes here
        public string Culture { get; set; }
        public long CurrentUserID { get; set; }
        public long ApplicationID { get; set; }

        //Object entity goes here
        public CulturalItem CulturalItem { get; set; }
        public Estimation Estimation { get; set; }
        public EstimationAction EstimationAction { get; set; }
        public EstimationNarrationExpense EstimationNarrationExpense { get; set; }
        public Order Order { get; set; }

        //List of object entity goes here
        public List<EstimationDetail> EstimationDetails { get; set; }
        public List<CompanyModel> CompanyModels { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<EstimationDeliveryFileType> FileTypes { get; set; }
        public List<EstimationWorkContent> WorkContents { get; set; }
        public List<AdvancedStaffSearchModel> assm { get; set; }

        public List<EstimationCompetency> estimationCompetencies { get; set; }
    }

    public class CulturalItem
    {
        public string ClientAddress { get; set; }
        public string BillingCompanyName { get; set; }
        public string BillingAddress { get; set; }
        public string DeliveryCompanyName { get; set; }
        public string DeliveryAddress { get; set; }
        public string CoordinatorNotes { get; set; }
        public string Remarks { get; set; }
        public string OtherItemName { get; set; }
        public string ActionDetails { get; set; }
        public string QuotationNotes { get; set; }
        public string CoordinatorPrecautions { get; set; }
        public string NotesToStaff { get; set; }
        public string ComplainDetails { get; set; }
        public string AccountingRelatedMemo { get; set; }
        public string DirectManuscript { get; set; }
    }
    
    public class CommonModelHelper_Transcription : CommonModelHelper
    {
        public new List<EstimationDetailsModel> EstimationDetails { get; set; }
    }
}

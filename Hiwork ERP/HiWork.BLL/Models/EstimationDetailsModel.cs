

using System;

namespace HiWork.BLL.Models
{
    public class EstimationDetailsModel
    {

        public bool IsMarkedForDelete { get; set; }


        public Guid ID { get; set; }
        public Guid EstimationID { get; set; }
        public Guid SourceLanguageID { get; set; }
        public Guid TargetLanguageID { get; set; }
        public Guid ServiceTypeID { get; set; }
        public Guid ServiceType { get; set; }
        public string ServiceName { get; set; }

        public string SourceLanguageName { get; set; }
        public string TargetLanguageName { get; set; }

        public decimal UnitPrice1 { get; set; }
        public int PageCount1 { get; set; }
        public decimal Discount1 { get; set; }
        public decimal UnitPrice2 { get; set; }
        public int PageCount2 { get; set; }
        public decimal Discount2 { get; set; }
        public decimal UnitPrice3 { get; set; }
        public int PageCount3 { get; set; }
        public decimal Discount3 { get; set; }
        public decimal UnitPrice4 { get; set; }
        public int PageCount4 { get; set; }
        public decimal Discount4 { get; set; }
        public decimal UnitPrice5 { get; set; }
        public int PageCount5 { get; set; }
        public decimal Discount5 { get; set; }
        public decimal BasicTime { get; set; }
        public decimal AdditionalBasicAmount { get; set; }
        public int ExtraTime { get; set; }
        public decimal AdditionalTime { get; set; }
        public decimal ExtensionTime { get; set; }
        public decimal LateNightTime { get; set; }


        public decimal TransferTime { get; set; }
        public decimal BasicAmount { get; set; }
        public decimal ExtensionAmount { get; set; }
        public decimal ExtraAmount { get; set; }
        public decimal LateAtNightAmount { get; set; }
        //public decimal MovingAmount { get; set; }
        public decimal TransferAmount { get; set; }
        public int NumberOfDays { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal OtherAmount { get; set; }
        public decimal CertificateAmount { get; set; }

        public bool ExcludeTax { get; set; }
        public EstimationFileModel Document { get; set; }

        public string Contents { get; set; }
        // added here
        public decimal LengthMinute { get; set; }
        public bool WithTranslation { get; set; }
        public decimal UnitPriceTime { get; set; }
        public decimal UnitPriceSubTotal { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal EstimationPrice { get; set; }
        public decimal StudioPrice { get; set; }
        public decimal StudioPriceTime { get; set; }
        public decimal EditPrice { get; set; }
        public decimal EditPriceTime { get; set; }
        public decimal StudioPriceSubTotal { get; set; }
        public decimal StudioPriceDiscountRate { get; set; }
        public decimal StudioDiscountedPrice { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public decimal PaymentRate { get; set; }
        public decimal ExpectedPayment { get; set; }
        public decimal Total { get; set; }

        public decimal TransportationUnitprice { get; set; }
        public decimal TransportationNumberOfPersons { get; set; }
        public decimal TransportationNumberOfDays { get; set; }
        public decimal TransportationTotal { get; set; }
        public decimal AccomodationUnitprice { get; set; }
        public decimal AccomodationNumberOfPersons { get; set; }
        public decimal AccomodationNumberOfDays { get; set; }
        public decimal AccomodationTotal { get; set; }
        public decimal MealUnitprice { get; set; }
        public decimal MealNumberOfPersons { get; set; }
        public decimal MealNumberOfDays { get; set; }
        public decimal MealTotal { get; set; }
        public decimal AllowanceUnitprice { get; set; }
        public decimal AllowanceNumberOfPersons { get; set; }
        public decimal AllowanceNumberOfDays { get; set; }
        public decimal AllowanceTotal { get; set; }
        public decimal TransportationCompleteSet { get; set; }
        public decimal TransportationExclTax { get; set; }
        public decimal AccomodationCompleteSet { get; set; }
        public decimal AccomodationExclTax { get; set; }
        public decimal MealCompleteSet { get; set; }
        public decimal MealExclTax { get; set; }
        public decimal AllowanceCompleteSet { get; set; }
        public decimal AllowanceExclTax { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string NewStartTime { get; set; }
        public string NewFinishTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool IsOverseas { get; set; }
        

    }
}

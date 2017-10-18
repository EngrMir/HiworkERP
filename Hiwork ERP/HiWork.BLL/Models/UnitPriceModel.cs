using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
   public class UnitPriceModel :BaseDomainModel<UnitPriceModel>
    {
        public long ID { get; set; }
        public long UnitID { get; set; }
        public int EstimationTypeID { get; set; }
        public Guid SourceLanguageID { get; set; }
        public Guid TargetLanguageID { get; set; }
        public decimal GeneralUnitPrice { get; set; }
        public decimal SpecialUnitPrice { get; set; }
        public decimal PatentUnitPrice { get; set; }
        public long CurrencyID { get; set; }
        public CurrencyModel Currency { get; set; }
        public LanguageModel SourceLanguage { get; set; }
        public LanguageModel TargetLanguage { get; set; }
        public UnitModel Unit { get; set; }
        public EstimationTypeModel EstimationType { get; set; } 
    }
}

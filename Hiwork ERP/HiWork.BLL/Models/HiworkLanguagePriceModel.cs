using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{ 
  public class HiworkLanguagePriceModel : BaseDomainModel<HiworkLanguagePriceModel>
    {
        public Guid ID { get; set; }
        public Guid SourceLanguageID { get; set; }
        public Guid TargetLanguageID { get; set; }
        public decimal GeneralPrice { get; set; }
        public decimal? SpecialPrice { get; set; }
        public decimal? PatentPrice { get; set; }
        public long CurrencyID { get; set; }
        public CurrencyModel Currency { get; set; }
        public LanguageModel SourceLanguage { get; set; }
        public LanguageModel TargetLanguage { get; set; }
    }
}

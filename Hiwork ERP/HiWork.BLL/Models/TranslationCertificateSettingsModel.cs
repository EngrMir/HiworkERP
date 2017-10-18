

using System;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    
    public class TranslationCertificateSettingsModel : BaseDomainModel<TranslationCertificateSettingsModel>
    {
        public long ID { get; set; }
        public CertificateType CertificateType { get; set; }
        public decimal UnitPrice { get; set; }
    }
    
}

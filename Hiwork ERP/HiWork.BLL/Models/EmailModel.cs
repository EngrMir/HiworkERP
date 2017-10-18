using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class EmailModel
    {
        public long UserID { get; set; }
        public string Name { get; set; }
        public string EmailTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string EmailCc { get; set; }
        public string EmailBcc { get; set; }

    }

    public class EmailDeliverySettingsModel : BaseDomainModel<EmailDeliverySettingsModel>
    {
     public Guid ID { get; set; }
     public Guid? CustomerID { get; set; }
     public Guid? StaffID { get; set; }
     public string Email { get; set; }
     public string MobileNumber { get; set; }
    public bool IsBusinessHour { get; set; }
    public bool IsInterval { get; set; }
    public bool IsAnyTime { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public bool IsArrangedNotice { get; set; }
    public bool IsDeliveryNotice { get; set; }
    public bool IsTranslatorRequest { get; set; }
    public bool IsMessageNotice { get; set; }
    public bool IsTranslatiorConfirmed { get; set; }
    public bool IsPaymentNotification { get; set; }
    }
}

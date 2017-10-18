
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class MessageModel : BaseDomainModel<MessageModel>
    {
        public long ID { get; set; }
        public Guid SenderID { get; set; }
        public string SenderName { get; set; }
        public Guid ReceiverID { get; set; }
        public string ReceiverName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Status { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string AttachedFile { get; set; }
        public long AttachedSize { get; set; }
        public string DownloadURL { get; set; }
        public Guid OrderID { get; set; }


    }
}


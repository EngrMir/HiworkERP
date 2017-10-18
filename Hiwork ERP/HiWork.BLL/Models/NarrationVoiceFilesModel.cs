using HiWork.Utils.Infrastructure;
using System;

namespace HiWork.BLL.Models
{
    public class NarrationVoiceFilesModel : BaseDomainModel<NarrationVoiceFilesModel>
    {
        public Guid ID { get; set; }
        public Guid StaffID { get; set; }
        public  Guid  NarrationTypeID { get; set; }
        public string OrderNo { get; set; }
        public string VoiceDetail_en { get; set; }
        public string VoiceDetail_jp { get; set; }
        public string VoiceDetail_kr { get; set; }
        public string VoiceDetail_cn { get; set; }
        public string VoiceDetail_fr { get; set; }
        public string VoiceDetail_tl { get; set; }
        public string NarrationFileName { get; set; }
        public string UploadURL { get; set; }

    }
}
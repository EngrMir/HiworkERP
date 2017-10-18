

using System;

namespace HiWork.BLL.Models
{
    public class EstimationFileModel
    {
        public Guid ID { get; set; }
        public Guid EstimateID { get; set; }
        public Guid EstimateDetailsID { get; set; }
        public string FileName { get; set; }
        public string DownloadURL { get; set; }
        public string TranslationText { get; set; }
        public int WordCount { get; set; }
    }
}

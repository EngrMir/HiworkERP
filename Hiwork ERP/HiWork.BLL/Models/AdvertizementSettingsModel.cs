using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class AdvertizementSettingsModel : BaseDomainModel<AdvertizementSettingsModel>
    {
       public Guid ID { get; set; }
       public string Title { get; set; } 
       public string Title_en { get; set; }
       public string Title_jp { get; set; }
       public string Title_kr { get; set; }
       public string Title_fr { get; set; }
       public string Title_cn { get; set; }
       public string Title_tl { get; set; }
       public string Description { get; set;}
       public DateTime? ValidDateTime { get; set; }
       public byte[] BackgroundImage { get; set; }
       public string BackgroundColor { get; set; }
       public string BackgroundImageFile { get; set; }
    }
}

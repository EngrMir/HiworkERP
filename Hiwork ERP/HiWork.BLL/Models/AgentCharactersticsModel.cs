using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
      public class AgentCharactersticsModel : BaseDomainModel<AgentCharactersticsModel>
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}

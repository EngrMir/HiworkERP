//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HiWork.DAL.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class EstimationDeliveryFileType
    {
        public System.Guid ID { get; set; }
        public System.Guid EstimationID { get; set; }
        public string FileType { get; set; }
        public string Version { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Estimation Estimation { get; set; }
    }
}
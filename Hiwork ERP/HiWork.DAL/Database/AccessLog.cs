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
    
    public partial class AccessLog
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string PersonId { get; set; }
        public string LoginDeviceName { get; set; }
        public string LoginIP { get; set; }
        public System.DateTime LoginTime { get; set; }
        public Nullable<System.DateTime> LogoutTime { get; set; }
        public Nullable<byte> PasswordAttemptCount { get; set; }
    }
}
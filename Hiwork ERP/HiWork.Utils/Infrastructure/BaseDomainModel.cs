using System;
using HiWork.Utils.Infrastructure.Contract;


namespace HiWork.Utils.Infrastructure
{
    [Serializable]
    public abstract class BaseDomainModel<T> : IBaseDomainModel where T : BaseDomainModel<T>
    {
        //public long Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        //public DateTime SetOn { get; set; }
        //public long SetBy { get; set; }
        //public string SetFrom { get; set; }
        //public string Note { get; set; }
        public long CurrentUserID { get; set; } 
        public string CurrentCulture { get; set; }
        public long ApplicationId { get; set; }
        public Guid? SessionId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Role { get; set; }
        public string Name_en { get; set; }
        public string Name_jp { get; set; }
        public string Name_kr { get; set; }
        public string Name_cn { get; set; }
        public string Name_fr { get; set; }
        public string Name_tl { get; set; }
        public string Description_en { get; set; }
        public string Description_jp { get; set; }
        public string Description_kr { get; set; }
        public string Description_cn { get; set; }
        public string Description_fr { get; set; }
        public string Description_tl { get; set; }
        public int SortBy { get; set; }
        //public string Note_en { get; set; }
        //public string Note_jp { get; set; }
        //public string Note_kr { get; set; }
        //public string Note_cn { get; set; }
        //public string CultureCode { get; set; }
        public bool ParentCheckValue { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
 
        public Nullable<DateTime> UpdatedDate { get; set; }

        public long? UpdatedBy { get; set; }
        public BaseDomainModel()
        {
            InitializeModel();
        }

        public void SetCreateProperties(long userId)
        {
            //SetOn = DateTime.UtcNow;
            //SetBy = userId;
        }

        public void SetUpdateProperties(long userId)
        {
            //ModifiedOn = DateTime.UtcNow;
            //ModifiedBy = userId;
        }


        private void InitializeModel()
        {
            IsActive = false;
            IsDeleted = false;
            //SetOn = DateTime.UtcNow;
            //SetBy = 0;
        }
    }

    //public class filter
    //{
    //    public string field { get; set; }
    //    public string Operator { get; set; }
    //    public string value { get; set; }
    //    public string logic { get; set; }
    //}


    public class BaseViewModel : BaseDomainModel<BaseViewModel>
    {
        public Guid ID { get; set; }
    }
}
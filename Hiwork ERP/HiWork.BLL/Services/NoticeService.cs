using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{

    public partial interface INoticeService : IBaseService<NoticeModel, Notice>
    {
        NoticeModel SaveNotice(NoticeModel aNoticeModel);
        List<NoticeModel> GetNoticelist(BaseViewModel aNoticeModel);
        List<NoticeModel> DeleteNotice(NoticeModel aNoticeModel);


    }
    public class NoticeService : BaseService<NoticeModel, Notice>, INoticeService
    {
        private readonly INoticeRepository _noticeRepository;
        public NoticeService(INoticeRepository noticeRepository) : base(noticeRepository)
        {
            _noticeRepository = noticeRepository;
        }
        public NoticeModel SaveNotice(NoticeModel aNoticeModel)
        {
            Utility.SetDynamicPropertyValue(aNoticeModel, aNoticeModel.CurrentCulture);
            var notice = Mapper.Map<NoticeModel, Notice>(aNoticeModel);

            if (aNoticeModel.ID == Guid.Empty)
            {
                notice.CreatedDate = DateTime.Now;
                notice.ID = Guid.NewGuid();
                _noticeRepository.InsertNotice(notice);
            }
            else
            {
                notice.UpdatedBy = aNoticeModel.CurrentUserID;
                notice.UpdatedDate = DateTime.Now;
                _noticeRepository.UpdateNotice(notice);
            }
            return aNoticeModel;
        }
        public List<NoticeModel> GetNoticelist(BaseViewModel model)
        {
            List<NoticeModel> noticeList = new List<NoticeModel>();
            NoticeModel noticeModel = new NoticeModel();

            try
            {

                List<Notice> repoNotice= _noticeRepository.GetNoticelist();
                if (repoNotice != null)
                {
                    foreach (Notice notices in repoNotice)
                    {
                        //if (notices.IsDeleted == true)
                        //    continue;

                        noticeModel = Mapper.Map<Notice, NoticeModel>(notices);

                        noticeModel.Title = Utility.GetPropertyValue(noticeModel, "Title", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(noticeModel, "Title", model.CurrentCulture).ToString();
                        noticeModel.Description = Utility.GetPropertyValue(noticeModel, "Description", model.CurrentCulture) == null ? string.Empty :
                                                             Utility.GetPropertyValue(noticeModel, "Description", model.CurrentCulture).ToString();

                        noticeModel.ClientDisplayStatusName = Utility.getItemCultureList(Utility.NoticeStatusList, model).Where(s => s.Id == notices.ClientDisplayStatus).FirstOrDefault().Name;
                        noticeModel.StaffDisplayStatusName= Utility.getItemCultureList(Utility.NoticeStatusList, model).Where(s => s.Id == notices.StaffDisplayStatus).FirstOrDefault().Name;
                        noticeModel.PartnerDisplayStatusName= Utility.getItemCultureList(Utility.NoticeStatusList, model).Where(s => s.Id == notices.PartnerDisplayStatus).FirstOrDefault().Name;
                        noticeModel.PriorityName = Utility.getItemCultureList(Utility.NoticePriorityList, model).Where(s => s.Id == notices.Priority).FirstOrDefault().Name;
                        noticeModel.CurrentUserID = model.CurrentUserID;
                        noticeModel.CurrentCulture = model.CurrentCulture;
                        noticeList.Add(noticeModel);
                    }
                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Notice", message);
                throw new Exception(ex.Message);
            }

            return noticeList;
        }
      public  List<NoticeModel> DeleteNotice(NoticeModel aNoticeModel)
        {
            try
            {
                aNoticeModel.IsDeleted = true;
                this.SaveNotice(aNoticeModel);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aNoticeModel.CurrentUserID, "", message);
                throw new Exception(ex.Message);
            }

            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = aNoticeModel.CurrentCulture;
            model.CurrentUserID = aNoticeModel.CurrentUserID;
            return this.GetNoticelist(model);
        }

    }


}



    


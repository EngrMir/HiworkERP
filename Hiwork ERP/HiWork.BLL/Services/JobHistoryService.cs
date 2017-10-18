using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{
  public partial interface IJobHistoryService : IBaseService<JobHistoryModel, Staff_JobHistory>
    {
        List<JobHistoryModel> SaveJobHistory(JobHistoryModel model);
        List<JobHistoryModel> GetAllJobHistoryList(BaseViewModel model);
        List<JobHistoryModel> DeleteJobHistory(JobHistoryModel model);
    }
    public class JobHistoryService : BaseService<JobHistoryModel, Staff_JobHistory>, IJobHistoryService
    {
        private readonly IJobHistoryRepository _JobHistoryRepository;
        public JobHistoryService(IJobHistoryRepository JobHistoryRepository) : base(JobHistoryRepository)
        {
            _JobHistoryRepository = JobHistoryRepository;
        }
        public List<JobHistoryModel> SaveJobHistory(JobHistoryModel aJobHistoryModel)
        {
            List<JobHistoryModel> JobHistorys = null;
            try
            {
                Utility.SetDynamicPropertyValue(aJobHistoryModel, aJobHistoryModel.CurrentCulture);
                var jobHistory = Mapper.Map<JobHistoryModel, Staff_JobHistory>(aJobHistoryModel);

                if (aJobHistoryModel.ID == Guid.Empty)
                {
                    jobHistory.ID = Guid.NewGuid();
                    //jobHistory.StaffID = aJobHistoryModel.StaffID;
                    //jobHistory.CompanyName_en = aJobHistoryModel.CompanyName_en;
                    //jobHistory.CompanyName_jp = aJobHistoryModel.CompanyName_jp;
                    //jobHistory.CompanyName_kr = aJobHistoryModel.CompanyName_kr;
                    //jobHistory.CompanyName_cn = aJobHistoryModel.CompanyName_cn;                    
                    //jobHistory.CompanyName_tl = aJobHistoryModel.CompanyName_tl;
                    //jobHistory.CompanyName_fr = aJobHistoryModel.CompanyName_fr;

                    //jobHistory.CompanyPrivacyName_en = aJobHistoryModel.CompanyPrivacyName_en;
                    //jobHistory.CompanyPrivacyName_jp = aJobHistoryModel.CompanyPrivacyName_jp;
                    //jobHistory.CompanyPrivacyName_kr = aJobHistoryModel.CompanyPrivacyName_kr;
                    //jobHistory.CompanyPrivacyName_cn = aJobHistoryModel.CompanyPrivacyName_cn;
                    //jobHistory.CompanyPrivacyName_tl = aJobHistoryModel.CompanyPrivacyName_tl;
                    //jobHistory.CompanyPrivacyName_fr = aJobHistoryModel.CompanyPrivacyName_fr;

                    //jobHistory.CompanyDetails_en = aJobHistoryModel.CompanyDetails_en;
                    //jobHistory.CompanyDetails_jp = aJobHistoryModel.CompanyDetails_jp;
                    //jobHistory.CompanyDetails_kr = aJobHistoryModel.CompanyDetails_kr;
                    //jobHistory.CompanyDetails_cn = aJobHistoryModel.CompanyDetails_cn;
                    //jobHistory.CompanyDetails_tl = aJobHistoryModel.CompanyDetails_tl;
                    //jobHistory.CompanyDetails_fr = aJobHistoryModel.CompanyDetails_fr;

                    //jobHistory.OfficeLocation_en = aJobHistoryModel.OfficeLocation_en;
                    //jobHistory.OfficeLocation_jp = aJobHistoryModel.OfficeLocation_jp;
                    //jobHistory.OfficeLocation_kr = aJobHistoryModel.OfficeLocation_kr;
                    //jobHistory.OfficeLocation_cn = aJobHistoryModel.OfficeLocation_cn;
                    //jobHistory.OfficeLocation_tl = aJobHistoryModel.OfficeLocation_tl;
                    //jobHistory.OfficeLocation_fr = aJobHistoryModel.OfficeLocation_fr;

                    //jobHistory.Position_en = aJobHistoryModel.Position_en;
                    //jobHistory.Position_jp = aJobHistoryModel.Position_jp;
                    //jobHistory.Position_kr = aJobHistoryModel.Position_kr;
                    //jobHistory.Position_cn = aJobHistoryModel.Position_cn;
                    //jobHistory.Position_tl = aJobHistoryModel.Position_tl;
                    //jobHistory.Position_fr = aJobHistoryModel.Position_fr;

                    //jobHistory.JobResponsibility_en = aJobHistoryModel.JobResponsibility_en;
                    //jobHistory.JobResponsibility_jp = aJobHistoryModel.JobResponsibility_jp;
                    //jobHistory.JobResponsibility_kr = aJobHistoryModel.JobResponsibility_kr;
                    //jobHistory.JobResponsibility_cn = aJobHistoryModel.JobResponsibility_cn;
                    //jobHistory.JobResponsibility_tl = aJobHistoryModel.JobResponsibility_tl;
                    //jobHistory.JobResponsibility_fr = aJobHistoryModel.JobResponsibility_fr;

                    //jobHistory.Achivements_en = aJobHistoryModel.Achivements_en;
                    //jobHistory.Achivements_jp = aJobHistoryModel.Achivements_jp;
                    //jobHistory.Achivements_kr = aJobHistoryModel.Achivements_kr;
                    //jobHistory.Achivements_cn = aJobHistoryModel.Achivements_cn;
                    //jobHistory.Achivements_tl = aJobHistoryModel.Achivements_tl;
                    //jobHistory.Achivements_fr = aJobHistoryModel.Achivements_fr;

                    //jobHistory.InterviewNote_en = aJobHistoryModel.InterviewNote_en;
                    //jobHistory.InterviewNote_jp = aJobHistoryModel.InterviewNote_jp;
                    //jobHistory.InterviewNote_kr = aJobHistoryModel.InterviewNote_kr;
                    //jobHistory.InterviewNote_cn = aJobHistoryModel.InterviewNote_cn;
                    //jobHistory.InterviewNote_tl = aJobHistoryModel.InterviewNote_tl;
                    //jobHistory.InterviewNote_fr = aJobHistoryModel.InterviewNote_fr;

                    //jobHistory.ReasonOfResignation_en = aJobHistoryModel.ReasonOfResignation_en;
                    //jobHistory.ReasonOfResignation_jp = aJobHistoryModel.ReasonOfResignation_jp;
                    //jobHistory.ReasonOfResignation_kr = aJobHistoryModel.ReasonOfResignation_kr;
                    //jobHistory.ReasonOfResignation_cn = aJobHistoryModel.ReasonOfResignation_cn;
                    //jobHistory.ReasonOfResignation_tl = aJobHistoryModel.ReasonOfResignation_tl;
                    //jobHistory.ReasonOfResignation_fr = aJobHistoryModel.ReasonOfResignation_fr;

                    //jobHistory.BusinessTypeID = aJobHistoryModel.BusinessTypeID;
                    //jobHistory.BusinessTypeItemID = aJobHistoryModel.BusinessTypeItemID;
                    //jobHistory.JobType1ID = aJobHistoryModel.JobType1ID;
                    //jobHistory.JobType1ItemID = aJobHistoryModel.JobType1ItemID;
                    //jobHistory.JobType2ID = aJobHistoryModel.JobType2ID;
                    //jobHistory.JobType2ItemID = aJobHistoryModel.JobType2ItemID;
                    //jobHistory.JobType3ID = aJobHistoryModel.JobType3ID;
                    //jobHistory.JobType3ItemID = aJobHistoryModel.JobType3ItemID;
                    //jobHistory.SalaryAmount = aJobHistoryModel.SalaryAmount;


                    _JobHistoryRepository.InsertJobHistory(jobHistory);
                }
                else
                {
                    jobHistory.ID = Guid.NewGuid();
                    jobHistory.StaffID = aJobHistoryModel.StaffID;
                    jobHistory.CompanyName_en = aJobHistoryModel.CompanyName_en;
                    //jobHistory.CompanyName_jp = aJobHistoryModel.CompanyName_jp;
                    //jobHistory.CompanyName_kr = aJobHistoryModel.CompanyName_kr;
                    //jobHistory.CompanyName_cn = aJobHistoryModel.CompanyName_cn;
                    //jobHistory.CompanyName_tl = aJobHistoryModel.CompanyName_tl;
                    //jobHistory.CompanyName_fr = aJobHistoryModel.CompanyName_fr;

                    //jobHistory.CompanyPrivacyName_en = aJobHistoryModel.CompanyPrivacyName_en;
                    //jobHistory.CompanyPrivacyName_jp = aJobHistoryModel.CompanyPrivacyName_jp;
                    //jobHistory.CompanyPrivacyName_kr = aJobHistoryModel.CompanyPrivacyName_kr;
                    //jobHistory.CompanyPrivacyName_cn = aJobHistoryModel.CompanyPrivacyName_cn;
                    //jobHistory.CompanyPrivacyName_tl = aJobHistoryModel.CompanyPrivacyName_tl;
                    //jobHistory.CompanyPrivacyName_fr = aJobHistoryModel.CompanyPrivacyName_fr;

                    //jobHistory.CompanyDetails_en = aJobHistoryModel.CompanyDetails_en;
                    //jobHistory.CompanyDetails_jp = aJobHistoryModel.CompanyDetails_jp;
                    //jobHistory.CompanyDetails_kr = aJobHistoryModel.CompanyDetails_kr;
                    //jobHistory.CompanyDetails_cn = aJobHistoryModel.CompanyDetails_cn;
                    //jobHistory.CompanyDetails_tl = aJobHistoryModel.CompanyDetails_tl;
                    //jobHistory.CompanyDetails_fr = aJobHistoryModel.CompanyDetails_fr;

                    //jobHistory.OfficeLocation_en = aJobHistoryModel.OfficeLocation_en;
                    //jobHistory.OfficeLocation_jp = aJobHistoryModel.OfficeLocation_jp;
                    //jobHistory.OfficeLocation_kr = aJobHistoryModel.OfficeLocation_kr;
                    //jobHistory.OfficeLocation_cn = aJobHistoryModel.OfficeLocation_cn;
                    //jobHistory.OfficeLocation_tl = aJobHistoryModel.OfficeLocation_tl;
                    //jobHistory.OfficeLocation_fr = aJobHistoryModel.OfficeLocation_fr;

                    //jobHistory.Position_en = aJobHistoryModel.Position_en;
                    //jobHistory.Position_jp = aJobHistoryModel.Position_jp;
                    //jobHistory.Position_kr = aJobHistoryModel.Position_kr;
                    //jobHistory.Position_cn = aJobHistoryModel.Position_cn;
                    //jobHistory.Position_tl = aJobHistoryModel.Position_tl;
                    //jobHistory.Position_fr = aJobHistoryModel.Position_fr;

                    //jobHistory.JobResponsibility_en = aJobHistoryModel.JobResponsibility_en;
                    //jobHistory.JobResponsibility_jp = aJobHistoryModel.JobResponsibility_jp;
                    //jobHistory.JobResponsibility_kr = aJobHistoryModel.JobResponsibility_kr;
                    //jobHistory.JobResponsibility_cn = aJobHistoryModel.JobResponsibility_cn;
                    //jobHistory.JobResponsibility_tl = aJobHistoryModel.JobResponsibility_tl;
                    //jobHistory.JobResponsibility_fr = aJobHistoryModel.JobResponsibility_fr;

                    //jobHistory.Achivements_en = aJobHistoryModel.Achivements_en;
                    //jobHistory.Achivements_jp = aJobHistoryModel.Achivements_jp;
                    //jobHistory.Achivements_kr = aJobHistoryModel.Achivements_kr;
                    //jobHistory.Achivements_cn = aJobHistoryModel.Achivements_cn;
                    //jobHistory.Achivements_tl = aJobHistoryModel.Achivements_tl;
                    //jobHistory.Achivements_fr = aJobHistoryModel.Achivements_fr;

                    //jobHistory.InterviewNote_en = aJobHistoryModel.InterviewNote_en;
                    //jobHistory.InterviewNote_jp = aJobHistoryModel.InterviewNote_jp;
                    //jobHistory.InterviewNote_kr = aJobHistoryModel.InterviewNote_kr;
                    //jobHistory.InterviewNote_cn = aJobHistoryModel.InterviewNote_cn;
                    //jobHistory.InterviewNote_tl = aJobHistoryModel.InterviewNote_tl;
                    //jobHistory.InterviewNote_fr = aJobHistoryModel.InterviewNote_fr;

                    //jobHistory.ReasonOfResignation_en = aJobHistoryModel.ReasonOfResignation_en;
                    //jobHistory.ReasonOfResignation_jp = aJobHistoryModel.ReasonOfResignation_jp;
                    //jobHistory.ReasonOfResignation_kr = aJobHistoryModel.ReasonOfResignation_kr;
                    //jobHistory.ReasonOfResignation_cn = aJobHistoryModel.ReasonOfResignation_cn;
                    //jobHistory.ReasonOfResignation_tl = aJobHistoryModel.ReasonOfResignation_tl;
                    //jobHistory.ReasonOfResignation_fr = aJobHistoryModel.ReasonOfResignation_fr;

                    //jobHistory.BusinessTypeID = aJobHistoryModel.BusinessTypeID;
                    //jobHistory.BusinessTypeItemID = aJobHistoryModel.BusinessTypeItemID;
                    //jobHistory.JobType1ID = aJobHistoryModel.JobType1ID;
                    //jobHistory.JobType1ItemID = aJobHistoryModel.JobType1ItemID;
                    //jobHistory.JobType2ID = aJobHistoryModel.JobType2ID;
                    //jobHistory.JobType2ItemID = aJobHistoryModel.JobType2ItemID;
                    //jobHistory.JobType3ID = aJobHistoryModel.JobType3ID;
                    //jobHistory.JobType3ItemID = aJobHistoryModel.JobType3ItemID;
                    //jobHistory.SalaryAmount = aJobHistoryModel.SalaryAmount;

                    _JobHistoryRepository.UpdateJobHistory(jobHistory);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aJobHistoryModel.CurrentCulture;
                baseViewModel.CurrentUserID = aJobHistoryModel.CurrentUserID;
                JobHistorys = GetAllJobHistoryList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aJobHistoryModel.CurrentUserID, "JobHistory", message);
                throw new Exception(ex.Message);
            }
            return JobHistorys;
        }
        public List<JobHistoryModel> GetAllJobHistoryList(BaseViewModel model)
        {
            List<JobHistoryModel> JobHistoryModelList = new List<JobHistoryModel>();
            JobHistoryModel JobHistoryModel = new JobHistoryModel();
            try
            {
                List<Staff_JobHistory> JobHistoryList = _JobHistoryRepository.GetAllJobHistoryList();
                if (JobHistoryList != null)
                {
                    JobHistoryList.ForEach(a =>
                    {
                        JobHistoryModel = Mapper.Map<Staff_JobHistory, JobHistoryModel>(a);
                        //JobHistoryModel.businessCategoryModel = Mapper.Map<Master_StaffBusinessCategory, BusinessCategoryModel>(a.Master_StaffBusinessCategory);
                        //JobHistoryModel.businessCategoryDetailsModel = Mapper.Map<Master_StaffBusinessCategoryDetails, BusinessCategoryDetailsModel>(a.Master_StaffBusinessCategoryDetails);

                        //JobHistoryModel.jobCategoryModel = Mapper.Map<Master_StaffJobCategory, JobCategoryModel>(a.Master_StaffJobCategory);
                        //JobHistoryModel.jobCategoryModel1 = Mapper.Map<Master_StaffJobCategory, JobCategoryModel>(a.Master_StaffJobCategory);
                        //JobHistoryModel.jobCategoryModel2 = Mapper.Map<Master_StaffJobCategory, JobCategoryModel>(a.Master_StaffJobCategory);

                        //JobHistoryModel.jobCategoryDetailsModel = Mapper.Map<Master_StaffJobCategoryDetails, JobCategoryDetailsModel>(a.Master_StaffJobCategoryDetails);
                        //JobHistoryModel.jobCategoryDetailsModel1 = Mapper.Map<Master_StaffJobCategoryDetails, JobCategoryDetailsModel>(a.Master_StaffJobCategoryDetails);
                        //JobHistoryModel.jobCategoryDetailsModel2 = Mapper.Map<Master_StaffJobCategoryDetails, JobCategoryDetailsModel>(a.Master_StaffJobCategoryDetails);
                        //JobHistoryModel.staffModel = Mapper.Map<Staff, StaffModel>(a.Staff);


                        
                        //JobHistoryModel.businessCategoryDetailsModel = Mapper.Map<Master_StaffBusinessCategoryDetails, BusinessCategoryDetailsModel>(a.Master_StaffBusinessCategoryDetails);


                        //if (JobHistoryModel.businessCategoryModel != null)
                        //    JobHistoryModel.businessCategoryModel.Name = Utility.GetPropertyValue(JobHistoryModel.businessCategoryModel, "Name", model.CurrentCulture) == null ? string.Empty :
                        //                                      Utility.GetPropertyValue(JobHistoryModel.businessCategoryModel, "Name", model.CurrentCulture).ToString();
                        //if (JobHistoryModel.businessCategoryDetailsModel != null)
                        //    JobHistoryModel.businessCategoryDetailsModel.BusinessCategory.Name = Utility.GetPropertyValue(JobHistoryModel.businessCategoryDetailsModel, "Name", model.CurrentCulture) == null ? string.Empty :
                        //                                      Utility.GetPropertyValue(JobHistoryModel.businessCategoryDetailsModel, "Name", model.CurrentCulture).ToString();

                
                        JobHistoryModel.CurrentUserID = model.CurrentUserID;

                        JobHistoryModel.CurrentCulture = model.CurrentCulture;
                        JobHistoryModelList.Add(JobHistoryModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "JobHistory", message);
                throw new Exception(ex.Message);
            }

            JobHistoryModelList.Sort(CompareJobHistoryByName);
            return JobHistoryModelList;
        }
        public List<JobHistoryModel> DeleteJobHistory(JobHistoryModel model)
        {
            List<JobHistoryModel> JobHistorys = null;
            try
            {
                _JobHistoryRepository.DeleteJobHistory(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                JobHistorys = GetAllJobHistoryList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "JobHistory", message);
                throw new Exception(ex.Message);
            }
            return JobHistorys;
        }

        private int CompareJobHistoryByName(JobHistoryModel arg1, JobHistoryModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.CreatedBy.ToString(), arg2.CreatedDate.ToString(), true);
            return cmpresult;
        }
    }
}

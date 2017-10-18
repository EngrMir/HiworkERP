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
    public partial interface IJobCategoryDetailsService : IBaseService<JobCategoryDetailsModel, Master_StaffJobCategoryDetails>
    {
        List<JobCategoryDetailsModel> SaveJobCategoryDetails(JobCategoryDetailsModel model);
        List<JobCategoryDetailsModel> GetAllJobCategoryDetailsList(JobCategoryDetailsModel model);
        List<JobCategoryDetailsModel> DeleteJobCategoryDetails(JobCategoryDetailsModel model);
    }

    public class JobCategoryDetailsService : BaseService<JobCategoryDetailsModel, Master_StaffJobCategoryDetails>, IJobCategoryDetailsService
    {


        private readonly IJobCategoryDetailsRepository _JobCategoryDetailsRepository;
        public JobCategoryDetailsService(IJobCategoryDetailsRepository JobCategoryDetailsRepository)
            : base(JobCategoryDetailsRepository)
        {
            _JobCategoryDetailsRepository = JobCategoryDetailsRepository;
        }
        public List<JobCategoryDetailsModel> SaveJobCategoryDetails(JobCategoryDetailsModel model)
        {
            List<JobCategoryDetailsModel> JobCategoryDetailss = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var JobCategoryDetails = Mapper.Map<JobCategoryDetailsModel, Master_StaffJobCategoryDetails>(model);
                if (JobCategoryDetails.ID != Guid.Empty)
                {
                    JobCategoryDetails.UpdatedBy = model.CurrentUserID;
                    JobCategoryDetails.UpdatedDate = DateTime.Now;
                    _JobCategoryDetailsRepository.UpdateJobCategoryDetails(JobCategoryDetails);
                }
                else
                {
                    JobCategoryDetails.ID = Guid.NewGuid();
                    JobCategoryDetails.CreatedBy = model.CurrentUserID;
                    JobCategoryDetails.CreatedDate = DateTime.Now;
                    _JobCategoryDetailsRepository.InsertJobCategoryDetails(JobCategoryDetails);
                }
                JobCategoryDetailss = GetAllJobCategoryDetailsList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "JobCategoryDetails", message);
                throw new Exception(ex.Message);
            }
            return JobCategoryDetailss;
        }
        public List<JobCategoryDetailsModel> GetAllJobCategoryDetailsList(JobCategoryDetailsModel model)
        {
            List<JobCategoryDetailsModel> JobCategoryDetailsList = new List<JobCategoryDetailsModel>();
            JobCategoryDetailsModel JobCategoryDetailsModel = new JobCategoryDetailsModel();
            try
            {
                List<Master_StaffJobCategoryDetails> JobCategoryDetailsvList = _JobCategoryDetailsRepository.GetAllJobCategoryDetailsList();
                if (JobCategoryDetailsvList != null)
                {
                    JobCategoryDetailsvList.ForEach(a =>
                    {
                        JobCategoryDetailsModel = Mapper.Map<Master_StaffJobCategoryDetails, JobCategoryDetailsModel>(a);

                        JobCategoryDetailsModel.JobCategory = Mapper.Map<Master_StaffJobCategory, JobCategoryModel>(a.Master_StaffJobCategory);
                        if (JobCategoryDetailsModel.JobCategory != null)
                            JobCategoryDetailsModel.JobCategory.Name = Utility.GetPropertyValue(JobCategoryDetailsModel.JobCategory, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(JobCategoryDetailsModel.JobCategory, "Name", model.CurrentCulture).ToString();

                        JobCategoryDetailsModel.Description = Utility.GetPropertyValue(JobCategoryDetailsModel, "Description", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(JobCategoryDetailsModel, "Description", model.CurrentCulture).ToString();
                        JobCategoryDetailsModel.CurrentUserID = model.CurrentUserID;
                        JobCategoryDetailsModel.CurrentCulture = model.CurrentCulture;
                        JobCategoryDetailsList.Add(JobCategoryDetailsModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "JobCategoryDetails", message);
                throw new Exception(ex.Message);
            }
            return JobCategoryDetailsList;
        }

        public List<JobCategoryDetailsModel> DeleteJobCategoryDetails(JobCategoryDetailsModel model)
        {
            List<JobCategoryDetailsModel> JobCategoryDetails = null;
            try
            {
                _JobCategoryDetailsRepository.DeleteJobCategoryDetails(model.ID);
                JobCategoryDetails = GetAllJobCategoryDetailsList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "JobCategoryDetails", message);
                throw new Exception(ex.Message);
            }
            return JobCategoryDetails;
        }
    }
}

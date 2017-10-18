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
    //public partial interface IJobTypeService : IBaseService<JobTypeModel, Master_StaffJobType>
    //{
    //    List<JobTypeModel> SaveJobType(JobTypeModel model);
    //    List<JobTypeModel> GetAllJobTypeList(JobTypeModel model);
    //    List<JobTypeModel> DeleteJobType(JobTypeModel model);
    //}

    //public class JobTypeService : BaseService<JobTypeModel, Master_StaffJobType>, IJobTypeService
    //{
    //    private readonly IJobTypeRepository _jobTypeRepository;
    //    public JobTypeService(IJobTypeRepository jobTypeRepository)
    //        : base(jobTypeRepository)
    //    {
    //        _jobTypeRepository = jobTypeRepository;
    //    }
    //    public List<JobTypeModel> SaveJobType(JobTypeModel model)
    //    {
    //        List<JobTypeModel> JobTypes = null;
    //        try
    //        {
    //            Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

    //        var JobType = Mapper.Map<JobTypeModel, Master_StaffJobType>(model);
    //        if (JobType.ID != Guid.Empty)
    //        {
    //            JobType.UpdatedBy = model.CurrentUserID;
    //            JobType.UpdatedDate = DateTime.Now;
    //                _jobTypeRepository.UpdateJobType(JobType);
    //        }
    //        else
    //        {
    //                JobType.ID = Guid.NewGuid();
    //                JobType.CreatedBy = model.CurrentUserID;
    //            JobType.CreatedDate = DateTime.Now;
    //                _jobTypeRepository.InsertJobType(JobType);
    //        }
    //        JobTypes = GetAllJobTypeList(model);
    //        }
    //        catch (Exception ex)
    //        {
    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(model.CurrentUserID, "JobType", message);
    //            throw new Exception(ex.Message);
    //        }
    //        return JobTypes;
    //    }
    //    public List<JobTypeModel> GetAllJobTypeList(JobTypeModel model)
    //    {
    //        List<JobTypeModel> JobTypeList = new List<JobTypeModel>();
    //        JobTypeModel JobTypeModel = new JobTypeModel();
    //        try
    //        {
    //            List<Master_StaffJobType> JobTypevList = _jobTypeRepository.GetAllJobTypeList();
    //        if (JobTypevList != null)
    //        {
    //            JobTypevList.ForEach(a =>
    //            {
    //                JobTypeModel = Mapper.Map<Master_StaffJobType, JobTypeModel>(a);
    //                JobTypeModel.Name = Utility.GetPropertyValue(JobTypeModel, "Name", model.CurrentCulture) == null ? string.Empty :
    //                    Utility.GetPropertyValue(JobTypeModel, "Name", model.CurrentCulture).ToString();
    //                JobTypeModel.Description = Utility.GetPropertyValue(JobTypeModel, "Description", model.CurrentCulture) == null ? string.Empty :
    //                    Utility.GetPropertyValue(JobTypeModel, "Description", model.CurrentCulture).ToString();
    //                JobTypeModel.CurrentUserID = model.CurrentUserID;
    //                JobTypeModel.CurrentCulture = model.CurrentCulture;
    //                JobTypeList.Add(JobTypeModel);
    //            });
    //        }
    //        }
    //        catch (Exception ex)
    //        {
    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(model.CurrentUserID, "JobType", message);
    //            throw new Exception(ex.Message);
    //        }
    //        return JobTypeList;
    //    }
    //    public List<JobTypeModel> DeleteJobType(JobTypeModel model)
    //    {
    //        List<JobTypeModel> jobTypes = null;
    //        try
    //        {
    //            _jobTypeRepository.DeleteJobType(model.ID);
    //            jobTypes = GetAllJobTypeList(model);
    //        }
    //        catch (Exception ex)
    //        {
    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(model.CurrentUserID, "JobType", message);
    //            throw new Exception(ex.Message);
    //        }
    //        return jobTypes;
    //    }
    //}
}

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
using System.Data.SqlClient;

namespace HiWork.BLL.Services
{
    public partial interface IJobCategoryService : IBaseService<JobCategoryModel, Master_StaffJobCategory>
    {
        List<JobCategoryModel> SaveJobCategory(JobCategoryModel model);
        List<JobCategoryModel> GetAllJobCategoryList(JobCategoryModel model);
        List<JobCategoryModel> DeleteJobCategory(JobCategoryModel aJobCategoryModel);
    }
    public class JobCategoryService : BaseService<JobCategoryModel, Master_StaffJobCategory>, IJobCategoryService 
    {
        private readonly IJobCategoryRepository _jobCategoryRepository;
        private readonly ISqlConnectionService _sqlConnService;
        public JobCategoryService(IJobCategoryRepository jobCategoryRepository)
            : base(jobCategoryRepository)
        {
            _jobCategoryRepository = jobCategoryRepository;
            _sqlConnService = new SqlConnectionService();
        }

        public List<JobCategoryModel> DeleteJobCategory(JobCategoryModel model)
        {
            List<JobCategoryModel> jobCategories = null;
            try
            {
                _jobCategoryRepository.DeleteJobCategory(model.ID);
                jobCategories = GetAllJobCategoryList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "JobCategory", message);
                throw new Exception(ex.Message);
            }
            return jobCategories;
        }

        public List<JobCategoryModel> GetAllJobCategoryList(JobCategoryModel model)
        {
            List<JobCategoryModel> jobCategories = new List<JobCategoryModel>();
            JobCategoryModel JobCategoryModel;
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetAllStaffJobCategory", _sqlConnService.CreateConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {                  
                    JobCategoryModel = new JobCategoryModel();
                    JobCategoryModel.ID = Guid.Parse(reader["ID"].ToString());
                    JobCategoryModel.Name = reader["Name"].ToString();
                    JobCategoryModel.SortBy = Convert.ToInt32(reader["SortBy"].ToString());
                    JobCategoryModel.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                    JobCategoryModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    JobCategoryModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    JobCategoryModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    JobCategoryModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    jobCategories.Add(JobCategoryModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "JobCategory", message);
                throw new Exception(ex.Message);
            }
            return jobCategories;
        }

        public List<JobCategoryModel> SaveJobCategory(JobCategoryModel model)
        {
            List<JobCategoryModel> jobCategories = null;
            try
            {

                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var JobCategory = Mapper.Map<JobCategoryModel, Master_StaffJobCategory>(model);
                if (JobCategory.ID != Guid.Empty)
                {
                    JobCategory.UpdatedBy = model.CurrentUserID;
                    JobCategory.UpdatedDate = DateTime.Now;
                    _jobCategoryRepository.UpdateJobCategory(JobCategory);
                }
                else
                {
                    JobCategory.ID = Guid.NewGuid();
                    JobCategory.CreatedBy = model.CurrentUserID;
                    JobCategory.CreatedDate = DateTime.Now;
                    _jobCategoryRepository.InsertJobCategory(JobCategory);
                }
                jobCategories = GetAllJobCategoryList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "JobCategory", message);
                throw new Exception(ex.Message);
            }
            return jobCategories;
        }
    }
}

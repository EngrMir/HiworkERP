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
    public partial interface IEducationService : IBaseService<EducationModel, Master_StaffEducationalDegree>
    {
        List<EducationModel> SaveEducation(EducationModel aEducationModel);
        List<EducationModel> GetAllEducationList(EducationModel aEducationModel);
        List<EducationModel> DeleteEducation(EducationModel aEducationModel);
    }
    public class EducationService : BaseService<EducationModel, Master_StaffEducationalDegree>, IEducationService
    {
        private readonly IEducationRepository _educationRepository;
        public EducationService(IEducationRepository EducationRepository)
            : base(EducationRepository)
        {
            _educationRepository = EducationRepository;
        }

        public List<EducationModel> SaveEducation(EducationModel aEducationModel)
        {
            List<EducationModel> Educations = null;
            try
            {

                Utility.SetDynamicPropertyValue(aEducationModel, aEducationModel.CurrentCulture);

                var Education = Mapper.Map<EducationModel, Master_StaffEducationalDegree>(aEducationModel);
                if (Education.ID != Guid.Empty)
                {
                        Education.UpdatedBy = aEducationModel.CurrentUserID;
                    Education.UpdatedDate = DateTime.Now;
                    _educationRepository.UpdateUserEducation(Education);
                }
                else
                {
                    Education.ID = Guid.NewGuid();
                    Education.CreatedBy = aEducationModel.CurrentUserID;
                    Education.CreatedDate = DateTime.Now;
                    _educationRepository.InsertUserEducation(Education);
                }
                Educations = GetAllEducationList(aEducationModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aEducationModel.CurrentUserID, "Education", message);
                throw new Exception(ex.Message);
            }
            return Educations;
        }
        public List<EducationModel> GetAllEducationList(EducationModel model)
        {
            List<EducationModel> EducationList = new List<EducationModel>();
            EducationModel EducationModel = new EducationModel();
            try
            {
                List<Master_StaffEducationalDegree> rollList = _educationRepository.GetAllUserEducationList();
                if (rollList != null)
                {
                    rollList.ForEach(a =>
                    {
                        EducationModel = Mapper.Map<Master_StaffEducationalDegree, EducationModel>(a);
                        EducationModel.Name = Utility.GetPropertyValue(EducationModel, "Name", model.CurrentCulture) == null ? null :
                                                              Utility.GetPropertyValue(EducationModel, "Name", model.CurrentCulture).ToString();
                        EducationModel.Description = Utility.GetPropertyValue(EducationModel, "Description", model.CurrentCulture) == null ? null :
                                                              Utility.GetPropertyValue(EducationModel, "Description", model.CurrentCulture).ToString();
                        EducationModel.CurrentUserID = model.CurrentUserID;
                        EducationModel.CurrentCulture = model.CurrentCulture;
                        EducationList.Add(EducationModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Education", message);
                throw new Exception(ex.Message);
            }

            return EducationList;
        }
        public List<EducationModel> DeleteEducation(EducationModel aEducationModel)
        {
            List<EducationModel> Educations = null;
            try
            {
                _educationRepository.DeleteUserEducation(aEducationModel.ID);
                Educations = GetAllEducationList(aEducationModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aEducationModel.CurrentUserID, "Education", message);
                throw new Exception(ex.Message);
            }
            return Educations;
        }
    }
}

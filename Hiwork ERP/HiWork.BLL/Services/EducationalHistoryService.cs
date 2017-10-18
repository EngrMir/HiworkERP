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
    public partial interface IEducationalHistoryService : IBaseService<EducationalHistoryModel, Staff_EducationalHistory>
    {
        List<EducationalHistoryModel> SaveEducationalHistory(EducationalHistoryModel model);
        List<EducationalHistoryModel> GetAllEducationalHistoryList(EducationalHistoryModel model);
        List<EducationalHistoryModel> DeleteEducationalHistory(EducationalHistoryModel model);
    }
    public class EducationalHistoryService : BaseService<EducationalHistoryModel, Staff_EducationalHistory>, IEducationalHistoryService
    {
        private readonly IEducationalHistoryRepository _EducationalHistoryRepository;
        public EducationalHistoryService(IEducationalHistoryRepository EducationalHistoryRepository) : base(EducationalHistoryRepository)
        {
            _EducationalHistoryRepository = EducationalHistoryRepository;
        }
        public List<EducationalHistoryModel> SaveEducationalHistory(EducationalHistoryModel aEducationalHistoryModel)
        {
            List<EducationalHistoryModel> EducationalHistorys = null;
            try
            {
                Utility.SetDynamicPropertyValue(aEducationalHistoryModel, aEducationalHistoryModel.CurrentCulture);
                var EducationalHistory = Mapper.Map<EducationalHistoryModel, Staff_EducationalHistory>(aEducationalHistoryModel);

                if (aEducationalHistoryModel.Id == Guid.Empty)
                {
                    EducationalHistory.ID = Guid.NewGuid();
                    EducationalHistory.StaffID = aEducationalHistoryModel.StaffID;
                    EducationalHistory.DegreeID = aEducationalHistoryModel.DegreeID;
                    EducationalHistory.MajorSubjectID = aEducationalHistoryModel.MajorSubjectID; 
                    EducationalHistory.CountryID = aEducationalHistoryModel.CountryId;                   
                    _EducationalHistoryRepository.InsertEducationalHistory(EducationalHistory);
                }
                else
                {
                     EducationalHistory.StaffID = aEducationalHistoryModel.StaffID;
                    EducationalHistory.DegreeID = aEducationalHistoryModel.DegreeID;
                    EducationalHistory.MajorSubjectID = aEducationalHistoryModel.MajorSubjectID; 
                    EducationalHistory.CountryID = aEducationalHistoryModel.CountryId;
                    _EducationalHistoryRepository.UpdateEducationalHistory(EducationalHistory);
                }
                EducationalHistorys = GetAllEducationalHistoryList(aEducationalHistoryModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aEducationalHistoryModel.CurrentUserID, "EducationalHistory", message);
                throw new Exception(ex.Message);
            }
            return EducationalHistorys;
        }
        public List<EducationalHistoryModel> GetAllEducationalHistoryList(EducationalHistoryModel model)
        {
            List<EducationalHistoryModel> EducationalHistoryModelList = new List<EducationalHistoryModel>();
            EducationalHistoryModel EducationalHistoryModel = new EducationalHistoryModel();
            try
            {
                List<Staff_EducationalHistory> EducationalHistoryList = _EducationalHistoryRepository.GetAllEducationalHistoryList();
                if (EducationalHistoryList != null)
                {
                    EducationalHistoryList.ForEach(a =>
                    {
                        EducationalHistoryModel = Mapper.Map<Staff_EducationalHistory, EducationalHistoryModel>(a);
                        EducationalHistoryModel.Country = Mapper.Map<Master_Country, CountryModel>(a.Master_Country);
                        EducationalHistoryModel.Staff = Mapper.Map<Staff, StaffModel>(a.Staff);

                        if (EducationalHistoryModel.Country != null)
                            EducationalHistoryModel.Country.Name = Utility.GetPropertyValue(EducationalHistoryModel.Country, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(EducationalHistoryModel.Country, "Name", model.CurrentCulture).ToString();
                        //if (EducationalHistoryModel.Staff != null)
                        //    EducationalHistoryModel.Staff.NickName = Utility.GetPropertyValue(EducationalHistoryModel.Staff, "NickName", model.CurrentCulture) == null ? string.Empty :
                        //                                      Utility.GetPropertyValue(EducationalHistoryModel.Staff, "NickName", model.CurrentCulture).ToString();
                        if (EducationalHistoryModel.Education != null)
                            EducationalHistoryModel.Education.Name = Utility.GetPropertyValue(EducationalHistoryModel.Education, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(EducationalHistoryModel.Education, "Name", model.CurrentCulture).ToString();
                        if (EducationalHistoryModel.MajorSubject != null)
                            EducationalHistoryModel.MajorSubject.Name = Utility.GetPropertyValue(EducationalHistoryModel.MajorSubject, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(EducationalHistoryModel.MajorSubject, "Name", model.CurrentCulture).ToString();
                        EducationalHistoryModel.CurrentUserID = model.CurrentUserID;

                        EducationalHistoryModel.CurrentCulture = model.CurrentCulture;
                        EducationalHistoryModelList.Add(EducationalHistoryModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EducationalHistory", message);
                throw new Exception(ex.Message);
            }
            return EducationalHistoryModelList;
        }
        public List<EducationalHistoryModel> DeleteEducationalHistory(EducationalHistoryModel model)
        {
            List<EducationalHistoryModel> EducationalHistorys = null;
            try
            {
                _EducationalHistoryRepository.DeleteEducationalHistory(model.Id);
                EducationalHistorys = GetAllEducationalHistoryList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EducationalHistory", message);
                throw new Exception(ex.Message);
            }
            return EducationalHistorys;
        }
    }
}

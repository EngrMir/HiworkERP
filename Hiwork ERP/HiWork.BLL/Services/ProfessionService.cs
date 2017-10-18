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
    public partial interface IProfessionService : IBaseService<ProfessionModel, Master_StaffProfession>
    {
        List<ProfessionModel> SaveProfession(ProfessionModel model);
        List<ProfessionModel> GetAllProfessionList(ProfessionModel model);
        List<ProfessionModel> DeleteProfession(ProfessionModel model);
    }

    public class ProfessionService : BaseService<ProfessionModel, Master_StaffProfession>, IProfessionService
    {
        private readonly IProfessionRepository _professionRepository;
        public ProfessionService(IProfessionRepository professionRepository)
            : base(professionRepository)
        {
            _professionRepository = professionRepository;
        }      
        public List<ProfessionModel> SaveProfession(ProfessionModel model)
        {
            List<ProfessionModel> professionList = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

            var profession = Mapper.Map<ProfessionModel, Master_StaffProfession>(model);
            if (profession.ID != Guid.Empty)
            {
                profession.UpdatedBy = model.CurrentUserID;
                profession.UpdatedDate = DateTime.Now;
                _professionRepository.UpdateProfession(profession);
            }
            else
            {
                    profession.ID = Guid.NewGuid();
                    profession.CreatedBy = model.CurrentUserID;
                profession.CreatedDate = DateTime.Now;
                _professionRepository.InsertProfession(profession);
            }
            professionList = GetAllProfessionList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Profession", message);
                throw new Exception(ex.Message);
            }
            return professionList;
        }
        public List<ProfessionModel> GetAllProfessionList(ProfessionModel model)
        {
            List<ProfessionModel> professionList = new List<ProfessionModel>();
            ProfessionModel professionModel = new ProfessionModel();
            try
            {
                List<Master_StaffProfession> professionvList = _professionRepository.GetAllProfessionList();
            if (professionvList != null)
            {
                professionvList.ForEach(a =>
                {
                    professionModel = Mapper.Map<Master_StaffProfession, ProfessionModel>(a);
                    professionModel.Name = Utility.GetPropertyValue(professionModel, "Name", model.CurrentCulture) == null ? string.Empty :
                        Utility.GetPropertyValue(professionModel, "Name", model.CurrentCulture).ToString();
                    professionModel.Description = Utility.GetPropertyValue(professionModel, "Description", model.CurrentCulture) == null ? string.Empty :
                        Utility.GetPropertyValue(professionModel, "Description", model.CurrentCulture).ToString();
                    professionModel.CurrentUserID = model.CurrentUserID;
                    professionModel.CurrentCulture = model.CurrentCulture;
                    professionList.Add(professionModel);
                });
            }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Profession", message);
                throw new Exception(ex.Message);
            }
            return professionList;
        }
        public List<ProfessionModel> DeleteProfession(ProfessionModel model)
        {
            List<ProfessionModel> professions = null;
            try
            {
                _professionRepository.DeleteProfession(model.ID);
                professions = GetAllProfessionList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Profession", message);
                throw new Exception(ex.Message);
            }
            return professions;
        }
    }
  }

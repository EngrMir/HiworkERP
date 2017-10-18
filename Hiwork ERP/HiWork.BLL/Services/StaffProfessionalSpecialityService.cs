using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
namespace HiWork.BLL.Services
{
    public partial interface IStaffProfessionalSpecialityService : IBaseService<StaffProfesionalSpecialityModel, Staff_ProfessionalSpeciality>
    {
        List<StaffProfesionalSpecialityModel> GetAllProfessionalSpeciality(BaseViewModel model);
        bool SaveStaffProfessionalList(List<StaffProfesionalSpecialityModel> profList);
    }


    public class StaffProfessionalSpecialityService :BaseService<StaffProfesionalSpecialityModel, Staff_ProfessionalSpeciality>, IStaffProfessionalSpecialityService
    {
        private readonly IStaffProfessioanlSpecialityRepository _spRepository;
        public StaffProfessionalSpecialityService(IStaffProfessioanlSpecialityRepository spRepository) : base(spRepository)
        {
            _spRepository = spRepository;
        }


        public bool SaveStaffProfessionalList(List<StaffProfesionalSpecialityModel> profList)
        {
            bool IsSuccessful;
            try
            {
                foreach (StaffProfesionalSpecialityModel aModel in profList)
                {
                    Utility.SetDynamicPropertyValue(aModel, aModel.CurrentCulture);
                    var staffspecial= Mapper.Map<StaffProfesionalSpecialityModel, Staff_ProfessionalSpeciality>(aModel);

                    if (aModel.ID >0)
                    {
                        _spRepository.UpdateStaffProfessinalSpeciality(staffspecial);
                    }
                    else
                    {
                        _spRepository.InsertStaffProfessinalSpeciality(staffspecial);
                    }
                }
                IsSuccessful = true;
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(0, "StaffProfession", message);
                throw new Exception(ex.Message);
            }
            return IsSuccessful;
        }


        public List<StaffProfesionalSpecialityModel> GetAllProfessionalSpeciality(BaseViewModel model)
        {
            List<StaffProfesionalSpecialityModel> spModel = new List<StaffProfesionalSpecialityModel>();
            StaffProfesionalSpecialityModel sModel = new StaffProfesionalSpecialityModel();
            try
            {
                List<Staff_ProfessionalSpeciality> statelist = _spRepository.GetAllStaffProfessinalSpecialityList();
                if (statelist != null)
                {
                    statelist.ForEach(a =>
                    {
                        sModel = Mapper.Map<Staff_ProfessionalSpeciality, StaffProfesionalSpecialityModel>(a);

                        sModel.CurrentUserID = model.CurrentUserID;
                        sModel.CurrentCulture = model.CurrentCulture;
                        spModel.Add(sModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "staff professional", message);
                throw new Exception(ex.Message);
            }

            return spModel;
        }




    }
}

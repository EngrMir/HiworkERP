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
    public partial interface IStaffSoftwareService:IBaseService<StaffSoftwareSkillModel,Staff_SoftwareSkill>
    {
        //StaffSoftwareSkillModel SaveStaffSoftware(StaffSoftwareSkillModel staff);
        List<StaffSoftwareSkillModel> GetStaffSoftware(BaseViewModel staff);
        bool staffSoftwareSkill(List<StaffSoftwareSkillModel> list);
       }


    public class StaffSoftwareSkillService:BaseService<StaffSoftwareSkillModel,Staff_SoftwareSkill>, IStaffSoftwareService
    {
        private readonly IStaffSoftwareRepository _staffsoftwareRepository;
        public StaffSoftwareSkillService(IStaffSoftwareRepository staffsoftwareRepository) : base(staffsoftwareRepository)
        {
            _staffsoftwareRepository = staffsoftwareRepository;

        }


        public bool staffSoftwareSkill(List<StaffSoftwareSkillModel> list)
        {
            bool IsSuccessful;
            try
            {
                foreach(StaffSoftwareSkillModel staff in list)
                {
                    var software = Mapper.Map<StaffSoftwareSkillModel, Staff_SoftwareSkill>(staff);
                    if (staff.ID == Guid.Empty)
                    {
                        software.CreatedDate = DateTime.Now;
                        software.ID = Guid.NewGuid();
                        _staffsoftwareRepository.InsertStaffSkill(software);
                    }
                    else
                    {
                        software.UpdatedDate = DateTime.Now;
                        software.UpdatedBy = staff.UpdatedBy;
                        _staffsoftwareRepository.UpdateStaffSkill(software);
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


        //public  StaffSoftwareSkillModel SaveStaffSoftware(StaffSoftwareSkillModel staff)
        //{
        //    Utility.SetDynamicPropertyValue(staff, staff.CurrentCulture);
        //    var software = Mapper.Map<StaffSoftwareSkillModel, Staff_SoftwareSkill>(staff);
        //    if (staff.ID == Guid.Empty)
        //    {   software.CreatedDate= DateTime.Now;
        //        software.ID = Guid.NewGuid();
        //        _staffsoftwareRepository.InsertStaffSkill(software);
        //    }
        //    else
        //    {
        //        software.UpdatedDate = DateTime.Now;
        //        software.UpdatedBy = staff.UpdatedBy;
        //        _staffsoftwareRepository.UpdateStaffSkill(software);
        //    }
        //    return staff;
        //}

     public   List<StaffSoftwareSkillModel> GetStaffSoftware(BaseViewModel staff)
        {
            List<StaffSoftwareSkillModel> skillList = new List<StaffSoftwareSkillModel>();
            StaffSoftwareSkillModel skillmodel = new StaffSoftwareSkillModel();
            try
            {
                List<Staff_SoftwareSkill> reposoftwareskill = _staffsoftwareRepository.GetStaffSoftware();

                if (reposoftwareskill != null)
                {
                    foreach (Staff_SoftwareSkill Skill in reposoftwareskill)
                    {
                        skillmodel = Mapper.Map<Staff_SoftwareSkill, StaffSoftwareSkillModel>(Skill);
                        skillmodel.Staff = Mapper.Map<Staff, StaffModel>(Skill.Staff);
                        skillmodel.MasterStaffSoftwareSkill = Mapper.Map<Master_StaffSoftwareSkill, MasterStaffSoftwareSkillModel>(Skill.Master_StaffSoftwareSkill);
                        skillmodel.CurrentUserID = staff.CurrentUserID;
                        skillmodel.CurrentCulture = staff.CurrentCulture;
                        skillList.Add(skillmodel);


                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(staff.CurrentUserID, "StaffSoftwareSkill", message);
                throw new Exception(ex.Message);
            }

            return skillList;





        }



        }



    }


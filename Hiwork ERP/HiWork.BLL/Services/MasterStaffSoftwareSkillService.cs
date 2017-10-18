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
    public partial interface IMasterStaffSoftwareSkillService : IBaseService<MasterStaffSoftwareSkillModel, Master_StaffSoftwareSkill>
    {
        MasterStaffSoftwareSkillModel SaveStaffSoftwareSkill(MasterStaffSoftwareSkillModel staff);
        List<MasterStaffSoftwareSkillModel> GetStaffSoftwareSkill(BaseViewModel staff);
        List<MasterStaffSoftwareSkillModel> DeleteStaffSoftwareSkill(MasterStaffSoftwareSkillModel staff);
    }



    public class MasterStaffSoftwareSkillService : BaseService<MasterStaffSoftwareSkillModel, Master_StaffSoftwareSkill>, IMasterStaffSoftwareSkillService
    {
        private readonly IMasterStaffSoftwareSkillRepository _staffsoftwareskillRepository;
        public MasterStaffSoftwareSkillService(IMasterStaffSoftwareSkillRepository staffsoftwareskillRepository ):base(staffsoftwareskillRepository)
            {
           
            _staffsoftwareskillRepository=staffsoftwareskillRepository;


             }
public MasterStaffSoftwareSkillModel SaveStaffSoftwareSkill(MasterStaffSoftwareSkillModel staff)
{
            Utility.SetDynamicPropertyValue(staff, staff.CurrentCulture);
            var staffsoftware = Mapper.Map<MasterStaffSoftwareSkillModel, Master_StaffSoftwareSkill>(staff);
           
           
            if(staffsoftware.ID>0)
            {
                staffsoftware.UpdatedBy = staff.CurrentUserID;
                staffsoftware.UpdatedDate = DateTime.Now;
                _staffsoftwareskillRepository.UpdateStaffSoftwareSkill(staffsoftware);

            }
            else
            {
                staffsoftware.CreatedDate = DateTime.Now;
                _staffsoftwareskillRepository.InsertStaffSoftwareSkill(staffsoftware);
            }

            return staff;
        }


    public List<MasterStaffSoftwareSkillModel> GetStaffSoftwareSkill(BaseViewModel staff)
        {
            List<MasterStaffSoftwareSkillModel> skillList = new List<MasterStaffSoftwareSkillModel>();
            MasterStaffSoftwareSkillModel skillmodel = new MasterStaffSoftwareSkillModel();
            try
            {
                List<Master_StaffSoftwareSkill> reposoftwareskill = _staffsoftwareskillRepository.GetStaffSoftwareSkill();

               if(reposoftwareskill !=null)
                {
                    foreach (Master_StaffSoftwareSkill Skill in reposoftwareskill)
                    {
                        skillmodel = Mapper.Map<Master_StaffSoftwareSkill, MasterStaffSoftwareSkillModel>(Skill);
                        skillmodel.Description = Utility.GetPropertyValue(skillmodel, "Description", staff.CurrentCulture) == null ? string.Empty :
                                                                Utility.GetPropertyValue(skillmodel, "Description", staff.CurrentCulture).ToString();

                        skillmodel.Name = Utility.GetPropertyValue(skillmodel, "Name", staff.CurrentCulture) == null ? string.Empty :
                                                                 Utility.GetPropertyValue(skillmodel, "Name", staff.CurrentCulture).ToString();
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


       public List<MasterStaffSoftwareSkillModel> DeleteStaffSoftwareSkill(MasterStaffSoftwareSkillModel astaff)

        {
            try {
                astaff.IsDeleted = true;
                this.SaveStaffSoftwareSkill(astaff);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(astaff.CurrentUserID, "StaffSoftwareSkill not Deleted", message);
                throw new Exception(ex.Message);
            }

            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = astaff.CurrentCulture;
            model.CurrentUserID = astaff.CurrentUserID;
            return this.GetStaffSoftwareSkill(model);



        }





    }

}
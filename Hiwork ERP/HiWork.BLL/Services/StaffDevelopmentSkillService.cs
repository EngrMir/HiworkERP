

/* ******************************************************************************************************************
 * Service for Master_StaffDevelopmentSkill Entity
 * Date             :   19-July-2017
 * By               :   Ashis Kr. Das
 * *****************************************************************************************************************/


using AutoMapper;
using System;
using System.Collections.Generic;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.BLL.Models;
using HiWork.DAL.Database;

namespace HiWork.BLL.Services
{

    public interface IStaffDevelopmentSkillService : IBaseService<StaffDevelopmentSkillModel, Master_StaffDevelopmentSkill>
    {
        List<StaffDevelopmentSkillModel> GetStaffDevelopmentSkillList(BaseViewModel model);
        List<StaffDevelopmentSkillModel> SaveStaffDevelopmentSkill(StaffDevelopmentSkillModel model);
        List<StaffDevelopmentSkillModel> DeleteStaffDevelopmentSkill(StaffDevelopmentSkillModel model);
    }

    public class StaffDevelopmentSkillService : BaseService<StaffDevelopmentSkillModel, Master_StaffDevelopmentSkill>,
                                                                    IStaffDevelopmentSkillService
    {
        private IStaffDevelopmentSkillRepository repository;
        public StaffDevelopmentSkillService(IStaffDevelopmentSkillRepository repo) : base(repo)
        {
            this.repository = repo;
        }

        public List<StaffDevelopmentSkillModel> GetStaffDevelopmentSkillList(BaseViewModel model)
        {
            object pvalue;
            List<Master_StaffDevelopmentSkill> datalist;
            StaffDevelopmentSkillModel devskill;
            List<StaffDevelopmentSkillModel> modlist = new List<StaffDevelopmentSkillModel>();

            try
            {
                datalist = repository.GetStaffDevelopmentSkillList();
                if (datalist != null)
                {
                    foreach (Master_StaffDevelopmentSkill data in datalist)
                    {
                        if (data.IsDeleted == true)
                            continue;

                        devskill = Mapper.Map<Master_StaffDevelopmentSkill, StaffDevelopmentSkillModel>(data);

                        pvalue = Utility.GetPropertyValue(devskill, "Name", model.CurrentCulture);
                        devskill.Name = pvalue == null ? string.Empty : pvalue.ToString();

                        pvalue = Utility.GetPropertyValue(devskill, "Description", model.CurrentCulture);
                        devskill.Description = pvalue == null ? string.Empty : pvalue.ToString();

                        devskill.CurrentCulture = model.CurrentCulture;
                        devskill.CurrentUserID = model.CurrentUserID;
                        modlist.Add(devskill);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }

            modlist.Sort(CompareStaffDevelopmentSkillByName);
            return modlist;
        }

        public List<StaffDevelopmentSkillModel> SaveStaffDevelopmentSkill(StaffDevelopmentSkillModel model)
        {
            Master_StaffDevelopmentSkill data;

            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                data = Mapper.Map<StaffDevelopmentSkillModel, Master_StaffDevelopmentSkill>(model);

                if (model.ID == Guid.Empty)
                {
                    data.ID = Guid.NewGuid();
                    data.CreatedBy = model.CurrentUserID;
                    data.CreatedDate = DateTime.Now;
                    repository.InsertStaffDevelopmentSkill(data);
                }
                else
                {
                    data.UpdatedBy = model.CurrentUserID;
                    data.UpdatedDate = DateTime.Now;
                    repository.UpdateStaffDevelopmentSkill(data);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetStaffDevelopmentSkillList(md);
        }

        public List<StaffDevelopmentSkillModel> DeleteStaffDevelopmentSkill(StaffDevelopmentSkillModel model)
        {
            List<StaffDevelopmentSkillModel> datalist;
            datalist = null;

            try
            {
                model.IsDeleted = true;
                datalist = this.SaveStaffDevelopmentSkill(model);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }
            return datalist;
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "StaffDevelopmentSkill", message);
            return message;
        }

        private int CompareStaffDevelopmentSkillByName(StaffDevelopmentSkillModel dataModel1, StaffDevelopmentSkillModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}

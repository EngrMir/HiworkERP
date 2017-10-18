

/* ******************************************************************************************************************
 * Service for Master_Team Entity
 * Date             :   08-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


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
    public interface ITeamService : IBaseService<TeamModel, Master_Team>
    {
        List<TeamModel> SaveTeam(TeamModel aTeamModel);
        List<TeamModel> GetTeamList(BaseViewModel model);
        TeamFormModel? GetTeamFormData(BaseViewModel model);
        List<TeamModel> DeleteTeam(TeamModel aTeamModel);
    }

    public class TeamService : BaseService<TeamModel, Master_Team>, ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        public TeamService(ITeamRepository teamRepository) : base(teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public List<TeamModel> SaveTeam(TeamModel aTeamModel)
        {
            Master_Team team = null;

            try
            {
                Utility.SetDynamicPropertyValue(aTeamModel, aTeamModel.CurrentCulture);
                team = Mapper.Map<TeamModel, Master_Team>(aTeamModel);

                if (aTeamModel.Id == Guid.Empty)
                {
                    team.ID = Guid.NewGuid();
                    team.CountryID = aTeamModel.CountryId;
                    team.BranchID = aTeamModel.BranchId;
                    team.DivisionID = aTeamModel.DivisionId;
                    team.DepartmentID = aTeamModel.DepartmentId;
                    team.CreatedBy = aTeamModel.CurrentUserID;
                    team.CreatedDate = DateTime.Now;
                    _teamRepository.InsertTeam(team);
                }
                else
                {
                    team.CountryID = aTeamModel.CountryId;
                    team.BranchID = aTeamModel.BranchId;
                    team.DivisionID = aTeamModel.DivisionId;
                    team.DepartmentID = aTeamModel.DepartmentId;
                    team.UpdatedBy = aTeamModel.CurrentUserID;
                    team.UpdatedDate = DateTime.Now;
                    _teamRepository.UpdateTeam(team);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, aTeamModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = aTeamModel.CurrentCulture;
            md.CurrentUserID = aTeamModel.CurrentUserID;
            return GetTeamList(md);
        }

        public List<TeamModel> GetTeamList(BaseViewModel model)
        {
            object pValue;
            string sValue;

            List<Master_Team> dataList;
            List<TeamModel> teamList = new List<TeamModel>();
            TeamModel teamModel = new TeamModel();

            try
            {
                dataList = _teamRepository.GetTeamList();
                if (dataList != null)
                {
                    foreach(Master_Team cmp_model in dataList)
                    {
                        teamModel = Mapper.Map<Master_Team, TeamModel>(cmp_model);
                        teamModel.Country = Mapper.Map<Master_Country, CountryModel>(cmp_model.Master_Country);
                        teamModel.Branch = Mapper.Map<Master_BranchOffice, BranchModel>(cmp_model.Master_BranchOffice);
                        teamModel.Division = Mapper.Map<Master_Division, DivisionModel>(cmp_model.Master_Division);
                        teamModel.Department = Mapper.Map<Master_Department, DepartmentModel>(cmp_model.Master_Department);

                        if (teamModel.Country != null)
                        {
                            pValue = Utility.GetPropertyValue(teamModel.Country, "Name", model.CurrentCulture);
                            sValue = pValue == null ? string.Empty : pValue.ToString();
                            teamModel.Country.Name = sValue;
                        }

                        if (teamModel.Branch != null)
                        {
                            pValue = Utility.GetPropertyValue(teamModel.Branch, "Name", model.CurrentCulture);
                            sValue = pValue == null ? string.Empty : pValue.ToString();
                            teamModel.Branch.Name = sValue;
                        }

                        if (teamModel.Division != null)
                        {
                            pValue = Utility.GetPropertyValue(teamModel.Division, "Name", model.CurrentCulture);
                            sValue = pValue == null ? string.Empty : pValue.ToString();
                            teamModel.Division.Name = sValue;
                        }

                        if (teamModel.Department != null)
                        {
                            pValue = Utility.GetPropertyValue(teamModel.Department, "Name", model.CurrentCulture);
                            sValue = pValue == null ? string.Empty : pValue.ToString();
                            teamModel.Department.Name = sValue;

                            pValue = Utility.GetPropertyValue(teamModel.Department, "Description", model.CurrentCulture);
                            sValue = pValue == null ? string.Empty : pValue.ToString();
                            teamModel.Department.Description = sValue;
                        }

                        pValue = Utility.GetPropertyValue(teamModel, "Name", model.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        teamModel.Name = sValue;

                        pValue = Utility.GetPropertyValue(teamModel, "Description", model.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        teamModel.Description = sValue;

                        teamModel.CurrentUserID = model.CurrentUserID;
                        teamModel.CurrentCulture = model.CurrentCulture;
                        teamList.Add(teamModel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }

            teamList.Sort(CompareTeamByName);
            return teamList;
        }

        public List<TeamModel> DeleteTeam(TeamModel aTeamModel)
        {
            try
            {
                _teamRepository.DeleteTeam(aTeamModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, aTeamModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = aTeamModel.CurrentCulture;
            md.CurrentUserID = aTeamModel.CurrentUserID;
            return GetTeamList(md);
        }

        public TeamFormModel? GetTeamFormData(BaseViewModel dataModel)
        {
            IUnitOfWork uwork = new UnitOfWork();
            IDivisionService sdiv;
            ICountryService scon;
            IBranchService sbrn;
            IDepartmentService sdep;

            TeamFormModel? data;
            TeamFormModel model;
            sdiv = new DivisionService(new DivisionRepository(uwork));
            scon = new CountryService(new CountryRepository(uwork));
            sbrn = new BranchService(new BranchRepository(uwork));
            sdep = new DepartmentService(new DepartmentRepository(uwork));

            try
            {
                model.divisionList = sdiv.GetDivisionList(dataModel);
                model.countryList = scon.GetAllCountryList(dataModel);
                model.branchList = sbrn.GetAllBranchList(dataModel);
                model.departmentList = sdep.GetAllDepartmentList(dataModel);
                data = model;
            }
            catch (Exception ex)
            {
                data = null;
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }
            return data;
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "Team", message);
            return message;
        }

        private int CompareTeamByName(TeamModel dataModel1, TeamModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}

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

    public interface IDesignationService: IBaseService<DesignationModel, Master_Designation>
    {
        List<DesignationModel> SaveDesignation(DesignationModel model);
        List<DesignationModel> GetAllDesignationList(BaseViewModel model);
        DesignationFormModel GetDesignationFormData(BaseViewModel arg);
        List<DesignationModel> DeleteDesignation(DesignationModel model);
        List<DesignationModel> UpdateDesignation(DesignationModel model);
    }
    public class DesignationService : BaseService<DesignationModel, Master_Designation>, IDesignationService
    {
        public IDesignationRepository _designationRepository;
        public DesignationService(IDesignationRepository designationRepository) : base(designationRepository)
        {
            _designationRepository = designationRepository;
        }

        public List<DesignationModel> DeleteDesignation(DesignationModel model)
        {
            _designationRepository.DeleteDesignation(model.ID);
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.CurrentCulture = model.CurrentCulture;
            baseViewModel.CurrentUserID = model.CurrentUserID;
            var Designations = GetAllDesignationList(baseViewModel);
            return Designations;
        }

        public List<DesignationModel> GetAllDesignationList(BaseViewModel model)
        {
            List<DesignationModel> designationList = new List<DesignationModel>();
            DesignationModel designation = new DesignationModel();


            try
            {
                List<Master_Designation> dbDesignationList = _designationRepository.GetAllDesignations();

                if (dbDesignationList != null)
                {
                    dbDesignationList.ForEach(a =>
                    {
                        designation = Mapper.Map<Master_Designation, DesignationModel>(a);

                        designation.countrymodel = Mapper.Map<Master_Country, CountryModel>(a.Master_Country);

                        designation.branchmodel = Mapper.Map<Master_BranchOffice, BranchModel>(a.Master_BranchOffice);

                        designation.divisionmodel = Mapper.Map<Master_Division, DivisionModel>(a.Master_Division);

                        designation.departmentmodel = Mapper.Map<Master_Department, DepartmentModel>(a.Master_Department);

                        designation.teammodel = Mapper.Map<Master_Team, TeamModel>(a.Master_Team);


                        if (designation.countrymodel != null)
                            designation.countrymodel.Name = Utility.GetPropertyValue(designation.countrymodel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(designation.countrymodel, "Name", model.CurrentCulture).ToString();

                        if (designation.branchmodel != null)
                            designation.branchmodel.Name = Utility.GetPropertyValue(designation.branchmodel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(designation.branchmodel, "Name", model.CurrentCulture).ToString();

                        if (designation.divisionmodel != null)
                            designation.divisionmodel.Name = Utility.GetPropertyValue(designation.divisionmodel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(designation.divisionmodel, "Name", model.CurrentCulture).ToString();

                        if (designation.departmentmodel != null)
                            designation.departmentmodel.Name = Utility.GetPropertyValue(designation.departmentmodel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(designation.departmentmodel, "Name", model.CurrentCulture).ToString();

                        if (designation.teammodel != null)
                            designation.teammodel.Name = Utility.GetPropertyValue(designation.teammodel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(designation.teammodel, "Name", model.CurrentCulture).ToString();

                        designation.Name = Utility.GetPropertyValue(designation, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(designation, "Name", model.CurrentCulture).ToString();


                        designation.CurrentUserID = model.CurrentUserID;
                        designation.ID = a.ID;
                        designation.CurrentCulture = model.CurrentCulture;
                        designationList.Add(designation);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Company Designation", message);
                throw new Exception(ex.Message);
            }

            designationList.Sort(CompareDesignationByName);
            return designationList;
        }

        public List<DesignationModel> SaveDesignation(DesignationModel model)
        {
            Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

            var designation = Mapper.Map<DesignationModel, Master_Designation>(model);

            if (model.ID == Guid.Empty)
            {
                designation.ID = Guid.NewGuid();
                designation.CreatedBy = model.CreatedBy;
                designation.CreatedDate = DateTime.Now;
                _designationRepository.InsertDesignation(designation);
            }
            else
            {

                designation.UpdatedBy = model.UpdatedBy;
                designation.UpdatedDate = DateTime.Now;
                _designationRepository.UpdateDesignation(designation);

            }
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.CurrentCulture = model.CurrentCulture;
            baseViewModel.CurrentUserID = model.CurrentUserID;
            var Designations = GetAllDesignationList(baseViewModel);
            return Designations;
        }

        public List<DesignationModel> UpdateDesignation(DesignationModel model)
        {
            var designation = Mapper.Map<DesignationModel, Master_Designation>(model);

            _designationRepository.UpdateDesignation(designation);

            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.CurrentCulture = model.CurrentCulture;
            baseViewModel.CurrentUserID = model.CurrentUserID;
            var Designations = GetAllDesignationList(baseViewModel);
            return Designations;
        }


        public DesignationFormModel GetDesignationFormData(BaseViewModel arg)
        {
            ICountryService scon;
            IBranchService sbrn;
            IDivisionService sdiv;
            IDepartmentService sdep;
            ITeamService stem;

            IUnitOfWork uwork = new UnitOfWork();
            DesignationFormModel model = new DesignationFormModel();
            scon = new CountryService(new CountryRepository(uwork));
            sbrn = new BranchService(new BranchRepository(uwork));
            sdiv = new DivisionService(new DivisionRepository(uwork));
            sdep = new DepartmentService(new DepartmentRepository(uwork));
            stem = new TeamService(new TeamRepository(uwork));

            try
            {
                model.countryList = scon.GetAllCountryList(arg);
                model.branchList = sbrn.GetAllBranchList(arg);
                model.divisionList = sdiv.GetDivisionList(arg);
                model.departmentList = sdep.GetAllDepartmentList(arg);
                model.teamList = stem.GetTeamList(arg);
            }
            catch (Exception ex)
            {
                model = null;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(arg.CurrentUserID, "Company Designation", message);
                throw new Exception(ex.Message);
            }
            return model;
        }

        private int CompareDesignationByName(DesignationModel arg1, DesignationModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}

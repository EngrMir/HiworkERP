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
    public partial interface IDepartmentService : IBaseService<DepartmentModel, Master_Department>
    {
        List<DepartmentModel> SaveDepartment(DepartmentModel aDepartmentModel);
        List<DepartmentModel> GetAllDepartmentList(BaseViewModel model);
        DepartmentFormModel GetDepartmentFormData(BaseViewModel arg);
        List<DepartmentModel> DeleteDepartment(DepartmentModel aDepartmentModel);
    }

    public class DepartmentService : BaseService<DepartmentModel, Master_Department>, IDepartmentService
    {
        private readonly IDepartmentRepository _deptRepository;
        public DepartmentService(IDepartmentRepository deptRepository) : base(deptRepository)
        {
            _deptRepository = deptRepository;
        }

        public List<DepartmentModel> SaveDepartment(DepartmentModel aDepartmentModel)
        {
            Master_Department department = null;

            try
            {
                Utility.SetDynamicPropertyValue(aDepartmentModel, aDepartmentModel.CurrentCulture);
                department = Mapper.Map<DepartmentModel, Master_Department>(aDepartmentModel);

                if (aDepartmentModel.ID==Guid.Empty)
                {
                    department.ID = Guid.NewGuid();
                    department.CountryID = aDepartmentModel.Country.ID;
                    department.CreatedBy = aDepartmentModel.CurrentUserID;
                    department.CreatedDate = DateTime.Now;
                    _deptRepository.InsertDepartment(department);
                }
                else
                {
                    department.CountryID = aDepartmentModel.Country.ID; 
                    department.UpdatedBy = aDepartmentModel.CurrentUserID;
                    department.UpdatedDate = DateTime.Now;
                    _deptRepository.UpdateDepartment(department);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aDepartmentModel.CurrentUserID, "Department", message);
                throw new Exception(ex.Message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = aDepartmentModel.CurrentCulture;
            md.CurrentUserID = aDepartmentModel.CurrentUserID;
            return GetAllDepartmentList(md);
        }

        public List<DepartmentModel> GetAllDepartmentList(BaseViewModel model)
        {
            List<DepartmentModel> departmentList = new List<DepartmentModel>();
            DepartmentModel departmentModel = new DepartmentModel();

            try
            {
                List<Master_Department> repoDepartmentList = _deptRepository.GetAllDepartmentList();
                if (repoDepartmentList != null)
                {
                    repoDepartmentList.ForEach(a =>
                    {
                        departmentModel = Mapper.Map<Master_Department, DepartmentModel>(a);
                        departmentModel.Country = Mapper.Map<Master_Country, CountryModel>(a.Master_Country);
                        departmentModel.Branch = Mapper.Map<Master_BranchOffice, BranchModel>(a.Master_BranchOffice);
                       // departmentModel.Division = Mapper.Map<Master_Division, DivisionModel>(a.Master_Division);
                        
                        if (departmentModel.Country != null)
                            departmentModel.Country.Name = Utility.GetPropertyValue(departmentModel.Country, "Name", model.CurrentCulture) == null ? string.Empty :
                                                           Utility.GetPropertyValue(departmentModel.Country, "Name", model.CurrentCulture).ToString();
                        
                        if (departmentModel.Branch != null)
                            departmentModel.Branch.Name = Utility.GetPropertyValue(departmentModel.Branch, "Name", model.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(departmentModel.Branch, "Name", model.CurrentCulture).ToString();
                        
                        //if (departmentModel.Division != null)
                        //    departmentModel.Division.Name = Utility.GetPropertyValue(departmentModel.Division, "Name", model.CurrentCulture) == null ? string.Empty :
                        //                                    Utility.GetPropertyValue(departmentModel.Division, "Name", model.CurrentCulture).ToString();
                        
                        departmentModel.Name = Utility.GetPropertyValue(departmentModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(departmentModel, "Name", model.CurrentCulture).ToString();
                        
                        departmentModel.Description = Utility.GetPropertyValue(departmentModel, "Description", model.CurrentCulture) == null ? string.Empty :
                                                             Utility.GetPropertyValue(departmentModel, "Description", model.CurrentCulture).ToString();
                        
                        departmentModel.CurrentUserID = model.CurrentUserID;
                        departmentModel.CurrentCulture = model.CurrentCulture;
                        departmentList.Add(departmentModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Department", message);
                throw new Exception(ex.Message);
            }

            departmentList.Sort(CompareDepartmentByName);
            return departmentList;
        }

        public List<DepartmentModel> DeleteDepartment(DepartmentModel aDepartmentModel)
        {
            List<DepartmentModel> departments = null;
            try
            {
                _deptRepository.DeleteDepartment(aDepartmentModel.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aDepartmentModel.CurrentCulture;
                baseViewModel.CurrentUserID = aDepartmentModel.CurrentUserID;
                departments = GetAllDepartmentList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aDepartmentModel.CurrentUserID, "Department", message);
                throw new Exception(ex.Message);
            }
            return departments;
        }

        public DepartmentFormModel GetDepartmentFormData(BaseViewModel arg)
        {
            ICountryService scon;
            IBranchService sbrn;
            //IDivisionService sdiv;
            IUnitOfWork uwork = new UnitOfWork();
            DepartmentFormModel model = new DepartmentFormModel();
            scon = new CountryService(new CountryRepository(uwork));
            sbrn = new BranchService(new BranchRepository(uwork));
            //sdiv = new DivisionService(new DivisionRepository(uwork));

            try
            {
                model.countryList = scon.GetAllCountryList(arg);
                model.branchList = sbrn.GetAllBranchList(arg);
               // model.divisionList = sdiv.GetDivisionList(arg);
            }
            catch (Exception ex)
            {
                model = null;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(arg.CurrentUserID, "Department", message);
                throw new Exception(ex.Message);
            }
            return model;
        }

        private int CompareDepartmentByName(DepartmentModel arg1, DepartmentModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}


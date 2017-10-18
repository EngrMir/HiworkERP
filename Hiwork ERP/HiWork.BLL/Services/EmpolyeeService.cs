


using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HiWork.BLL.Services
{
    public interface IEmployeeService : IBaseService<EmployeeModel, Employee>
    {
        EmployeeModel  SaveEmployee(EmployeeModel model);
        List<EmployeeModel>  GetEmployeeList(BaseViewModel model);
        List<EmployeeModel>  DeleteEmployee(EmployeeModel model);
        EmployeeModel  UpdateEmployee(EmployeeModel model);
        EmployeeFormModel  GetEmployeeFormData(BaseViewModel model);
        EmployeeModel GetEmployeeByID(BaseViewModel model);
        bool CheckEmployeeByEmployeeID(EmployeeModel model);
        List<EmployeeModel> GetSearchEmployeeList(BaseViewModel model, string con);
        List<RoleModel> GetAllRoleList(BaseViewModel model);
    }

    public class EmployeeService : BaseService<EmployeeModel, Employee>, IEmployeeService
    {
        public IEmployeeRepository _employeRepository;
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel basemodel;

        public EmployeeService(IEmployeeRepository empolyeRepository) : base(empolyeRepository)
        {
            _employeRepository = empolyeRepository;
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            basemodel = new BaseViewModel();
        }
        public List<RoleModel> GetAllRoleList(BaseViewModel model)
        {
            List<RoleModel> merList = new List<RoleModel>();
            RoleModel ccModel = new RoleModel();

            try
            {
                List<Role> repoRoleList = _dbContext.Roles.ToList();
                if (repoRoleList != null)
                {
                    repoRoleList.ForEach(a =>
                    {
                        ccModel = Mapper.Map<Role, RoleModel>(a);
                        ccModel.Name = Utility.GetPropertyValue(ccModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(ccModel, "Name", model.CurrentCulture).ToString();
                        ccModel.CurrentUserID = model.CurrentUserID;
                        ccModel.CurrentCulture = model.CurrentCulture;
                        merList.Add(ccModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Country", message);
                throw new Exception(ex.Message);
            }
            merList.Sort(CompareRoleByName);
            return merList;
        }
        private int CompareRoleByName(RoleModel arg1, RoleModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
        public List<EmployeeModel> DeleteEmployee(EmployeeModel model)
        {
            try
            {
                _employeRepository.DeleteEmployee(model.ID);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }
            
            basemodel.CurrentCulture = model.CurrentCulture;
            basemodel.CurrentUserID = model.CurrentUserID;
            return GetEmployeeList(basemodel);
        }
        public List<EmployeeModel> GetSearchEmployeeList(BaseViewModel model, string con)
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            EmployeeModel employeeModel;
            try
            {
                string branch = con.Split(',')[0];
                long type = Convert.ToInt32(con.Split(',')[1]);
                string eid = con.Split(',')[2];
                bool resign = Convert.ToBoolean(con.Split(',')[3]);
                string role = con.Split(',')[4];
                string author = con.Split(',')[5];
                string ename = con.Split(',')[6];
                _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetSearchAllEmployee", _sqlConnService.CreateConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                if (model.ID == Guid.Empty)
                {
                    command.Parameters.AddWithValue("@ID", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@ID", model.ID);
                }
                command.Parameters.AddWithValue("@EmployeeId", DBNull.Value);
                command.Parameters.AddWithValue("@EmployeeTypeID", DBNull.Value);
                command.Parameters.AddWithValue("@DepartmentID", DBNull.Value);
                if (branch == "")
                    command.Parameters.AddWithValue("@branch", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@branch", branch);
                if (type == 0)
                    command.Parameters.AddWithValue("@type", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@type", type);
                if (eid == "")
                    command.Parameters.AddWithValue("@eid", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@eid", eid);
                if (resign == false)
                    command.Parameters.AddWithValue("@resign", 1);
                else
                    command.Parameters.AddWithValue("@resign", 0); 
                if (role == "")
                    command.Parameters.AddWithValue("@role", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@role", role);
                if (author == "")
                    command.Parameters.AddWithValue("@author", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@author", author);
                if (ename == "")
                    command.Parameters.AddWithValue("@ename", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@ename", ename);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employeeModel = new EmployeeModel();
                    employeeModel.ID = Guid.Parse(reader["ID"].ToString());
                    employeeModel.Name = reader["Name"].ToString();
                    employeeModel.RegistrationID = Convert.ToInt64(reader["RegistrationID"].ToString());
                    employeeModel.EmployeeID = reader["EmployeeID"].ToString();
                    employeeModel.CountryName = reader["CountryName"].ToString();
                    employeeModel.CountryID = Convert.ToInt64(reader["CountryID"].ToString());
                    employeeModel.DepartmentName = reader["DepartmentName"].ToString();
                    employeeModel.DepartmentID = Guid.Parse(reader["DepartmentID"].ToString());
                    employeeModel.BranchName = reader["BranchName"].ToString();
                    employeeModel.BranchOfficeID = reader["BranchOfficeID"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["BranchOfficeID"].ToString());
                    employeeModel.EmployeeTypeName = reader["EmployeeTypeName"].ToString();
                    if (reader["EmployeeTypeID"] != DBNull.Value)
                    {
                        employeeModel.EmployeeTypeID = Convert.ToInt64(reader["EmployeeTypeID"].ToString().Trim());
                    }
                    employeeModel.Password = reader["Password"].ToString();
                    employeeModel.BirthDate = Convert.ToDateTime(reader["BirthDate"].ToString());
                    employeeModel.JoiningDate = Convert.ToDateTime(reader["JoiningDate"].ToString());
                    employeeModel.RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"].ToString());
                    employeeModel.LeavingDate = reader["LeavingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LeavingDate"].ToString());
                    employeeModel.AttendanceDay = Convert.ToInt32(reader["AttendanceDay"].ToString());
                    employeeModel.ClockInTime = reader["ClockInTime"].ToString();
                    employeeModel.ClockOutTime = reader["ClockOutTime"].ToString();
                    employeeModel.SkypeID = reader["SkypeID"].ToString();
                    employeeModel.SkypePassword = reader["SkypePassword"].ToString();
                    employeeModel.Email = reader["Email"].ToString();
                    employeeModel.IsResponsiblePerson = Convert.ToBoolean(reader["IsResponsiblePerson"].ToString());
                    employeeModel.HomeAddress = reader["HomeAddress"].ToString();
                    employeeModel.HomePhone = reader["HomePhone"].ToString();
                    employeeModel.MobilePhone = reader["MobilePhone"].ToString();
                    employeeModel.PCEmail = reader["PCEmail"].ToString();
                    employeeModel.MobileEmail = reader["MobileEmail"].ToString();
                    employeeModel.Sns_one = reader["Sns_One"].ToString();
                    employeeModel.Sns_two = reader["Sns_two"].ToString();
                    employeeModel.BankName = reader["BankName"].ToString();
                    employeeModel.BankID = Guid.Parse(reader["BankID"].ToString());
                    employeeModel.BankBranchName = reader["BankBranchName"].ToString();
                    employeeModel.BankBranchID = Guid.Parse(reader["BankBranchID"].ToString());
                    employeeModel.BankAccountTypeName = reader["BankBranchName"].ToString();
                    employeeModel.BankAccountTypeID = Convert.ToInt64(reader["BankAccountTypeID"].ToString());
                    employeeModel.BankAccountNumber = reader["BankAccountNumber"].ToString();
                    employeeModel.BankAccountName = reader["BankAccountName"].ToString();
                    employeeModel.Photo = reader["Photo"].ToString();
                    employeeModel.Signature = reader["Signature"].ToString();
                    employeeModel.Language_one = Guid.Parse(reader["Language_one"].ToString());
                    employeeModel.LanguageName1 = reader["LanguageName1"].ToString();
                    employeeModel.Language_two = Guid.Parse(reader["Language_two"].ToString());
                    employeeModel.LanguageName1 = reader["LanguageName2"].ToString();
                    employeeModel.Language_three = Guid.Parse(reader["Language_three"].ToString());
                    employeeModel.LanguageName1 = reader["LanguageName3"].ToString();
                    employeeModel.IsActive = Convert.ToBoolean(reader["Active"].ToString());
                    employeeModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    employeeModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    employeeModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    employeeModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    employeeList.Add(employeeModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }
            return employeeList;
        }
        public List<EmployeeModel> GetEmployeeList(BaseViewModel model)
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            EmployeeModel employeeModel;

            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetAllEmployee", _sqlConnService.CreateConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                if(model.ID==Guid.Empty){
                    command.Parameters.AddWithValue("@ID", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@ID", model.ID);
                }
                command.Parameters.AddWithValue("@EmployeeId", DBNull.Value);
                command.Parameters.AddWithValue("@EmployeeTypeID", DBNull.Value);
                //command.Parameters.AddWithValue("@DivisionID", DBNull.Value);
                command.Parameters.AddWithValue("@DepartmentID", DBNull.Value);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    employeeModel = new EmployeeModel();
                    employeeModel.ID = Guid.Parse(reader["ID"].ToString());
                    employeeModel.Name = reader["Name"].ToString();
                    employeeModel.RegistrationID =Convert.ToInt64(reader["RegistrationID"].ToString());
                    employeeModel.EmployeeID = reader["EmployeeID"].ToString();
                    //employeeModel.DivisionName = reader["DivisionName"].ToString();
                    //employeeModel.DivisionID =Guid.Parse( reader["DivisionID"].ToString());
                    employeeModel.CountryName = reader["CountryName"].ToString();
                    employeeModel.CountryID = Convert.ToInt64(reader["CountryID"].ToString());
                    employeeModel.DepartmentName = reader["DepartmentName"].ToString();
                    employeeModel.DepartmentID = Guid.Parse(reader["DepartmentID"].ToString());
                    employeeModel.BranchName = reader["BranchName"].ToString();
                    employeeModel.BranchOfficeID = reader["BranchOfficeID"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["BranchOfficeID"].ToString());
                    employeeModel.EmployeeTypeName = reader["EmployeeTypeName"].ToString();

                    if (reader["EmployeeTypeID"] != DBNull.Value)
                    {
                        employeeModel.EmployeeTypeID = Convert.ToInt64(reader["EmployeeTypeID"].ToString().Trim());
                    }

                    employeeModel.Password = reader["Password"].ToString();
                    employeeModel.BirthDate = Convert.ToDateTime(reader["BirthDate"].ToString());
                    employeeModel.JoiningDate = Convert.ToDateTime(reader["JoiningDate"].ToString());
                    employeeModel.RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"].ToString());
                    employeeModel.LeavingDate = reader["LeavingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LeavingDate"].ToString());
                    employeeModel.AttendanceDay = Convert.ToInt32(reader["AttendanceDay"].ToString());
                    employeeModel.ClockInTime = reader["ClockInTime"].ToString();
                    employeeModel.ClockOutTime = reader["ClockOutTime"].ToString();
                    employeeModel.SkypeID = reader["SkypeID"].ToString();
                    employeeModel.SkypePassword = reader["SkypePassword"].ToString();
                    employeeModel.Email = reader["Email"].ToString();
                    employeeModel.IsResponsiblePerson = Convert.ToBoolean(reader["IsResponsiblePerson"].ToString());
                    employeeModel.HomeAddress = reader["HomeAddress"].ToString();
                    employeeModel.HomePhone = reader["HomePhone"].ToString();
                    employeeModel.MobilePhone = reader["MobilePhone"].ToString();
                    employeeModel.PCEmail = reader["PCEmail"].ToString();
                    employeeModel.MobileEmail = reader["MobileEmail"].ToString();
                    employeeModel.Sns_one = reader["Sns_One"].ToString();
                    employeeModel.Sns_two = reader["Sns_two"].ToString();
                    employeeModel.BankName = reader["BankName"].ToString();
                    employeeModel.BankID = Guid.Parse(reader["BankID"].ToString());
                    employeeModel.BankBranchName = reader["BankBranchName"].ToString();
                    employeeModel.BankBranchID = Guid.Parse(reader["BankBranchID"].ToString());
                    employeeModel.BankAccountTypeName = reader["BankBranchName"].ToString();
                    employeeModel.BankAccountTypeID =Convert.ToInt64(reader["BankAccountTypeID"].ToString());
                    employeeModel.BankAccountNumber = reader["BankAccountNumber"].ToString();
                    employeeModel.BankAccountName = reader["BankAccountName"].ToString();
                    employeeModel.Photo = reader["Photo"].ToString();
                    employeeModel.Signature = reader["Signature"].ToString();
                    employeeModel.Language_one = Guid.Parse(reader["Language_one"].ToString());
                    employeeModel.LanguageName1 = reader["LanguageName1"].ToString();
                    employeeModel.Language_two = Guid.Parse(reader["Language_two"].ToString());
                    employeeModel.LanguageName1 = reader["LanguageName2"].ToString();
                    employeeModel.Language_three = Guid.Parse(reader["Language_three"].ToString());
                    employeeModel.LanguageName1 = reader["LanguageName3"].ToString();
                    employeeModel.IsActive = Convert.ToBoolean(reader["Active"].ToString());
                    employeeModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    employeeModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    employeeModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    employeeModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    employeeList.Add(employeeModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }

            return employeeList;
        }

        public EmployeeModel GetEmployeeByID(BaseViewModel model)
        {

            try
            {

                var dbEmployee = _employeRepository.GetEmployeeById(model.ID);

              


                var employee = Mapper.Map<Employee, EmployeeModel>(dbEmployee);

                var dept = Mapper.Map<Master_Department, DepartmentModel>(dbEmployee.Master_Department);

                var branch = Mapper.Map<Master_BranchOffice, BranchModel>(dbEmployee.Master_BranchOffice);

                employee.DepartmentName = Utility.GetPropertyValue(dept, "Name", model.CurrentCulture) == null ? string.Empty :
                                                    Utility.GetPropertyValue(dept, "Name", model.CurrentCulture).ToString();
                employee.BranchName = Utility.GetPropertyValue(branch, "Name", model.CurrentCulture) == null ? string.Empty :
                                                  Utility.GetPropertyValue(branch, "Name", model.CurrentCulture).ToString();




                return employee;
            }
            catch(Exception ex)
            {

            }
            return null;
          
        }
        public EmployeeFormModel GetEmployeeFormData(BaseViewModel model)
        {
            IUnitOfWork uwork = new UnitOfWork();
            EmployeeFormModel formData = new EmployeeFormModel();

            ICountryService countrySerivce = new CountryService(new CountryRepository(uwork));
          //  IDivisionService divisionService = new DivisionService(new DivisionRepository(uwork));
            IDepartmentService departmentService = new DepartmentService(new DepartmentRepository(uwork));
            IRoleService roleService = new RoleService(new RoleRepository(uwork));
            IBranchService branchService = new BranchService(new BranchRepository(uwork));
            IBankService bankService = new BankService(new BankRepository(uwork));
            IBankBranchService bankBranchService = new BankBranchSerivce(new BankBranchRepository(uwork));
            IBankAccountService banAccService = new BankAccountService(new BankAccountRepository(uwork));
            ILanguageService langService = new LanguageService(new LanguageRepository(uwork));
            IApplicationService appService = new ApplicationService(new ApplicationRepository(uwork));
            
            basemodel.CurrentCulture = model.CurrentCulture;
            basemodel.CurrentUserID = model.CurrentUserID;

            var LastEmployee = GetEmployeeList(basemodel).LastOrDefault();
            long NextID = LastEmployee==null ? 0 : LastEmployee.RegistrationID + 1;

            try
            {
                formData.countries = countrySerivce.GetAllCountryList(model).Where(c=>c.IsActive==true).ToList();
               // formData.divisions = divisionService.GetDivisionList(model);
                formData.departments = departmentService.GetAllDepartmentList(model).Where(c => c.IsActive == true).ToList();
                formData.roles = roleService.GetAllRoleList(model).Where(c => c.IsActive == true).ToList();
                formData.branches = branchService.GetAllBranchList(model).Where(c => c.IsActive == true).ToList();
                formData.banks = bankService.GetAllBankList(model).Where(c => c.IsActive == true).ToList();
                formData.bankbranches = bankBranchService.GetAllBankBranchList(model).Where(c => c.IsActive == true).ToList();
                formData.bankAccountTypes = banAccService.GetAllBankAccountType(model).Where(c => c.IsActive == true).ToList();
                formData.languages = langService.GetAllLanguageList(model).Where(c => c.IsActive == true).ToList();
                //formData.EmployeeNo = Helper.GenerateUniqueID(appService.GetApplicationCode(model.ApplicationId), NextID.ToString());
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }
            return formData;
        }

        public EmployeeModel SaveEmployee(EmployeeModel model)
        {
            UserInfoModel  userModel = new UserInfoModel();
            userModel.Id = 0;
            userModel.EmployeeID = model.ID;

            IUserInfoService userInfoService = new UserInfoService(new UserInfoRepository(new UnitOfWork()));

            try
            {
                SqlCommand cmd;
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_SaveDeleteEmployee", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", model.EmployeeID);
                cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
                cmd.Parameters.AddWithValue("@CountryID", model.CountryID);
                //cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
                cmd.Parameters.AddWithValue("@BranchOfficeID", model.BranchOfficeID);
                cmd.Parameters.AddWithValue("@EmployeeTypeID", model.EmployeeTypeID);
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Name_" + model.CurrentCulture, model.Name);
                cmd.Parameters.AddWithValue("@Password",Utility.MD5( model.Password));
                cmd.Parameters.AddWithValue("@BirthDate", model.BirthDate);
                cmd.Parameters.AddWithValue("@JoiningDate", model.JoiningDate);
                cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@LeavingDate", model.LeavingDate < DateTime.Now ? null : model.LeavingDate);
                cmd.Parameters.AddWithValue("@AttendanceDay", model.AttendanceDay);
                cmd.Parameters.AddWithValue("@ClockInTime", model.ClockInTime);
                cmd.Parameters.AddWithValue("@ClockOutTime", model.ClockOutTime);
                cmd.Parameters.AddWithValue("@SkypeID", model.SkypeID);
                cmd.Parameters.AddWithValue("@SkypePassword", model.SkypePassword);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@IsResponsiblePerson", model.IsResponsiblePerson);
                cmd.Parameters.AddWithValue("HomeAddress_" + model.CurrentCulture, model.HomeAddress);
                cmd.Parameters.AddWithValue("@HomePhone", model.HomePhone);
                cmd.Parameters.AddWithValue("@MobilePhone", model.MobilePhone);
                cmd.Parameters.AddWithValue("@PCEmail", model.PCEmail);
                cmd.Parameters.AddWithValue("@MobileEmail", model.MobileEmail);
                cmd.Parameters.AddWithValue("@Sns_one", model.Sns_one);
                cmd.Parameters.AddWithValue("@Sns_two", model.Sns_two);
                cmd.Parameters.AddWithValue("@BankID", model.BankID);
                cmd.Parameters.AddWithValue("@BankBranchID", model.BankBranchID);
                cmd.Parameters.AddWithValue("@BankAccountTypeID", model.BankAccountTypeID);
                cmd.Parameters.AddWithValue("@BankAccountNumber", model.BankAccountNumber);
                cmd.Parameters.AddWithValue("@BankAccountName", model.BankAccountName);
                cmd.Parameters.AddWithValue("@Photo", model.Photo);
                cmd.Parameters.AddWithValue("@Signature", model.Signature);
                cmd.Parameters.AddWithValue("@Institute_" + model.CurrentCulture, model.Institute);
                cmd.Parameters.AddWithValue("@AcademicQualification", model.AcademicQualification);
                cmd.Parameters.AddWithValue("@Language_one", model.Language_one);
                cmd.Parameters.AddWithValue("@Language_two", model.Language_two);
                cmd.Parameters.AddWithValue("@Language_three", model.Language_three);
                cmd.Parameters.AddWithValue("@SelfIntroduction_" + model.CurrentCulture, model.SelfIntroduction);
                cmd.Parameters.AddWithValue("@Note_" + model.CurrentCulture, model.Note);
                cmd.Parameters.AddWithValue("@Active", model.IsActive);
                cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedBy", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);

                if (model.ID == Guid.Empty)
                {
                    Guid employeeId = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", employeeId);
                    cmd.Parameters.AddWithValue("@StatementType", "Insert");
                    userModel.EmployeeID = employeeId;
                    createUser(model, userModel, userInfoService);
                }
                else {

                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@StatementType", "Update");

                    basemodel.CurrentUserID = model.CurrentUserID;
                    basemodel.CurrentCulture = model.CurrentCulture;
                    basemodel.ID =model.ID;
                  //  userModel = userInfoService.GetUserByEmployeeId(basemodel);
         
                }
          
                     cmd.ExecuteNonQuery();
                    _sqlConnService.CloseConnection();
             
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }
            finally{
                _sqlConnService.CloseConnection();
            }
            return model;
        }

        private bool createUser(EmployeeModel model, UserInfoModel userModel, IUserInfoService userInfoService)
        {
            bool userCreated = false;
            try
            {
                userModel.FirstName = model.Name;
                userModel.Email = model.Email;
                userModel.Username = model.EmployeeID;
                userModel.Password = model.Password;
                userModel.UserTypeId = (long)USERTYPE.Employee;
                userModel.RoleId = model.EmployeeTypeID;
                userModel.IsLocked = false;
                userModel.IsSuperAdmin = false;
                userModel.IsActive = true;
                userModel.DateOfBirth = model.BirthDate;
                userModel.CurrentCulture = model.CurrentCulture;
                userModel.CurrentUserID = model.CurrentUserID;
                userInfoService.SaveUserInfo(userModel);
                userCreated = true;
            }
            catch(Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }
            return userCreated;
        }

        public EmployeeModel UpdateEmployee(EmployeeModel model)
        {
            Employee employee, anEmployee;
            EmployeeModel updatedEmployee;

            try
            {
                employee = Mapper.Map<EmployeeModel, Employee>(model);
                anEmployee = _employeRepository.UpdateEmployee(employee);
                updatedEmployee = Mapper.Map<Employee, EmployeeModel>(anEmployee);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }
            return model;
        }
        
        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "Employee", message);
            return message;
        }

        public bool CheckEmployeeByEmployeeID(EmployeeModel model)
        {
           var  mapModel = Mapper.Map<EmployeeModel,Employee>(model);
            return  _employeRepository.CheckEmployeeByEmployeeID(mapModel);

        }
    }
}
